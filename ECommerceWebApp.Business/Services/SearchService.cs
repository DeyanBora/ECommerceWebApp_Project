using ECommerceWebApp.Business.Interfaces;
using ECommerceWebApp.Entities.Entities.Products;
using ECommerceWebApp.Shared.DTOs.Search;
using System.Net.Http.Json;

namespace ECommerceWebApp.Business.Services;
public class SearchService : ISearchService
{
    private readonly HttpClient _httpClient;

    public SearchService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string query, CancellationToken cancellationToken = default)
    {
        var searchPayload = new { Query = query };

        var response = await _httpClient.PostAsJsonAsync("/products/getall", searchPayload, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to search products: {response.ReasonPhrase}");
        }

        var products = await response.Content.ReadFromJsonAsync<IEnumerable<Product>>(cancellationToken: cancellationToken);
        return products ?? Enumerable.Empty<Product>();
    }

    public async Task<bool> BulkAddProductsAsync(IEnumerable<CreateElasticProductDto> products, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("/products/bulkAdd", products, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to bulk add products: {response.ReasonPhrase}");
        }

        var result = await response.Content.ReadFromJsonAsync<dynamic>(cancellationToken: cancellationToken);
        return result?.indexedCount > 0;
    }

    public async Task<string> CreateProductAsync(CreateElasticProductDto product, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("/products/create", product, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to create product: {response.ReasonPhrase}");
        }

        var result = await response.Content.ReadFromJsonAsync<dynamic>(cancellationToken: cancellationToken);
        return result?.id;
    }

    public async Task<bool> UpdateProductAsync(UpdateElasticProductDto product, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync("/products/update", product, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to update product: {response.ReasonPhrase}");
        }

        return true;
    }

    public async Task<bool> DeleteProductByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync($"/products/deleteById?id={id}", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to delete product: {response.ReasonPhrase}");
        }

        return true;
    }
}