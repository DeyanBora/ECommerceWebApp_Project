using ECommerceWebApp.DataAccess.Repositories.Interfaces;
using ECommerceWebApp.Entities.Entities.Products;
using ECommerceWebApp.Shared.DTOs.ProductDtos;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWebApp.Api.Endpoints;

public static class CategoriesEndpoints
{
    public static RouteGroupBuilder MapCategoriesEndpoint(this IEndpointRouteBuilder routes)
    {
        var group = routes.NewVersionedApi().MapGroup("/categories")
                        .HasApiVersion(1.0)
                        .WithParameterValidation()
                        .WithOpenApi()
                        .WithTags("Categories");

        // GET ALL CATEGORIES
        group.MapGet("/", async ([FromServices] ICategoriesRepository categoriesRepository) =>
        {
            return Results.Ok(await categoriesRepository.GetAllAsync());
        })
        .RequireAuthorization()
        .MapToApiVersion(1.0)
        .WithSummary("Gets All Categories")
        .WithDescription("Retrieves all categories available.");

        // GET CATEGORY BY ID
        group.MapGet("/{id}", async ([FromServices] ICategoriesRepository categoriesRepository, [FromRoute] int id) =>
        {
            var category = await categoriesRepository.GetAsync(id);
            return category is not null ? Results.Ok(category) : Results.NotFound();
        })
        .RequireAuthorization()
        .MapToApiVersion(1.0)
        .WithSummary("Get Category by ID")
        .WithDescription("Retrieves a specific category by its ID.");

        // CREATE CATEGORY
        group.MapPost("/", async ([FromServices] ICategoriesRepository categoriesRepository, [FromBody] CreateCategoryDto categoryDto) =>
        {
            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                Slug = categoryDto.Slug
            };

            await categoriesRepository.CreateAsync(category);
            return Results.Created($"/categories/{category.Id}", category);
        })
        .RequireAuthorization()
        .MapToApiVersion(1.0)
        .WithSummary("Create a Category")
        .WithDescription("Creates a new category.");

        // UPDATE CATEGORY
        group.MapPut("/{id}", async ([FromServices] ICategoriesRepository categoriesRepository, [FromRoute] int id, [FromBody] UpdateCategoryDto categoryDto) =>
        {
            var existingCategory = await categoriesRepository.GetAsync(id);
            if (existingCategory is null)
                return Results.NotFound();

            existingCategory.Name = categoryDto.Name;
            existingCategory.Description = categoryDto.Description;
            existingCategory.Slug = categoryDto.Slug;

            await categoriesRepository.UpdateAsync(existingCategory);
            return Results.NoContent();
        })
        .RequireAuthorization()
        .MapToApiVersion(1.0)
        .WithSummary("Update a Category")
        .WithDescription("Updates the properties of a category.");

        // DELETE CATEGORY
        group.MapDelete("/{id}", async ([FromServices] ICategoriesRepository categoriesRepository, [FromRoute] int id) =>
        {
            var category = await categoriesRepository.GetAsync(id);
            if (category is not null)
                await categoriesRepository.DeleteAsync(id);

            return Results.NoContent();
        })
        .RequireAuthorization()
        .MapToApiVersion(1.0)
        .WithSummary("Delete a Category")
        .WithDescription("Deletes a category by its ID.");

        return group;
    }
}