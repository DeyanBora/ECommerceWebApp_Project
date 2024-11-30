using System.ComponentModel.DataAnnotations;

namespace ECommerceWebApp.Shared.DTOs;

public class UserLoginPostDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
public record UserLoginResultDto(
    string Email,
    string Username,
    string FirstName,
    string LastName,
    string Token,
    DateTime TokenExpiration,
    long TokenExpirationDuration,
    DateTimeOffset TokenExpirationOffset);
public record UserDto(int Id, string FirstName, string LastName, string UserName, string Email);

public record UserWithRolesDto(int Id, string FirstName, string LastName, string Email, IEnumerable<string> Roles);

public record PaginatedUsersDto(int TotalCount, IEnumerable<UserDto> Users);

public record UserRoleDto(int UserId, IEnumerable<string> Roles);

public record UserUpdateDto(string FirstName, string LastName, string Email);

public record UserCreateDto(string FirstName, string LastName, string Email, string Password);

public record UserRegistrationRequestDto
{
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
    public required string FirstName { get; init; }

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
    public required string LastName { get; init; }

    [Required(ErrorMessage = "User name is required.")]
    [StringLength(50, ErrorMessage = "User name cannot exceed 50 characters.")]
    public required string UserName { get; init; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public required string Email { get; init; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    public required string Password { get; init; }
}