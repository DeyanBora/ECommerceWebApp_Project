using ECommerceWebApp.DataAccess.Repositories.Interfaces;
using ECommerceWebApp.Entities.Entities.Products;
using ECommerceWebApp.Shared.DTOs.ProductDtos;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWebApp.Api.Endpoints;

public static class ManufacturersEndpoints
{
    public static RouteGroupBuilder MapManufacturersEndpoint(this IEndpointRouteBuilder routes)
    {
        var group = routes.NewVersionedApi().MapGroup("/manufacturers")
                        .HasApiVersion(1.0)
                        .WithParameterValidation()
                        .WithOpenApi()
                        .WithTags("Manufacturers");

        // GET ALL MANUFACTURERS
        group.MapGet("/", async ([FromServices] IManufacturersRepository manufacturersRepository) =>
        {
            return Results.Ok(await manufacturersRepository.GetAllAsync());
        })
        .RequireAuthorization()
        .MapToApiVersion(1.0)
        .WithSummary("Gets All Manufacturers")
        .WithDescription("Retrieves all manufacturers available.");

        // GET MANUFACTURER BY ID
        group.MapGet("/{id}", async ([FromServices] IManufacturersRepository manufacturersRepository, [FromRoute] int id) =>
        {
            var manufacturer = await manufacturersRepository.GetAsync(id);
            return manufacturer is not null ? Results.Ok(manufacturer) : Results.NotFound();
        })
        .RequireAuthorization()
        .MapToApiVersion(1.0)
        .WithSummary("Get Manufacturer by ID")
        .WithDescription("Retrieves a specific manufacturer by its ID.");

        // CREATE MANUFACTURER
        group.MapPost("/", async ([FromServices] IManufacturersRepository manufacturersRepository, [FromBody] CreateManufacturerDto manufacturerDto) =>
        {
            var manufacturer = new Manufacturer
            {
                Name = manufacturerDto.Name,
                ContactInfo = manufacturerDto.ContactInfo,
                Address = manufacturerDto.Address,
                Slug = manufacturerDto.Slug
            };

            await manufacturersRepository.CreateAsync(manufacturer);
            return Results.Created($"/manufacturers/{manufacturer.Id}", manufacturer);
        })
        .RequireAuthorization()
        .MapToApiVersion(1.0)
        .WithSummary("Create a Manufacturer")
        .WithDescription("Creates a new manufacturer.");

        // UPDATE MANUFACTURER
        group.MapPut("/{id}", async ([FromServices] IManufacturersRepository manufacturersRepository, [FromRoute] int id, [FromBody] UpdateManufacturerDto manufacturerDto) =>
        {
            var existingManufacturer = await manufacturersRepository.GetAsync(id);
            if (existingManufacturer is null)
                return Results.NotFound();

            existingManufacturer.Name = manufacturerDto.Name;
            existingManufacturer.ContactInfo = manufacturerDto.ContactInfo;
            existingManufacturer.Address = manufacturerDto.Address;
            existingManufacturer.Slug = manufacturerDto.Slug;

            await manufacturersRepository.UpdateAsync(existingManufacturer);
            return Results.NoContent();
        })
        .RequireAuthorization()
        .MapToApiVersion(1.0)
        .WithSummary("Update a Manufacturer")
        .WithDescription("Updates the properties of a manufacturer.");

        // DELETE MANUFACTURER
        group.MapDelete("/{id}", async ([FromServices] IManufacturersRepository manufacturersRepository, [FromRoute] int id) =>
        {
            var manufacturer = await manufacturersRepository.GetAsync(id);
            if (manufacturer is not null)
                await manufacturersRepository.DeleteAsync(id);

            return Results.NoContent();
        })
        .RequireAuthorization()
        .MapToApiVersion(1.0)
        .WithSummary("Delete a Manufacturer")
        .WithDescription("Deletes a manufacturer by its ID.");

        return group;
    }
}