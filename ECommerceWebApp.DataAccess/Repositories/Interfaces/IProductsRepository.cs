using ECommerceWebApp.Entities.Entities.Products;

namespace ECommerceWebApp.DataAccess.Repositories.Interfaces
{
    public interface IProductsRepository
    {
        Task CreateAsync(Product product);
        Task DeleteAsync(int id);
        Task<Product?> GetAsync(int id);
        Task<IEnumerable<Product>> GetAllAsyncWithPagination(int pageNumber, int pagesize, string? filter);
        Task<IEnumerable<Product>> GetAllAsync();
        Task UpdateAsync(Product addedProduct);

        Task<int> CountAsync(string? filter);
        Task<string> GetProductWithDetailsAsync(int productId);

        Task<bool> ExistsAsync(int id);

        Task<Guid> GetElasticIdAsync(int id);


    }
}