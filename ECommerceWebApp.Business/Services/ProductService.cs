using ECommerceWebApp.Business.Extensions;
using ECommerceWebApp.Business.Services.Interfaces;
using ECommerceWebApp.DataAccess.Repositories.Interfaces;
using ECommerceWebApp.Entities.Entities.Products;
using ECommerceWebApp.Shared.DTOs.ProductDtos;
using Microsoft.Extensions.Logging;

namespace ECommerceWebApp.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductsRepository _productsRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductsRepository productsRepository, ILogger<ProductService> logger)
        {
            _productsRepository = productsRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync(GetProductsDto request)
        {
            var products = await _productsRepository.GetAllAsyncWithPagination(request.PageNumber, request.PageSize, request.Filter);
            return products.Select(p => p.ToDto());
        }

        public async Task<int> GetTotalCountAsync(string? filter)
        {
            try
            {
                int count = await _productsRepository.CountAsync(filter);
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting total product count.");
                throw; // Optionally, handle or wrap the exception as needed
            }
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _productsRepository.GetAsync(id);
            return product?.ToDto();
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto productDto)
        {
            var product = new Product
            {
                ElasticId = Guid.NewGuid(),
                ErpCode = productDto.ErpCode,
                Title = productDto.Title,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock,
                CategoryId = productDto.CategoryId,
                BrandId = productDto.BrandId,
                ManufacturerId = productDto.ManufacturerId,
                ImageUrl = productDto.ImageUrl,
                Slug = productDto.Slug
            };

            
            await _productsRepository.CreateAsync(product);
            return product.ToDto();
        }

        public async Task<bool> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
        {
            var existingProduct = await _productsRepository.GetAsync(id);
            if (existingProduct == null)
                return false;

            // Map properties from DTO to the entity
            existingProduct.ErpCode = updateProductDto.ErpCode;
            existingProduct.Title = updateProductDto.Title;
            existingProduct.Description = updateProductDto.Description;
            existingProduct.Price = updateProductDto.Price;
            existingProduct.Stock = updateProductDto.Stock;
            existingProduct.CategoryId = updateProductDto.CategoryId;
            existingProduct.BrandId = updateProductDto.BrandId;
            existingProduct.ManufacturerId = updateProductDto.ManufacturerId;
            existingProduct.ImageUrl = updateProductDto.ImageUrl;

            // Optionally, generate a slug if provided
            if (!string.IsNullOrEmpty(updateProductDto.Slug))
                existingProduct.Slug = updateProductDto.Slug;

            // Call the repository's update method
            await _productsRepository.UpdateAsync(existingProduct);
            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productsRepository.GetAsync(id);
            if (product == null)
                return false;

            await _productsRepository.DeleteAsync(id);
            return true;
        }

        public async Task<string?> GetProductForElasticSearchAsync(int id)
        {
            return await _productsRepository.GetProductWithDetailsAsync(id);
        }
    }
}