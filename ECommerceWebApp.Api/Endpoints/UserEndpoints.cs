using Microsoft.AspNetCore.Mvc;
using ECommerceWebApp.Business.Interfaces;
using ECommerceWebApp.Shared.DTOs;

namespace ECommerceWebApp.Api.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var userGroup = routes.NewVersionedApi().MapGroup("/users")
            .HasApiVersion(1.0)
            .WithParameterValidation()
            .WithOpenApi()
            .WithTags("Users");

        // Get users with pagination
        userGroup.MapGet("/", async ([FromServices] IUserService userService, int pageNumber, int pageSize, string? filter) =>
        {
            var (totalCount, users) = await userService.GetUsersWithPaginationAsync(pageNumber, pageSize, filter);
            return Results.Ok(new { TotalCount = totalCount, Users = users });
        })
        .MapToApiVersion(1.0)
        .WithSummary("Fetches paginated users")
        .WithDescription("Fetches users with pagination and optional filtering.");

        // Get user by ID
        userGroup.MapGet("/{id:int}", async ([FromServices] IUserService userService, int id) =>
        {
            var user = await userService.GetUserByIdAsync(id);
            return user != null ? Results.Ok(user) : Results.NotFound(new { Message = "User not found." });
        })
        .MapToApiVersion(1.0)
        .WithSummary("Fetches a user by ID")
        .WithDescription("Retrieves details of a specific user by their ID.");

        // Create a user
        userGroup.MapPost("/", async ([FromServices] IUserService userService, [FromBody] UserRegistrationRequestDto userDto) =>
        {
            try
            {

                var user = await userService.CreateUserAsync(userDto);
                return Results.Created($"/users/{user.Id}", user);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { Message = ex.Message });
            }
        })
        .MapToApiVersion(1.0)
        .WithSummary("Creates a new user")
        .WithDescription("Creates a new user and assigns roles.");

        // Update a user
        userGroup.MapPut("/{id:int}", async ([FromServices] IUserService userService, int id, [FromBody] UserUpdateDto userUpdateDto) =>
        {
            var updatedUser = await userService.UpdateUserAsync(id, userUpdateDto);
            return updatedUser != null ? Results.Ok(updatedUser) : Results.NotFound(new { Message = "User not found." });
        })
        .MapToApiVersion(1.0)
        .WithSummary("Updates a user")
        .WithDescription("Updates details of an existing user.");

        // Delete a user
        userGroup.MapDelete("/{id:int}", async ([FromServices] IUserService userService, int id) =>
        {
            var result = await userService.DeleteUserAsync(id);
            return result ? Results.NoContent() : Results.NotFound(new { Message = "User not found." });
        })
        .MapToApiVersion(1.0)
        .WithSummary("Deletes a user")
        .WithDescription("Deletes a user by their ID.");

        return userGroup;
    }
}