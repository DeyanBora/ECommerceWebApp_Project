
using ECommerceWebApp.Api.Authenticate.Interfaces;
using ECommerceWebApp.Api.Authenticate.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ECommerceWebApp.Api.Extensions.Jwt
{
    public static class JwtServiceExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services, JwtSettings jwtSettings)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var secretkey = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
                var encryptionkey = Encoding.UTF8.GetBytes(jwtSettings.Encryptkey);

                var validationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero, // default: 5 min
                    RequireSignedTokens = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretkey),

                    RequireExpirationTime = true,
                    ValidateLifetime = true,

                    ValidateAudience = true, //default : false
                    ValidAudience = jwtSettings.Audience,

                    ValidateIssuer = true, //default : false
                    ValidIssuer = jwtSettings.Issuer,

                    TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey)
                };

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = validationParameters;
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        //var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                        //logger.LogError("Authentication failed.", context.Exception);

                        //if (context.Request.Path.Equals(jwtSettings.GetRefreshTokenURL))
                        //{
                        //    return Task.CompletedTask;
                        //}

                        if (context.Exception != null)
                        {
                            //if (context.Exception.Message.Contains("IDX10603"))
                            //    throw new SecurityTokenExpiredException("Token has expired. Please login again.");
                            //if (context.Exception.Message.Contains("IDX10503"))
                            //    throw new SecurityTokenInvalidAudienceException("Token audience is invalid.");
                            //if (context.Exception.Message.Contains("IDX10211"))
                            //    throw new SecurityTokenInvalidIssuerException("Token issuer is invalid.");
                            //if (context.Exception.Message.Contains("IDX10223"))
                            //    throw new SecurityTokenInvalidLifetimeException("Token lifetime is invalid.");
                            //if (context.Exception.Message.Contains("IDX10500"))
                            //    throw new SecurityTokenInvalidSignatureException("Token signature is invalid.");
                            //if (context.Exception.Message.Contains("IDX10501"))
                            //    throw new SecurityTokenInvalidSigningKeyException("Token signing key is invalid.");
                            //else throw context.Exception;
                            //var response = context.Response;
                            //response.StatusCode = StatusCodes.Status401Unauthorized;
                            //response.ContentType = "application/json";
                            //string result = System.Text.Json.JsonSerializer.Serialize(new { message = "Unauthorized" });
                            //return response.WriteAsync(result);
                            throw new UnauthorizedAccessException("Unauthorized", context.Exception);
                        }

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = async context =>
                    {

                        /*
                        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;

                        //var applicationSignInManager = context.HttpContext.RequestServices.GetRequiredService<IApplicationSignInManager>();
                        var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();


                        // var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                        if (claimsIdentity.Claims?.Any() != true)
                            throw new UnauthorizedAccessException("This token has no claims.");
                        //context.Fail("This token has no claims.");


                        var securityStamp = claimsIdentity.FindFirst(new ClaimsIdentityOptions().SecurityStampClaimType);
                        if (securityStamp == null || securityStamp.Value == null)
                            throw new UnauthorizedAccessException("This token has no secuirty stamp");
                        //context.Fail("This token has no secuirty stamp");

                        //Find user and token from database and perform your custom validation
                        //var userId = claimsIdentity.GetUserId<int>();
                        int uid = 0;
                        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                        if (!int.TryParse(userId, out uid))
                            throw new UnauthorizedAccessException("This token has no user id");
                        //context.Fail("This token has no user id");
                        var user = await userRepository.GetAsync(uid);
                        //var user = await userRepository.GetByIdAsync(context.HttpContext.RequestAborted, userId);

                        if (user.SecurityStamp != Guid.Parse(securityStamp.Value))
                            throw new UnauthorizedAccessException("Token secuirty stamp is not valid.");
                        //context.Fail("Token secuirty stamp is not valid.");

                        //var validatedUser = await applicationSignInManager.ValidateSecurityStampAsync(context.Principal);
                        //if (validatedUser == null)
                        //    context.Fail("Token secuirty stamp is not valid.");

                        //if (!user.IsActive)
                        //    context.Fail("User is not active.");

                        //await userRepository.UpdateLastLoginDateAsync(user, context.HttpContext.RequestAborted);
                        */


                    },
                    OnChallenge = context =>
                    {
                        //var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                        //logger.LogError("OnChallenge error", context.Error, context.ErrorDescription);

                        if (context.AuthenticateFailure != null)
                            throw new Exception("context.AuthenticateFailure.Message");
                        return Task.CompletedTask;
                    }
                };
            });
        }

        public static IServiceCollection AddJwtAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
            return services;
        }
        public static IServiceCollection AddJwtServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();
            return services;
        }
    }
}
