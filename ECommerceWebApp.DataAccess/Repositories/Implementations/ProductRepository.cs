using ECommerceWebApp.DataAccess.Data;
using ECommerceWebApp.DataAccess.Repositories.Interfaces;
using ECommerceWebApp.Entities.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ECommerceWebApp.DataAccess.Repositories.Implementations;

public class ProductRepository : IProductsRepository
{
    private readonly ECommerceContext dbContext;
    private readonly ILogger<ProductRepository> logger;

    public ProductRepository(ECommerceContext dbContext, ILogger<ProductRepository> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task<IEnumerable<Product>> GetAllAsyncWithPagination(int pageNumber, int pageSize, string? filter)
    {
        var skipCount = (pageNumber - 1) * pageSize;

        return await FilterProducts(filter)
                    .OrderBy(product => product.Id)
                    .Skip(skipCount)
                    .Take(pageSize)
                    .AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        var products = await dbContext.Products.AsNoTracking()
                        .Include(p => p.Category)
                        .Include(p => p.Brand)
                        .Include(p => p.Manufacturer)
                        .ToListAsync();

        return products;
    }

    public async Task<Product?> GetAsync(int id)
    {
        return await dbContext.Products.FindAsync(id);
    }

    public async Task CreateAsync(Product product)
    {
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        logger.LogInformation($"Created Product {product.Title}.");
    }

    public async Task UpdateAsync(Product addedProduct)
    {
        dbContext.Update(addedProduct);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        //Delete without loading into memory
        await dbContext.Products.Where(p => p.Id == id)
                                .ExecuteDeleteAsync();
    }

    public async Task<int> CountAsync(string? filter)
    {
        return await FilterProducts(filter).CountAsync();
    }

    private IQueryable<Product> FilterProducts(string? filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
        {
            return dbContext.Products;
        }
        return dbContext.Products.Where(product => product.Title.Contains(filter) || product.Description.Contains(filter));
    }

    public async Task<string> GetProductWithDetailsAsync(int productId)
    {
        var product = await dbContext.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Manufacturer)
            .Where(p => p.Id == productId)
            .Select(p => new
            {
                p.Id,
                p.ElasticId,
                p.ErpCode,
                p.Title,
                p.Description,
                p.CategoryId,
                p.BrandId,
                p.ManufacturerId,
                p.Stock,
                p.Price,
                p.ImageUrl,
                p.Slug,
                p.IsDeleted,
                p.CreatedDate,
                p.UpdatedDate,
                p.DeletedDate,
                p.CreatedBy,
                p.UpdatedBy,
                Category = new
                {
                    p.Category.Id,
                    p.Category.Name,
                    p.Category.Description,
                    p.Category.Slug,
                    p.Category.IsDeleted,
                    p.Category.CreatedDate,
                    p.Category.UpdatedDate,
                    p.Category.DeletedDate,
                    p.Category.CreatedBy,
                    p.Category.UpdatedBy
                },
                Brand = new
                {
                    p.Brand.Id,
                    p.Brand.Name,
                    p.Brand.LogoUrl,
                    p.Brand.Description,
                    p.Brand.Slug,
                    p.Brand.IsDeleted,
                    p.Brand.CreatedDate,
                    p.Brand.UpdatedDate,
                    p.Brand.DeletedDate,
                    p.Brand.CreatedBy,
                    p.Brand.UpdatedBy
                },
                Manufacturer = new
                {
                    p.Manufacturer.Id,
                    p.Manufacturer.Name,
                    p.Manufacturer.ContactInfo,
                    p.Manufacturer.Address,
                    p.Manufacturer.Slug,
                    p.Manufacturer.IsDeleted,
                    p.Manufacturer.CreatedDate,
                    p.Manufacturer.UpdatedDate,
                    p.Manufacturer.DeletedDate,
                    p.Manufacturer.CreatedBy,
                    p.Manufacturer.UpdatedBy
                }
            })
            .FirstOrDefaultAsync();

        if (product == null)
        {
            return JsonSerializer.Serialize(new { message = "Product not found" });
        }

        // Convert the result to JSON using System.Text.Json
        var options = new JsonSerializerOptions
        {
            WriteIndented = true, // Format JSON with indentation
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull // Ignore null values
        };
        return JsonSerializer.Serialize(product, options);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await dbContext.Set<Product>().AnyAsync(p => p.Id == id);
    }

    public Task<Guid> GetElasticIdAsync(int id)
    {
        return dbContext.Products.Where(p => p.Id == id)
        .Select(p => p.ElasticId)
        .FirstOrDefaultAsync();
    }
}
