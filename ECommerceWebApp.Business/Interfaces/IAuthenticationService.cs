using ECommerceWebApp.Shared.DTOs;

namespace ECommerceWebApp.Api.Authenticate.Interfaces
{
    public interface IAuthenticationService
    {
        Task<UserLoginResultDto> AuthenticateAsync(UserLoginPostDto userLoginDto);
    }
}
