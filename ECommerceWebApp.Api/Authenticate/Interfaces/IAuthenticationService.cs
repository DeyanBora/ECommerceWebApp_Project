using ECommerceWebApp.Shared.DTOs;

namespace ECommerceWebApp.Api.Authenticate.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IResult> AuthenticateAsync(UserLoginPostDto userLoginDto);
    }
}
