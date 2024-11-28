using ECommerceWebApp.Shared.DTOs;

namespace ECommerceWebApp.Api.Authenticate.Interfaces
{
    public interface IAuthenticationService
    {
        UserLoginResultDto Authenticate(UserLoginPostDto userLoginDto);
    }
}
