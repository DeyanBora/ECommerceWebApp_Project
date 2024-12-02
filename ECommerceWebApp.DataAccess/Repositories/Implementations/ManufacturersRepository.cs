using ECommerceWebApp.DataAccess.Data;
using ECommerceWebApp.DataAccess.Repositories.Interfaces;
using ECommerceWebApp.Entities.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerceWebApp.DataAccess.Repositories.Implementations;

public class ManufacturersRepository : IManufacturersRepository
{
    private readonly ECommerceContext dbContext;
    private readonly ILogger<ManufacturersRepository> logger;

    public ManufacturersRepository(ECommerceContext dbContext, ILogger<ManufacturersRepository> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task<IEnumerable<Manufacturer>> GetAllAsync()
    {
        return await dbContext.Manufacturers.AsNoTracking().ToListAsync();
    }

    public async Task<Manufacturer?> GetAsync(int id)
    {
        return await dbContext.Manufacturers.FindAsync(id);
    }

    public async Task CreateAsync(Manufacturer manufacturer)
    {
        manufacturer.CreatedDate = DateTime.UtcNow;
        dbContext.Manufacturers.Add(manufacturer);
        await dbContext.SaveChangesAsync();

        logger.LogInformation($"Created Manufacturer {manufacturer.Name}.");
    }

    public async Task UpdateAsync(Manufacturer manufacturer)
    {
        manufacturer.UpdatedDate = DateTime.UtcNow;
        dbContext.Manufacturers.Update(manufacturer);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await dbContext.Manufacturers.Where(m => m.Id == id).ExecuteDeleteAsync();
    }
}