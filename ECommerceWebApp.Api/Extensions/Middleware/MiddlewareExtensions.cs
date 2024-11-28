using ECommerceWebApp.Api.ErrorHandling;
using ECommerceWebApp.Api.Middleware;

namespace ECommerceWebApp.Api.Extensions.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(exceptionHandlerApp => exceptionHandlerApp.ConfigureExceptionHandler());
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            return app;
        }
    }
}
