using ECommerceWebApp.DataAccess.Repositories.Interfaces;
using ECommerceWebApp.Entities.Entities.Products;
using ECommerceWebApp.Shared.DTOs;
using ECommerceWebApp.Shared.DTOs.ProductDtos;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWebApp.Api.Endpoints;

public static class BrandsEndpoints
{
    public static RouteGroupBuilder MapBrandsEndpoint(this IEndpointRouteBuilder routes)
    {
        var group = routes.NewVersionedApi().MapGroup("/brands")
                        .HasApiVersion(1.0)
                        .WithParameterValidation()
                        .WithOpenApi()
                        .WithTags("Brands");

        // GET ALL BRANDS
        group.MapGet("/", async ([FromServices] IBrandsRepository brandsRepository) =>
        {
            return Results.Ok(await brandsRepository.GetAllAsync());
        })
        .RequireAuthorization()
        .MapToApiVersion(1.0)
        .WithSummary("Gets All Brands")
        .WithDescription("Retrieves all brands available.");

        // GET BRAND BY ID
        group.MapGet("/{id}", async ([FromServices] IBrandsRepository brandsRepository, int id) =>
        {
            var brand = await brandsRepository.GetAsync(id);
            return brand is not null ? Results.Ok(brand) : Results.NotFound();
        })
        .RequireAuthorization()
        .MapToApiVersion(1.0)
        .WithSummary("Get Brand by ID")
        .WithDescription("Retrieves a specific brand by its ID.");

        // CREATE BRAND
        group.MapPost("/", async ([FromServices] IBrandsRepository brandsRepository, [FromBody] CreateBrandDto brandDto) =>
        {
            var brand = new Brand
            {
                Name = brandDto.Name,
                Description = brandDto.Description,
                Slug = brandDto.Slug
            };

            await brandsRepository.CreateAsync(brand);
            return Results.Created($"/brands/{brand.Id}", brand);
        })
        .RequireAuthorization()
        .MapToApiVersion(1.0)
        .WithSummary("Create a Brand")
        .WithDescription("Creates a new brand.");


        // UPDATE BRAND
        group.MapPut("/{id}", async ([FromServices] IBrandsRepository brandsRepository, [FromRoute] int id, [FromBody] UpdateBrandDto brandDto) =>
        {
            var existingBrand = await brandsRepository.GetAsync(id);
            if (existingBrand is null)
                return Results.NotFound();

            existingBrand.Name = brandDto.Name;
            existingBrand.Description = brandDto.Description;
            existingBrand.Slug = brandDto.Slug;

            await brandsRepository.UpdateAsync(existingBrand);
            return Results.NoContent();
        })
        .RequireAuthorization()
        .MapToApiVersion(1.0)
        .WithSummary("Update a Brand")
        .WithDescription("Updates the properties of a brand.");

        // DELETE BRAND
        group.MapDelete("/{id}", async ([FromServices] IBrandsRepository brandsRepository, int id) =>
        {
            var brand = await brandsRepository.GetAsync(id);
            if (brand is not null)
                await brandsRepository.DeleteAsync(id);

            return Results.NoContent();
        })
        .RequireAuthorization()
        .MapToApiVersion(1.0)
        .WithSummary("Delete a Brand")
        .WithDescription("Deletes a brand by its ID.");

        return group;
    }
}