using ECommerceWebApp.DataAccess.Data;
using ECommerceWebApp.DataAccess.Repositories.Interfaces;
using ECommerceWebApp.Entities.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

    public async Task<IEnumerable<Product>> GetAllAsync(int pageNumber, int pageSize, string? filter)
    {
        var skipCount = (pageNumber - 1) * pageSize;

        return await FilterProducts(filter)
                    .OrderBy(product => product.Id)
                    .Skip(skipCount)
                    .Take(pageSize)
                    .AsNoTracking().ToListAsync();
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
}
