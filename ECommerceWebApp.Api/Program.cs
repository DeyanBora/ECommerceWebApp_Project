using ECommerceWebApp.Api.Authenticate.Implementations;
using ECommerceWebApp.Api.Authenticate.Interfaces;
using ECommerceWebApp.Api.Cors;
using ECommerceWebApp.Api.ErrorHandling;
using ECommerceWebApp.Api.Extensions.Endpoint;
using ECommerceWebApp.Api.JWT;
using ECommerceWebApp.Api.OpenAPI;
using ECommerceWebApp.Business.Interfaces;
using ECommerceWebApp.Business.Services;
using ECommerceWebApp.Business.Services.Interfaces;
using ECommerceWebApp.DataAccess.Data;
using ECommerceWebApp.DataAccess.Data.Extensions;
using ECommerceWebApp.Entities.Entities.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Load configuration from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Register Repositories
builder.Services.AddRepositories(builder.Configuration);

//Register HttpClient with ElasticUrl to connect to ElasticSearch
var elasticUrl = builder.Configuration.GetSection("ElasticSettings:Url").Value;
var handler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
};
builder.Services.AddHttpClient<IElasticApiService, ElasticApiService>(client =>
{
    client.BaseAddress = new Uri("elasticUrl"); 
})
.ConfigurePrimaryHttpMessageHandler(() => handler);

// Register Password Hasher
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// Register Token Generator
builder.Services.AddSingleton<ITokenGenerator, TokenGenerator>();

// Register all services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();

//Register Elastic
builder.Services.AddScoped<IElasticApiService, ElasticApiService>();

// Register DbContext with SQL Server
builder.Services.AddDbContext<ECommerceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ECommerceContext")).LogTo(Console.WriteLine, LogLevel.Information));
    
// Authentication Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.IncludeErrorDetails = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsASecretKeyForTokenGeneration")),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = "ECommerceWebApp",
            ValidAudience = "ECommerceWebApp",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(5) // Reduced ClockSkew
        };

        // Optional: Add events for debugging
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token successfully validated.");
                return Task.CompletedTask;
            }
        };
    });

// Authorization Configuration
builder.Services.AddAuthorization();

// API Versioning Configuration
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new(1.0);
    options.AssumeDefaultVersionWhenUnspecified = true;
})
.AddApiExplorer(options => options.GroupNameFormat = "'v'VVV");

// CORS Configuration
builder.Services.AddToCorsProject(builder.Configuration); // Ensure this defines a named policy, e.g., "ProductPolicy"

builder.Services.AddCors(options =>
{
    options.AddPolicy("ElasticCors",
        policy =>
        {
            policy.WithOrigins("http://localhost:5118")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Swagger Configuration
builder.Services.AddSwaggerGen()
    .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
    .AddEndpointsApiExplorer();

var app = builder.Build();

//Exception Handling Middleware
//app.UseExceptionHandler(exceptionHandlerApp => exceptionHandlerApp.ConfigureExceptionHandler());
// app.UseMiddleware<RequestTimingMiddleware>();

//Initialize Database EF
await app.Services.InitializeDbAsync();

//CORS Middleware
app.UseRouting();
app.UseCors("ElasticCors"); 

//Authentication and Authorization Middleware
app.UseAuthentication();
app.UseAuthorization();

//Swagger Middleware (!!before endpoint mapping!!)
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

// Endpoint Mapping
app.MapApiEndpoints();

app.Run();