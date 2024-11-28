//using ECommerceWebApp.Api.Authenticate.Implementations;
//using ECommerceWebApp.Api.Authenticate.Interfaces;
//using ECommerceWebApp.Api.Authenticate.Jwt;
//using ECommerceWebApp.Api.Cors;
//using ECommerceWebApp.Api.Endpoints;
//using ECommerceWebApp.Api.Extensions.Authorization;
//using ECommerceWebApp.Api.Extensions.Endpoint;
//using ECommerceWebApp.Api.Extensions.Jwt;
//using ECommerceWebApp.Api.Extensions.Middleware;
//using ECommerceWebApp.Api.Middleware;
//using ECommerceWebApp.Api.OpenAPI;
//using ECommerceWebApp.DataAccess.Data.Extensions;

//var builder = WebApplication.CreateBuilder(args);
//var jwtSetting = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
////builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
//// Configure Services
//builder.Services
//    .AddJwtAppSettings(builder.Configuration) // Register app settings like JwtSettings
//    .AddRepositories(builder.Configuration) // Register repositories
//    .AddJwtServices() // Add scoped services like JwtService
//    .AddProductAuthorization() // Authorization
//    .AddApiVersioningAndExplorer() // API versioning
//    .AddSwaggerDocumentation() // Swagger
//    .AddProductCors(builder.Configuration); // CORS
//builder.Services.AddJwtAuthentication(jwtSetting); // Authentication

//var app = builder.Build();

//// Configure Middleware
////app.UseGlobalExceptionHandling(); // Global exception handler
////app.UseMiddleware<RequestTimingMiddleware>(); // Timing middleware
////await app.Services.InitializeDbAsync(); // Initialize database

//// Configure HTTP Pipeline
////
////app.UseCors(); // CORS
////app.UseAuthentication(); // Authentication
////app.UseAuthorization(); // Authorization
//app.MapProductsEndpoint();
////app.MapApiEndpoints();
//app.UseRouting();
//app.ConfigureSwagger(); // Swagger

//app.UseRouting();

//app.Run();


using ECommerceWebApp.Api.Authenticate;
using ECommerceWebApp.Api.Authenticate.Implementations;
using ECommerceWebApp.Api.Authenticate.Interfaces;
using ECommerceWebApp.Api.Authenticate.Jwt;
using ECommerceWebApp.Api.Authorization;
using ECommerceWebApp.Api.Cors;
using ECommerceWebApp.Api.Endpoints;
using ECommerceWebApp.Api.ErrorHandling;
using ECommerceWebApp.Api.Extensions.Jwt;
using ECommerceWebApp.Api.OpenAPI;
using ECommerceWebApp.DataAccess.Data.Extensions;
using ECommerceWebApp.DataAccess.Repositories.Implementations;
using ECommerceWebApp.DataAccess.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));
var jwtSetting = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
//Authentication Options
//builder.Services.AddAuthentication().AddJwtBearer();
// Authorization Options
//builder.Services.AddProductAuthorization();

builder.Services.AddJwtAuthentication(jwtSetting);
builder.Services.AddAuthorization();

//API Versioning Options
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new(1.0);
    options.AssumeDefaultVersionWhenUnspecified = true;
})
    .AddApiExplorer(options => options.GroupNameFormat = "'v'VVV");

//Cors Options
builder.Services.AddProductCors(builder.Configuration);

builder.Services.AddSwaggerGen()
                .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
                .AddEndpointsApiExplorer();

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp => exceptionHandlerApp.ConfigureExceptionHandler());
//app.UseMiddleware<RequestTimingMiddleware>();

//await app.Services.InitializeDbAsync();

app.MapProductsEndpoint();
app.MapAuthEndpoint();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in app.DescribeApiVersions())
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });
}

app.Run();