namespace ECommerceWebApp.Shared.DTOs
{
    public record UserLoginPostDto(
        string Email,
        string Password
        );

    public record UserLoginResultDto(
        string Email,
        string FirstName,
        string LastName,
        string Token,
        DateTime TokenExpiration
        );

}
