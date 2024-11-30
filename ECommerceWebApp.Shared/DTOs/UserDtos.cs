namespace ECommerceWebApp.Shared.DTOs;

public record UserLoginPostDto(string Email, string Password);

public record UserLoginResultDto(string Email, string FirstName, string LastName, string Token, DateTime TokenExpiration);

public record UserDto(int Id, string FirstName, string LastName, string Email);

public record UserWithRolesDto(int Id, string FirstName, string LastName, string Email, IEnumerable<string> Roles);

public record PaginatedUsersDto(int TotalCount, IEnumerable<UserDto> Users);

public record UserRoleDto(int UserId, IEnumerable<string> Roles);

public record UserUpdateDto(string FirstName, string LastName, string Email);

public record UserCreateDto(string FirstName, string LastName, string Email, string Password);
