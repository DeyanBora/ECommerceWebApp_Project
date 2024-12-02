using ECommerceWebApp.Business.Interfaces;
using ECommerceWebApp.DataAccess.Repositories.Implementations;
using ECommerceWebApp.DataAccess.Repositories.Interfaces;
using ECommerceWebApp.Entities.Entities.Products;
using ECommerceWebApp.Shared.DTOs;
using ECommerceWebApp.Shared.DTOs.ElasticSearchDtos;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace ECommerceWebApp.Business.Services;
public class ElasticApiService : IElasticApiService
{
    private readonly HttpClient _httpClient;
    private readonly IProductsRepository _productsRepository;
    public ElasticApiService(HttpClient httpClient, IProductsRepository productsRepository)
    {
        _productsRepository = productsRepository;
        _httpClient = httpClient;
    }

    public async Task<string> CreateProductAsync(int id, CancellationToken cancellationToken)
    {
        if (!await _productsRepository.ExistsAsync(id))
        {
            throw new InvalidOperationException($"Product with ID {id} does not exist.");
        }

        _httpClient.BaseAddress = new Uri("http://localhost:5118");

        // Get the product details from the database
        string elasticString = await GetProductForElasticSearchAsync(id);

        // Use HttpContent to send the JSON string directly
        var content = new StringContent(elasticString, Encoding.UTF8, "application/json");
        //Send to Elasticsearch
        var response = await _httpClient.PostAsync("/products/create", content, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> UpdateProductAsync(int id, CancellationToken cancellationToken)
    {
        string elasticString = await GetProductForElasticSearchAsync(id);

        // Use HttpContent to send the JSON string directly
        var content = new StringContent(elasticString, Encoding.UTF8, "application/json");

        _httpClient.BaseAddress = new Uri("http://localhost:5118");
        var response = await _httpClient.PutAsync("/products/update", content, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> DeleteProductAsync(Guid id, CancellationToken cancellationToken)
    {
        _httpClient.BaseAddress = new Uri("http://localhost:5118");
        var response = await _httpClient.DeleteAsync($"/products/deleteById?id={id}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> DeleteProductAtElasticAsync(int id, CancellationToken cancellationToken)
    {
        var elasticId = await GetProductElasticIdAsync(id);

        _httpClient.BaseAddress = new Uri("http://localhost:5118");
        var response = await _httpClient.DeleteAsync($"/products/deleteById?id={elasticId}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<IEnumerable<ElasticProductDto>> GetAllProductsAsync(int size, int page, string? filter,CancellationToken cancellationToken)
    {
        _httpClient.BaseAddress = new Uri("http://localhost:5118");

        var queryString = $"?size={size}&page={page}&filter={Uri.EscapeDataString(filter ?? string.Empty)}";

        // Send the GET request
        var response = await _httpClient.PostAsync($"/products/getall{queryString}", null,cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<ElasticProductDto>>(cancellationToken: cancellationToken);
    }

    public async Task<string> BulkAddProductsAsync(IEnumerable<Product> products, CancellationToken cancellationToken)
    {
        _httpClient.BaseAddress = new Uri("http://localhost:5118");
        var response = await _httpClient.PostAsJsonAsync("/products/bulkAdd", products, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    private async Task<string?> GetProductForElasticSearchAsync(int id)
    {
        return await _productsRepository.GetProductWithDetailsAsync(id);
    }
    public async Task<Guid> GetProductElasticIdAsync(int id)
    {
        return await _productsRepository.GetElasticIdAsync(id);
    }

    public async Task<string> ReIndexAll(CancellationToken cancellationToken)
    {
        _httpClient.BaseAddress = new Uri("http://localhost:5118");
        var a = await _productsRepository.GetAllAsync();
        var response = await _httpClient.PostAsJsonAsync("/products/bulkAdd",await _productsRepository.GetAllAsync(), cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}