
using ECommerceWebApp.Api.Authenticate.Interfaces;
using System.Security.Claims;
using Microsoft.Extensions.Options;

using ECommerceWebApp.Api.Authenticate.Jwt;
using ECommerceWebApp.Shared.DTOs;
using ECommerceWebApp.Shared.DTOs.Jwt;
using ECommerceWebApp.Entities.Entities;
namespace ECommerceWebApp.Api.Authenticate.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtService _jwtService;
        private readonly JwtSettings _jwtSettings;

        public AuthenticationService(IJwtService jwtService, IOptions<JwtSettings> jwtSettings)
        {
            _jwtService = jwtService;
            _jwtSettings = jwtSettings.Value;
        }

        public UserLoginResultDto Authenticate(UserLoginPostDto userLoginDto)
        {
            // Validation logic
            if (string.IsNullOrWhiteSpace(userLoginDto.Email))
            {
                throw new ArgumentException("Email is required.");
            }
            if (string.IsNullOrWhiteSpace(userLoginDto.Password) || userLoginDto.Password.Length < 6)
            {
                throw new ArgumentException("Password must be at least 6 characters.");
            }

            // Authentication logic...
            var userTokenInfo = new UserTokenInfo
            {
                Id = 1,
                Email = userLoginDto.Email,
                UserName = userLoginDto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            //var userTokenInfo = new UserTokenInfo
            //{
            //    Id = user.Id,
            //    Email = user.Email,
            //    UserName = user.UserName,
            //    SecurityStamp = user.SecurityStamp, // Ideally stored in DB
            //};

            var roles = new List<string> { "Admin" };
            var claims = new List<Claim>
        {
            new Claim("Phone Number", "123456789")
        };

            var tokenInfo = _jwtService.GenerateToken(_jwtSettings, userTokenInfo, roles, claims);

            return new UserLoginResultDto(
                userLoginDto.Email,
                "test",
                "test",
                tokenInfo.Token,
                tokenInfo.ExpirationDate
            );
        }
    }
}
