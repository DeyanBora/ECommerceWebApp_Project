using ECommerceWebApp.DataAccess.Repositories.Interfaces;
using ECommerceWebApp.Entities.Entities.Products;
using ECommerceWebApp.Shared.DTOs;

namespace ECommerceWebApp.DataAccess.Repositories.Implementations
{
    internal class ElasticRepository : IElasticRepository
    {
        public Task<string> BulkAddProductsAsync(IEnumerable<CreateProductDto> dtos, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> CreateProductAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteProductAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateProductAsync(UpdateProductDto dto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
