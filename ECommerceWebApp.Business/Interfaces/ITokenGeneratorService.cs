using ECommerceWebApp.Shared.DTOs.Jwt;

namespace ECommerceWebApp.Business.Interfaces
{
    public interface ITokenGeneratorService
    {
        public JwtTokenResultDto GenerateToken(string email);
    }
}
