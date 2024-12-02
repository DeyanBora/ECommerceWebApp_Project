using ECommerceWebApp.Shared.DTOs.ProductDtos;

namespace ECommerceWebApp.Business.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync(GetProductsDto request);
        Task<int> GetTotalCountAsync(string? filter); // Newly added method
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(CreateProductDto productDto);
        Task<bool> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
        Task<bool> DeleteProductAsync(int id);
        Task<string?> GetProductForElasticSearchAsync(int id);
    }
}