using ECommerceWebApp.Business.Interfaces;
using ECommerceWebApp.Entities.Entities.Products;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWebApp.Api.Endpoints
{
    public static class ElasticEndpoints
    {
        public static void MapElasticEndpoints(this IEndpointRouteBuilder endpoints)
        {
            // Define the versioned API group for Elasticsearch endpoints
            var elasticGroup = endpoints.NewVersionedApi().MapGroup("/elastic")
                .HasApiVersion(1.0)
                .WithParameterValidation()
                .WithOpenApi()
                .WithTags("Elasticsearch");

            // Create Product
            elasticGroup.MapPost("/products", async (int id, CancellationToken cancellationToken, [FromServices] IElasticApiService elasticApiService) =>
            {
                try
                {
                    var result = await elasticApiService.CreateProductAsync(id, cancellationToken);
                    return Results.Ok(new { Message = "Product created successfully.", Data = result });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error creating product: {ex.Message}");
                }
            })
            .RequireAuthorization()
            .WithSummary("Creates a product in Elasticsearch")
            .WithDescription("Creates a new product in Elasticsearch with the provided details.");

            // Update Product
            elasticGroup.MapPut("/products", async (int id, CancellationToken cancellationToken, [FromServices] IElasticApiService elasticApiService) =>
            {
                try
                {
                    var result = await elasticApiService.UpdateProductAsync(id, cancellationToken);
                    return Results.Ok(new { Message = "Product updated successfully.", Data = result });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error updating product: {ex.Message}");
                }
            })
                .RequireAuthorization()
            .WithSummary("Updates a product in Elasticsearch")
            .WithDescription("Updates an existing product in Elasticsearch with the provided details.");

            // Delete Product
            elasticGroup.MapDelete("/products/{id:guid}", async (Guid id, CancellationToken cancellationToken, [FromServices] IElasticApiService elasticApiService) =>
            {
                try
                {
                    var result = await elasticApiService.DeleteProductAsync(id, cancellationToken);
                    return Results.Ok(new { Message = "Product deleted successfully.", Data = result });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error deleting product: {ex.Message}");
                }
            })
                .RequireAuthorization()
            .WithSummary("Deletes a product in Elasticsearch")
            .WithDescription("Deletes a product from Elasticsearch using its ID.");

            // Get All Products
            elasticGroup.MapGet("/products", async (CancellationToken cancellationToken, [FromServices] IElasticApiService elasticApiService, int size, int page, string? filter) =>
            {
                try
                {
                    var products = await elasticApiService.GetAllProductsAsync(size, page, filter, cancellationToken);
                    return Results.Ok(products);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error retrieving products: {ex.Message}");
                }
            })
                .RequireAuthorization()
            .WithSummary("Retrieves all products in Elasticsearch")
            .WithDescription("Fetches all products currently stored in Elasticsearch.");

            // Bulk Add Products
            elasticGroup.MapPost("/products/bulk", async (IEnumerable<Product> products, CancellationToken cancellationToken, [FromServices] IElasticApiService elasticApiService) =>
            {
                try
                {
                    var result = await elasticApiService.BulkAddProductsAsync(products, cancellationToken);
                    return Results.Ok(new { Message = "Bulk products added successfully.", Data = result });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error adding products in bulk: {ex.Message}");
                }
            })
                .RequireAuthorization()
            .WithSummary("Bulk add products in Elasticsearch")
            .WithDescription("Adds multiple products to Elasticsearch in a single operation.");



            // ReIndex Products
            elasticGroup.MapGet("/products/reIndex", async (CancellationToken cancellationToken, [FromServices] IElasticApiService elasticApiService) =>
            {
                try
                {
                    var result = await elasticApiService.ReIndexAll(cancellationToken);
                    return Results.Ok(new { Message = "Re Indexed successfully.", Data = result });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error ReIndex: {ex.Message}");
                }
            })
                .RequireAuthorization()
            .WithSummary("ReIndex products to Elasticsearch")
            .WithDescription("ReIndex products to Elasticsearch in a single operation.");
        }
    }
}