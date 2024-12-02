using ECommerceWebApp.Business.Interfaces;
using ECommerceWebApp.Shared.DTOs.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ECommerceWebApp.Business.Services
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        public JwtTokenResultDto GenerateToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = "ThisIsASecretKeyForTokenGeneration"u8.ToArray();//Byte array

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Email, email)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = "ECommerceWebApp",
                Audience = "ECommerceWebApp",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                NotBefore = DateTime.UtcNow,
                IssuedAt = DateTime.UtcNow

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new JwtTokenResultDto
            {
                Token = tokenHandler.WriteToken(token),
                ExpirationDateDuration = 1,
                ExpirationDateOffset = DateTime.Now.AddMinutes(60),
                ExpirationDate = tokenDescriptor.Expires.Value
            };
        }
    }
}
