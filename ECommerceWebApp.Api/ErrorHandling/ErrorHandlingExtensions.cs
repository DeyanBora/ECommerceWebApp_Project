using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ECommerceWebApp.Api.ErrorHandling;

public static class ErrorHandlingExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var logger = context.RequestServices.GetRequiredService<ILoggerFactory>()
                                    .CreateLogger("GlobalExceptionHandler");

                var exceptionDetails = context.Features.Get<IExceptionHandlerFeature>();
                var exception = exceptionDetails?.Error;

                var statusCode = exception switch
                {
                    ArgumentException => StatusCodes.Status400BadRequest,
                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                    _ => StatusCodes.Status500InternalServerError
                };

                logger.LogError(exception,
                    "An error occurred while processing the request. Machine: {Machine}, TraceId: {TraceId}",
                    Environment.MachineName, Activity.Current?.TraceId);

                var problem = new ProblemDetails
                {
                    Title = GetProblemTitle(statusCode),
                    Status = statusCode,
                    Detail = GetProblemDetail(context, exception),
                    Extensions =
                    {
                        { "traceId", Activity.Current?.TraceId.ToString() },
                        { "machine", Environment.MachineName }
                    }
                };

                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = statusCode;

                await context.Response.WriteAsJsonAsync(problem);
            });
        });
    }

    private static string GetProblemTitle(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status400BadRequest => "Bad Request",
            StatusCodes.Status401Unauthorized => "Unauthorized Access",
            StatusCodes.Status500InternalServerError => "Server Error",
            _ => "An Error Occurred"
        };
    }

    private static string GetProblemDetail(HttpContext context, Exception? exception)
    {
        var environment = context.RequestServices.GetRequiredService<IHostEnvironment>();
        if (environment.IsDevelopment())
        {
            // Include exception details in development environment
            return exception?.ToString() ?? "An error occurred.";
        }

        return exception?.Message ?? "An error occurred while processing the request.";
    }
}