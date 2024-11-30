using ECommerceWebApp.Entities.Entities.Products;
using ECommerceWebApp.Entities.Entities.Search;
using ECommerceWebApp.Shared.DTOs;
using ECommerceWebApp.Shared.DTOs.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWebApp.Business.Interfaces
{
    public interface ISearchService
    {
        Task<IEnumerable<Product>> SearchProductsAsync(string query, CancellationToken cancellationToken = default);
        Task<bool> BulkAddProductsAsync(IEnumerable<CreateElasticProductDto> products, CancellationToken cancellationToken = default);
        Task<string> CreateProductAsync(CreateElasticProductDto product, CancellationToken cancellationToken = default);
        Task<bool> UpdateProductAsync(UpdateElasticProductDto product, CancellationToken cancellationToken = default);
        Task<bool> DeleteProductByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
