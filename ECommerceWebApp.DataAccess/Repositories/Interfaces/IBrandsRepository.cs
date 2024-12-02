using ECommerceWebApp.Entities.Entities.Products;

namespace ECommerceWebApp.DataAccess.Repositories.Interfaces;

public interface IBrandsRepository
{
    Task<IEnumerable<Brand>> GetAllAsync();
    Task<Brand?> GetAsync(int id);
    Task CreateAsync(Brand brand);
    Task UpdateAsync(Brand brand);
    Task DeleteAsync(int id);
}