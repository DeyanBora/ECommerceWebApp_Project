using ECommerceWebApp.Business.Interfaces;
using ECommerceWebApp.Entities.Entities.Products;
using ECommerceWebApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerceWebApp.Business.Services
{
    public class ElasticApiClient : IElasticApiClient
    {
        private readonly HttpClient _httpClient;

        public ElasticApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Guid> CreateProductAsync(CreateProductDto productDto, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync("/products/create", productDto, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var createResponse = JsonSerializer.Deserialize<CreateResponse>(responseContent);

            return Guid.Parse(createResponse.Id);
        }

        public async Task UpdateProductAsync(UpdateProductDto productDto, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PutAsJsonAsync("/products/update", productDto, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProductByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var requestUri = $"/products/deleteById?id={id}";
            var response = await _httpClient.DeleteAsync(requestUri, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsync("/products/getall", null, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var products = JsonSerializer.Deserialize<IEnumerable<Product>>(responseContent);

            return products;
        }

        public async Task BulkAddProductsAsync(IEnumerable<CreateProductDto> products, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync("/products/bulkAdd", products, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
    }

    // Additional classes to deserialize responses if necessary
    public class CreateResponse
    {
        public string Id { get; set; }
    }
}
