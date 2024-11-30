using ECommerceWebApp.Entities.Entities.Products;
using ECommerceWebApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWebApp.Business.Interfaces
{
    public interface IElasticApiClient
    {
        Task<Guid> CreateProductAsync(CreateProductDto productDto, CancellationToken cancellationToken);
        Task UpdateProductAsync(UpdateProductDto productDto, CancellationToken cancellationToken);
        Task DeleteProductByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken);
        Task BulkAddProductsAsync(IEnumerable<CreateProductDto> products, CancellationToken cancellationToken);
    }
}
