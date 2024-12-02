using ECommerceWebApp.Api.Endpoints;

namespace ECommerceWebApp.Api.Extensions.Endpoint
{
    public static class EndpointExtensions
    {
        public static void MapApiEndpoints(this WebApplication app)
        {
            // Map all your endpoints here
            app.MapProductsEndpoint();        // Map product-related endpoints
            app.MapAuthEndpoint();           // Map authentication-related endpoints
            app.MapUserEndpoints();          // Map user-related endpoints
            app.MapElasticEndpoints();       // Map ElasticSearch-related endpoints
            app.MapBrandsEndpoint();         // Map brand-related endpoints
            app.MapCategoriesEndpoint();     // Map category-related endpoints
            app.MapManufacturersEndpoint();  // Map manufacturer-related endpoints
        }
    }
}