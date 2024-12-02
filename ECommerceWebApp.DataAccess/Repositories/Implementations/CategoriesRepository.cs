using ECommerceWebApp.DataAccess.Data;
using ECommerceWebApp.DataAccess.Repositories.Interfaces;
using ECommerceWebApp.Entities.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerceWebApp.DataAccess.Repositories.Implementations;

public class CategoriesRepository : ICategoriesRepository
{
    private readonly ECommerceContext dbContext;
    private readonly ILogger<CategoriesRepository> logger;

    public CategoriesRepository(ECommerceContext dbContext, ILogger<CategoriesRepository> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await dbContext.Categories.AsNoTracking().ToListAsync();
    }

    public async Task<Category?> GetAsync(int id)
    {
        return await dbContext.Categories.FindAsync(id);
    }

    public async Task CreateAsync(Category category)
    {
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();

        logger.LogInformation($"Created Category {category.Name}.");
    }

    public async Task UpdateAsync(Category category)
    {
        dbContext.Categories.Update(category);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await dbContext.Categories.Where(c => c.Id == id).ExecuteDeleteAsync();
    }
}