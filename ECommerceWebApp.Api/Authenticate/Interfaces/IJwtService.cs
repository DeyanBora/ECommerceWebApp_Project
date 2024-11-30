using ECommerceWebApp.Api.Authenticate.Jwt;
using ECommerceWebApp.Shared.DTOs.Jwt;
using System.Security.Claims;

namespace ECommerceWebApp.Api.Authenticate.Interfaces
{
    public interface IJwtService
    {
        JwtTokenResultDto GenerateToken(JwtSettings jwtSetting, UserTokenInfo userTokenInfo, IList<string> roles /*,List<Claim> claims*/);
        ClaimsPrincipal GetPrincipalFromExpiredToken(JwtSettings jwtSetting, string token);
        string GetUserIdFromExpiredToken(JwtSettings jwtSetting, string token);
    }
}
