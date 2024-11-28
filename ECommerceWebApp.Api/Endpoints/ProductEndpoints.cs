using ECommerceWebApp.Api.Authorization;
using ECommerceWebApp.DataAccess.Repositories.Interfaces;
using ECommerceWebApp.Entities.Entities.Products;
using ECommerceWebApp.Shared.DTOs;
using ECommerceWebApp.Business.Extensions;
namespace ECommerceWebApp.Api.Endpoints;

public static class ProductEndpoints
{
    const string GetProductV1EndpointName = "GetProductV1";
    const string GetProductV2EndpointName = "GetProductV2";

    public static RouteGroupBuilder MapProductsEndpoint(this IEndpointRouteBuilder routes)
    {

        var group = routes.NewVersionedApi().MapGroup("/products")
                        .HasApiVersion(1.0)
                        .HasApiVersion(2.0)
                        .WithParameterValidation()
                        .WithOpenApi()
                        .WithTags("Products");

        // V1 GET ENDPOINTS
        
        group.MapGet("/", async (
                                IProductsRepository productsRepository, 
                                ILoggerFactory loggerFactory, 
                                [AsParameters]GetProductsDtoV1 request,
                                HttpContext http) =>
        {
            var totalCount = await productsRepository.CountAsync(request.Filter);
            http.Response.AddPaginationHeader(totalCount, request.PageSize);

            return Results.Ok((await productsRepository.GetAllAsync(
                request.PageNumber, 
                request.PageSize,
                request.Filter))
                .Select(product => product.ToDtoV1()));
        })
            .RequireAuthorization()
            .MapToApiVersion(1.0)
            .WithSummary("Gets All Products")
            .WithDescription("Gets all products by using filtering and pagination");

        group.MapGet("/{id}", async (IProductsRepository productsRepository, int id) =>
        {
            Product? product = await productsRepository.GetAsync(id);
            return product is not null ? Results.Ok(product.ToDtoV1()) : Results.NotFound();
        })
        .WithName(GetProductV1EndpointName)
        .RequireAuthorization(Policies.ReadAccess)
        .MapToApiVersion(1.0)
        .WithSummary("Gets product by id")
        .WithDescription("Gets the product that has specified id");
        
        //V2 GET ENDPOINTS
        group.MapGet("/", async (
                                IProductsRepository productsRepository, 
                                ILoggerFactory loggerFactory,
                                [AsParameters] GetProductsDtoV1 request,
                                HttpContext http) =>
        {
            var totalCount = await productsRepository.CountAsync(request.Filter);
            http.Response.AddPaginationHeader(totalCount, request.PageSize);

            return Results.Ok((await productsRepository.GetAllAsync(
                request.PageNumber, 
                request.PageSize, 
                request.Filter))
                .Select(product => product.ToDtoV2()));
        })
            .MapToApiVersion(2.0)
            .WithSummary("Gets All Products")
            .WithDescription("Gets all products by using filtering and pagination"); ; 

        group.MapGet("/{id}", async (IProductsRepository productsRepository, int id) =>
        {
            Product? product = await productsRepository.GetAsync(id);
            return product is not null ? Results.Ok(product.ToDtoV2()) : Results.NotFound();
        })
        .WithName(GetProductV2EndpointName)
        .RequireAuthorization(Policies.ReadAccess)
        .MapToApiVersion(2.0)
        .WithSummary("Gets product by id")
        .WithDescription("Gets the product that has specified id");

        //V1 POST ENDPOINTS
        group.MapPost("/", async (IProductsRepository productsRepository, CreateProductDto productDto) =>
        {
            Product product = new()
            {
                Title = productDto.Title,
                Description = productDto.Description,
                Price = productDto.Price,
                CategoryId = 1,
                ErpCode = "",
                ImageUri = "",
                BrandId = 1,
            };

            await productsRepository.CreateAsync(product); 
            return Results.CreatedAtRoute(GetProductV1EndpointName, new { id = product.Id }, product);
        })
        .RequireAuthorization(Policies.WriteAccess)
        .MapToApiVersion(1.0)
        .WithSummary("Creates a new product")
        .WithDescription("Creates a new product by specified properties");

        //V1 PUT ENDPOINTS
        group.MapPut("/{id}", async (IProductsRepository productsRepository, int id, UpdateProductDto updateProductDto) =>
        {
            Product? existingProduct = await productsRepository.GetAsync(id);
            if (existingProduct is null)
            {
                return Results.NotFound();
            }

            existingProduct.Title = updateProductDto.Title;
            existingProduct.ErpCode = "updateProductDto.ErpCode";
            existingProduct.Price = updateProductDto.Price;
            existingProduct.Description = updateProductDto.Description;
            existingProduct.ImageUri = "updateProductDto.ImageUri";
            existingProduct.CategoryId = 1;

            await productsRepository.UpdateAsync(existingProduct);
            return Results.NoContent();
        })
        .RequireAuthorization(Policies.WriteAccess).MapToApiVersion(1.0)
        .WithSummary("Updates a product")
        .WithDescription("Updates a product using given properties for the specified id");
        

        //V1 DELETE ENDPOINTS
        group.MapDelete("/{id}", async (IProductsRepository productsRepository, int id) =>
        {
            Product? product = await productsRepository.GetAsync(id);

            if (product is not null)
            {
                await productsRepository.DeleteAsync(id);
            }

            return Results.NoContent();
        })
        .RequireAuthorization(Policies.WriteAccess)
        .MapToApiVersion(1.0)
        .WithSummary("Deletes a product")
        .WithDescription("Deletes a product that has specified id");

        return group;
    }
}
