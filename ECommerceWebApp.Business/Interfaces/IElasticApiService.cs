using ECommerceWebApp.Entities.Entities.Products;
using ECommerceWebApp.Shared.DTOs;
using ECommerceWebApp.Shared.DTOs.ElasticSearchDtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerceWebApp.Business.Interfaces
{
    public interface IElasticApiService
    {
        Task<string> CreateProductAsync(int id, CancellationToken cancellationToken);
        Task<string> UpdateProductAsync(int id, CancellationToken cancellationToken);
        Task<string> DeleteProductAsync(Guid id, CancellationToken cancellationToken);
        Task<string> DeleteProductAtElasticAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<ElasticProductDto>> GetAllProductsAsync(int size, int page, string? filter, CancellationToken cancellationToken);
        Task<string> BulkAddProductsAsync(IEnumerable<Product> products, CancellationToken cancellationToken);
        Task<Guid> GetProductElasticIdAsync(int id);

        Task<string> ReIndexAll(CancellationToken cancellationToken);
    }
}