using ECommerceWebApp.DataAccess.Repositories.Implementations;
using ECommerceWebApp.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceWebApp.DataAccess.Data.Extensions;

public static class DataExtensions
{
    public static async Task InitializeDbAsync(this IServiceProvider serviceProvider)
    {
        //Automatically apply migration everytime app started
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ECommerceContext>();
        await dbContext.Database.MigrateAsync();
    }
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        var ConnectString = configuration.GetConnectionString("ECommerceContext");
        services.AddSqlServer<ECommerceContext>(ConnectString)
                .AddScoped<IProductsRepository, ProductRepository>()
                .AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}
