using ECommerceWebApp.Api.Authenticate.Interfaces;
using ECommerceWebApp.Shared.DTOs.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerceWebApp.Api.Authenticate.Jwt
{
    public class JwtService : IJwtService
    {
        public JwtTokenResultDto GenerateToken(JwtSettings jwtSetting, UserTokenInfo userTokenInfo, IList<string> roles/*, List<Claim> claims*/)
        {
            var secretKey = Encoding.UTF8.GetBytes(jwtSetting.SecretKey); // longer that 16 character
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);


            var encryptionkey = Encoding.UTF8.GetBytes(jwtSetting.Encryptkey); //must be 16 character
            /*var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);*/

            if (secretKey.Length < 16 || encryptionkey.Length != 16)
            {
                throw new ArgumentException("SecretKey must be at least 16 characters, and EncryptKey must be exactly 16 characters.");
            }

            var _claims = _getClaims(userTokenInfo, roles, new List<Claim>());
            var identity = new ClaimsIdentity(_claims, "custom", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            var principal = new ClaimsPrincipal(identity);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtSetting.Issuer,
                Audience = jwtSetting.Audience,
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(jwtSetting.NotBeforeMinutes),
                Expires = DateTime.Now.AddMinutes(jwtSetting.ExpirationMinutes),
                SigningCredentials = signingCredentials,
                //EncryptingCredentials = encryptingCredentials,
                Subject = new ClaimsIdentity(_claims),
                //Claims = _claims.ToDictionary(c => c.Type, c => (object)c.Value)
            };

            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            //JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(descriptor);

            var jwt = tokenHandler.WriteToken(securityToken);
            var result = new JwtTokenResultDto
            {
                Token = jwt,
                ExpirationDate = descriptor.Expires.Value,
                ExpirationDateDuration = (long)(descriptor.Expires.Value - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds,
                ExpirationDateOffset = descriptor.Expires.Value

            };

            return result;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(JwtSettings jwtSetting, string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false, // Here we are not validating the lifetime
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSetting.Issuer,
                ValidAudience = jwtSetting.Audience,

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecretKey)),
                TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Encryptkey))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            //var jwtSecurityToken = securityToken as JwtSecurityToken;

            // if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            //     throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public string GetUserIdFromExpiredToken(JwtSettings jwtSetting, string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false, // Here we are not validating the lifetime
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSetting.Issuer,
                ValidAudience = jwtSetting.Audience,

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecretKey)),
                //TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Encryptkey))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            ClaimsPrincipal principal = null;
            try
            {
                principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                // var jwtSecurityToken = securityToken as JwtSecurityToken;

                // if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                //     throw new SecurityTokenException("Invalid token");
                var claim = principal.FindFirst(ClaimTypes.NameIdentifier) ?? principal.FindFirst("sub");

                if (claim != null)
                {
                    return claim.Value;
                }
                else
                {
                    return "1"; //  "Invalid token or user";
                }
            }
            catch (Exception e)
            {

                return "2";//  "Invalid token";
            }


        }

        private List<Claim> _getClaims(UserTokenInfo userTokenInfo, IList<string> roles, List<Claim> claims)
        {
            var securityStampClaimType = new ClaimsIdentityOptions().SecurityStampClaimType;

            var list = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userTokenInfo.UserName),
                new Claim(ClaimTypes.NameIdentifier, userTokenInfo.Id.ToString()),
                new Claim(securityStampClaimType, userTokenInfo.SecurityStamp.ToString()),

            };

            //var roles = new Role[] { new Role { Name = "Admin" } };
            foreach (var role in roles)
                list.Add(new Claim(ClaimTypes.Role, role));


            list.AddRange(claims);

            return list;
        }
    }
}
