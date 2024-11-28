using ECommerceWebApp.Api.Authenticate.Interfaces;
using ECommerceWebApp.Shared.DTOs;
namespace ECommerceWebApp.Api.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoint(this IEndpointRouteBuilder routes)
    {
        var authgroup = routes.NewVersionedApi().MapGroup("/auth")
            .HasApiVersion(1.0)
            .WithParameterValidation()
            .WithOpenApi()
            .WithTags("Authentication");

        authgroup.MapPost("/login", async (IAuthenticationService authService, UserLoginPostDto userLoginDto, ILoggerFactory loggerFactory) =>
        {
            // Create a logger for a specific category
            var logger = loggerFactory.CreateLogger("UserEndpoints");

            // Validate input
            if (string.IsNullOrWhiteSpace(userLoginDto.Email) || string.IsNullOrWhiteSpace(userLoginDto.Password))
            {
                logger.LogWarning("Invalid login attempt with empty fields.");
                return Results.BadRequest(new { Message = "Email and Password are required." });
            }

            try
            {
                // Authenticate user
                var userLoginResult = authService.Authenticate(userLoginDto);

                if (userLoginResult == null)
                {
                    logger.LogWarning("Authentication failed for email: {Email}", userLoginDto.Email);
                    return Results.Json(new { Message = "Invalid credentials." }, statusCode: StatusCodes.Status401Unauthorized);
                }

                // Return successful response
                return Results.Ok(userLoginResult);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during login process.");
                return Results.Problem("An error occurred during login. Please try again later.");
            }
        })
        .MapToApiVersion(1.0)
        .WithSummary("Logs in a user and generates a JWT")
        .WithDescription("Authenticates a user with their credentials and returns a JWT along with user details.");

        return authgroup;
    }


}