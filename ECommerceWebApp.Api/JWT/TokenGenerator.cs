using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ECommerceWebApp.Api.JWT
{
    public class TokenGenerator : ITokenGenerator
    {
        public string GenerateToken(string email)
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
            
            return tokenHandler.WriteToken(token);
        }
    }
}
