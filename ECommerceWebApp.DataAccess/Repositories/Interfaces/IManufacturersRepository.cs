using ECommerceWebApp.Entities.Entities.Products;

namespace ECommerceWebApp.DataAccess.Repositories.Interfaces;

public interface IManufacturersRepository
{
    Task<IEnumerable<Manufacturer>> GetAllAsync();
    Task<Manufacturer?> GetAsync(int id);
    Task CreateAsync(Manufacturer manufacturer);
    Task UpdateAsync(Manufacturer manufacturer);
    Task DeleteAsync(int id);
}