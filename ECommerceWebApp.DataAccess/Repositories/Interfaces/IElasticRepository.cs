using ECommerceWebApp.Entities.Entities.Products;
using ECommerceWebApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWebApp.DataAccess.Repositories.Interfaces
{
    public interface IElasticRepository 
    {
        Task<string> CreateProductAsync(int id, CancellationToken cancellationToken);
        Task<string> UpdateProductAsync(UpdateProductDto dto, CancellationToken cancellationToken);
        Task<string> DeleteProductAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken);
        Task<string> BulkAddProductsAsync(IEnumerable<CreateProductDto> dtos, CancellationToken cancellationToken);
    }
}
