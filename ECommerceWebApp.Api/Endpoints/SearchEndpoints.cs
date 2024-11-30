using ECommerceWebApp.Business.Interfaces;
using ECommerceWebApp.Entities.Entities.Search;
using ECommerceWebApp.Shared.DTOs;
using ECommerceWebApp.Shared.DTOs.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ECommerceWebApp.Api.Endpoints;

public static class SearchEndpoint
{
    public static RouteGroupBuilder MapSearchEndpoint(this IEndpointRouteBuilder routes)
    {
        var searchGroup = routes.NewVersionedApi().MapGroup("/e-search")
            .HasApiVersion(1.0)
            .WithParameterValidation()
            .WithOpenApi()
            .WithTags("Elastic Search");

        // Search Endpoint
        searchGroup.MapGet("/search", async (ISearchService searchService, [FromQuery] string query, [FromQuery] int page, [FromQuery] int pageSize, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("SearchEndpoints");

            if (string.IsNullOrWhiteSpace(query))
            {
                logger.LogWarning("Search query is missing or empty.");
            }

            if (page < 1)
            {
                logger.LogWarning("Invalid page number: {Page}. Defaulting to page 1.", page);
                page = 1;
            }

            if (pageSize <= 0)
            {
                logger.LogWarning("Invalid page size: {PageSize}. Defaulting to 10.", pageSize);
                pageSize = 10;
            }

            var parameters = new SearchParameters
            {
                Query = query,
                Page = page,
                PageSize = pageSize
            };

            try
            {
                logger.LogInformation("Executing search: Query='{Query}', Page={Page}, PageSize={PageSize}.", query, page, pageSize);
                var results = await searchService.SearchProductsAsync(query);

                logger.LogInformation("Search completed. Found {Count} results.", results.Count());

                return Results.Ok(results);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while executing search.");
                return Results.Problem("An error occurred while processing the search request.");
            }
        })
        .MapToApiVersion(1.0)
        .WithSummary("Search products in Elasticsearch.")
        .WithDescription("Performs a search in Elasticsearch using the provided query, page number, and page size.");

        // Create Product Endpoint
        searchGroup.MapPost("/create", async (ISearchService searchService, CreateElasticProductDto product, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("SearchEndpoints");
            try
            {
                var productId = await searchService.CreateProductAsync(product);
                logger.LogInformation("Product created successfully with ID: {Id}.", productId);
                return Results.Ok(new { Id = productId });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while creating a product.");
                return Results.Problem("An error occurred while creating the product.");
            }
        })
        .MapToApiVersion(1.0)
        .WithSummary("Create a new product.")
        .WithDescription("Adds a new product to the Elasticsearch index.");

        // Update Product Endpoint
        searchGroup.MapPut("/update", async (ISearchService searchService, UpdateElasticProductDto product, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("SearchEndpoints");
            try
            {
                var isSuccess = await searchService.UpdateProductAsync(product);
                logger.LogInformation("Product updated successfully with ID: {Id}.", product.Id);
                return isSuccess ? Results.Ok("Product updated successfully.") : Results.BadRequest("Failed to update product.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while updating product with ID: {Id}.", product.Id);
                return Results.Problem("An error occurred while updating the product.");
            }
        })
        .MapToApiVersion(1.0)
        .WithSummary("Update an existing product.")
        .WithDescription("Updates the details of an existing product in the Elasticsearch index.");

        // Delete Product Endpoint
        searchGroup.MapDelete("/delete", async (ISearchService searchService, [FromQuery] Guid id, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("SearchEndpoints");
            try
            {
                var isSuccess = await searchService.DeleteProductByIdAsync(id);
                logger.LogInformation("Product deleted successfully with ID: {Id}.", id);
                return isSuccess ? Results.Ok("Product deleted successfully.") : Results.BadRequest("Failed to delete product.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while deleting product with ID: {Id}.", id);
                return Results.Problem("An error occurred while deleting the product.");
            }
        })
        .MapToApiVersion(1.0)
        .WithSummary("Delete a product by ID.")
        .WithDescription("Deletes a product from the Elasticsearch index using its ID.");

        // Bulk Add Products Endpoint
        searchGroup.MapPost("/bulk-add", async (ISearchService searchService, IEnumerable<CreateElasticProductDto> products, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("SearchEndpoints");
            try
            {
                var isSuccess = await searchService.BulkAddProductsAsync(products);
                logger.LogInformation("Bulk add operation completed successfully. Products count: {Count}.", products.Count());
                return isSuccess ? Results.Ok("Products added successfully.") : Results.BadRequest("Failed to add products.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while performing bulk add operation.");
                return Results.Problem("An error occurred while adding the products.");
            }
        })
        .MapToApiVersion(1.0)
        .WithSummary("Bulk add products.")
        .WithDescription("Adds multiple products to the Elasticsearch index in a single operation.");

        return searchGroup;
    }
}