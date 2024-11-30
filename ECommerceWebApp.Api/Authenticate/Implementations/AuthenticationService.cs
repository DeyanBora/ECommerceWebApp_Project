
using ECommerceWebApp.Api.Authenticate.Interfaces;
using ECommerceWebApp.Api.Authenticate.Jwt;
using ECommerceWebApp.DataAccess.Data;
using ECommerceWebApp.Entities.Entities.Users;
using ECommerceWebApp.Shared.DTOs;
using ECommerceWebApp.Shared.DTOs.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
namespace ECommerceWebApp.Api.Authenticate.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtService _jwtService;
        private readonly JwtSettings _jwtSettings;
        private readonly ECommerceContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;


        public AuthenticationService(IJwtService jwtService, IOptions<JwtSettings> jwtSettings, ECommerceContext context, IPasswordHasher<User> passwordHasher)
        {
            _jwtService = jwtService;
            _jwtSettings = jwtSettings.Value;
            _context = context;

            _passwordHasher = passwordHasher;

        }

        public async Task<IResult> AuthenticateAsync(UserLoginPostDto userLoginDto)
        {
            // Input validation
            if (string.IsNullOrWhiteSpace(userLoginDto.Email))
            {
                return Results.BadRequest(new { Error = "The email address is required." });
            }

            if (string.IsNullOrWhiteSpace(userLoginDto.Password) || userLoginDto.Password.Length < 6)
            {
                return Results.BadRequest(new { Error = "The password must be at least 6 characters long." });
            }

            // Find the user
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == userLoginDto.Email.ToLower());

            if (user == null)
            {
                return Results.Unauthorized();
            }

            // Verify password
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, userLoginDto.Password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return Results.Unauthorized();
            }

            // Ensure security stamp
            if (string.IsNullOrEmpty(user.SecurityStamp.ToString()))
            {
                user.SecurityStamp = Guid.NewGuid();
                _context.Update(user);
                await _context.SaveChangesAsync();
            }

            // Create token
            var userTokenInfo = new UserTokenInfo
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                SecurityStamp = user.SecurityStamp.ToString(),
            };

            var roles = new List<string>();

            var jwtTokenResult = _jwtService.GenerateToken(_jwtSettings, userTokenInfo, roles);

            // Combine user details and token info into the response DTO
            var result = new UserLoginResultDto(
                Email: user.Email,
                Username: user.UserName,
                FirstName: user.FirstName,
                LastName: user.LastName,
                Token: jwtTokenResult.Token,
                TokenExpiration: jwtTokenResult.ExpirationDate,
                TokenExpirationDuration: jwtTokenResult.ExpirationDateDuration,
                TokenExpirationOffset: jwtTokenResult.ExpirationDateOffset
            );

            return Results.Ok(result);
        }
    }
}
