using ECommerceWebApp.Business.Interfaces;
using ECommerceWebApp.Business.Services;
using ECommerceWebApp.Business.Services.Interfaces;
using ECommerceWebApp.Shared.DTOs;
using System.Threading;

namespace ECommerceWebApp.Api.Endpoints;

public static class ProductEndpoints
{
    const string GetProductV1EndpointName = "GetProductV1";

    public static RouteGroupBuilder MapProductsEndpoint(this IEndpointRouteBuilder routes)
    {
        var group = routes.NewVersionedApi().MapGroup("/products")
                        .HasApiVersion(1.0)
                        .HasApiVersion(2.0)
                        .WithParameterValidation()
                        .WithOpenApi()
                        .WithTags("Products");

        // V1 GET ALL PRODUCTS
        group.MapGet("/", async (
                                IProductService productService,
                                ILoggerFactory loggerFactory,
                                [AsParameters] GetProductsDto request,
                                HttpContext http) =>
        {
            var products = await productService.GetAllProductsAsync(request);
            var totalCount = await productService.GetTotalCountAsync(request.Filter); // You may need to add this method
            http.Response.AddPaginationHeader(totalCount, request.PageSize);
            return Results.Ok(products);
        })
            .RequireAuthorization()
            .MapToApiVersion(1.0)
            .WithSummary("Gets All Products")
            .WithDescription("Gets all products by using filtering and pagination");

        // V1 GET PRODUCT BY ID
        group.MapGet("/{id}", async (IProductService productService, int id) =>
        {
            var product = await productService.GetProductByIdAsync(id);
            return product is not null ? Results.Ok(product) : Results.NotFound();
        })
        .WithName(GetProductV1EndpointName)
        .RequireAuthorization()
        .MapToApiVersion(1.0)
        .WithSummary("Gets product by id")
        .WithDescription("Gets the product that has specified id");

        // V1 POST CREATE PRODUCT
        group.MapPost("/", async (IProductService productService, IElasticApiService elasticApiService, CreateProductDto productDto, CancellationToken cancellationToken) =>
        {
            // Validate the incoming DTO
            if (string.IsNullOrWhiteSpace(productDto.ErpCode) || string.IsNullOrWhiteSpace(productDto.Title) ||
                string.IsNullOrWhiteSpace(productDto.Description) || string.IsNullOrWhiteSpace(productDto.ImageUrl))
            {
                return Results.BadRequest("All required fields must be provided.");
            }

            try
            {
                var createdProduct = await productService.CreateProductAsync(productDto);
                //Send to ElasticSearch
                var elasticResponse = await elasticApiService.CreateProductAsync(createdProduct.Id, cancellationToken);

                var responsePayload = new
                {
                    CreatedProduct = createdProduct,
                    ElasticResponse = elasticResponse
                };

                return Results.CreatedAtRoute(GetProductV1EndpointName, new { id = createdProduct.Id }, responsePayload);
            }
            catch (Exception ex)
            {
                return Results.Problem("An error occurred while creating the product: " + ex.Message);
            }
        })
        .RequireAuthorization()
        .MapToApiVersion(1.0)
        .WithSummary("Creates a new product")
        .WithDescription("Creates a new product by specified properties");

        // V1 PUT UPDATE PRODUCT
        group.MapPut("/{id}", async (IProductService productService, int id, IElasticApiService elasticApiService, UpdateProductDto updateProductDto, CancellationToken cancellationToken) =>
        {
            var success = await productService.UpdateProductAsync(id, updateProductDto);

            var elasticResponse = await elasticApiService.UpdateProductAsync(id, cancellationToken);

            var responsePayload = new
            {
                UpdatedProduct = success,
                ElasticResponse = elasticResponse
            };
            
            return success ? Results.Ok(responsePayload) : Results.NotFound();
        })
        .RequireAuthorization()
        .MapToApiVersion(1.0)
        .WithSummary("Updates a Product")
        .WithDescription("Updates a product using given properties for the specified id");

        // V1 DELETE PRODUCT
        group.MapDelete("/{id}", async (IProductService productService, IElasticApiService elasticApiService, int id) =>
        {
            var success = await productService.DeleteProductAsync(id);
            var elasticResponse = await elasticApiService.DeleteProductAtElasticAsync(id, CancellationToken.None);
            return Results.NoContent(); // Optionally, return NotFound() if not successful
        })
        .RequireAuthorization()
        .MapToApiVersion(1.0)
        .WithSummary("Deletes a product")
        .WithDescription("Deletes a product that has specified id");

        // V1 GET PRODUCT FOR ELASTICSEARCH
        group.MapGet("/e/{id}", async (IProductService productService, int id) =>
        {
            var product = await productService.GetProductForElasticSearchAsync(id);
            return product is not null ? Results.Ok(product) : Results.NotFound();
        })
        .WithName("Elastic String Getter")
        .RequireAuthorization()
        .MapToApiVersion(1.0)
        .WithSummary("Gets product by id for ElasticSearch type string")
        .WithDescription("Gets the product that has specified id ElasticSearch type string");

        return group;
    }
}

////V2 GET ENDPOINTS
//group.MapGet("/", async(
//                        IProductsRepository productsRepository,
//                        ILoggerFactory loggerFactory,
//                        [AsParameters] GetProductsDtoV1 request,
//                        HttpContext http) =>
//        {
//            var totalCount = await productsRepository.CountAsync(request.Filter);
//http.Response.AddPaginationHeader(totalCount, request.PageSize);

//            return Results.Ok((await productsRepository.GetAllAsync(
//                request.PageNumber,
//                request.PageSize,
//                request.Filter))
//                .Select(product => product.ToDtoV2()));
//        })
//            .MapToApiVersion(2.0)
//            .WithSummary("Gets All Products")
//            .WithDescription("Gets all products by using filtering and pagination"); ;

//group.MapGet("/{id}", async (IProductsRepository productsRepository, int id) =>
//{
//    Product? product = await productsRepository.GetAsync(id);
//    return product is not null ? Results.Ok(product.ToDtoV2()) : Results.NotFound();
//})
//.WithName(GetProductV2EndpointName)
//.RequireAuthorization()
//.MapToApiVersion(2.0)
//.WithSummary("Gets product by id")
//.WithDescription("Gets the product that has specified id");
