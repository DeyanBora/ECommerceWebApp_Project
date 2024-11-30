using ECommerceWebApp.Api.Authenticate.Implementations;
using ECommerceWebApp.Api.Authenticate.Interfaces;
using ECommerceWebApp.Api.Authenticate.Jwt;
using ECommerceWebApp.Api.Cors;
using ECommerceWebApp.Api.Endpoints;
using ECommerceWebApp.Api.ErrorHandling;
using ECommerceWebApp.Api.Extensions.Jwt;
using ECommerceWebApp.Api.OpenAPI;
using ECommerceWebApp.Business.Interfaces;
using ECommerceWebApp.Business.Services;
using ECommerceWebApp.DataAccess.Data;
using ECommerceWebApp.DataAccess.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));
var jwtSetting = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

builder.Services.AddRepositories(builder.Configuration);
// Add configuration for ElasticSearch URL
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);


// Register ISearchService with HttpClient
builder.Services.AddHttpClient<ISearchService, SearchService>(client =>
{
    client.BaseAddress = new Uri("https://your-elastic-api-url"); // Replace with your Elasticsearch API URL
});

builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

//Authentication Options
//builder.Services.AddAuthentication().AddJwtBearer();
// Authorization Options
//builder.Services.AddProductAuthorization();

builder.Services.AddDbContext<ECommerceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ECommerceContext")));

var connectionString = builder.Configuration.GetConnectionString("ECommerceContext");


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

//Swagger Options
builder.Services.AddSwaggerGen()
                .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
                .AddEndpointsApiExplorer();

var app = builder.Build();

//Configre Middlewares
app.UseExceptionHandler(exceptionHandlerApp => exceptionHandlerApp.ConfigureExceptionHandler());
//app.UseMiddleware<RequestTimingMiddleware>();

//Initialize Database EF
await app.Services.InitializeDbAsync();

//Configure Endpoints
app.MapProductsEndpoint();
app.MapAuthEndpoint();
app.MapSearchEndpoint();


app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// Configure Swagger
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