using ECommerceWebApp.Entities.Entities.Products;

namespace ECommerceWebApp.DataAccess.Repositories.Interfaces;

public interface ICategoriesRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetAsync(int id);
    Task CreateAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(int id);
}