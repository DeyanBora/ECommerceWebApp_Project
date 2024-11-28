using ECommerceWebApp.Api.Endpoints;

namespace ECommerceWebApp.Api.Extensions.Endpoint
{
    public static class EndpointExtensions
    {
        public static IApplicationBuilder MapApiEndpoints(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapProductsEndpoint(); // Map product-related endpoints
                //endpoints.MapUserEndpoint();    // Map user-related endpoints
            });

            return app;
        }
    }
}