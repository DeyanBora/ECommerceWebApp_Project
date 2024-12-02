using ECommerceWebApp.DataAccess.Data;
using ECommerceWebApp.DataAccess.Repositories.Interfaces;
using ECommerceWebApp.Entities.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerceWebApp.DataAccess.Repositories.Implementations;

public class BrandsRepository : IBrandsRepository
{
    private readonly ECommerceContext dbContext;
    private readonly ILogger<BrandsRepository> logger;

    public BrandsRepository(ECommerceContext dbContext, ILogger<BrandsRepository> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task<IEnumerable<Brand>> GetAllAsync()
    {
        return await dbContext.Brands.AsNoTracking().ToListAsync();
    }

    public async Task<Brand?> GetAsync(int id)
    {
        return await dbContext.Brands.FindAsync(id);
    }

    public async Task CreateAsync(Brand brand)
    {
        brand.CreatedDate = DateTime.UtcNow;
        dbContext.Brands.Add(brand);
        await dbContext.SaveChangesAsync();

        logger.LogInformation($"Created Brand {brand.Name}.");
    }

    public async Task UpdateAsync(Brand brand)
    {
        brand.UpdatedDate = DateTime.UtcNow;
        dbContext.Brands.Update(brand);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await dbContext.Brands.Where(b => b.Id == id).ExecuteDeleteAsync();
    }
}