using ECommerceWebApp.Api.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ECommerceWebApp.Api.Extensions.Authorization;

public static class AuthorizationPolicyExtensions
{
    /*public static IServiceCollection AddProductAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.ReadAccess, builder =>
                builder.RequireClaim("scope", "product:read")
                       .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));

            options.AddPolicy(Policies.WriteAccess, builder =>
                builder.RequireClaim("scope", "product:write")
                       .RequireRole("Admin")
                       .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
        });

        return services;
    }*/
}