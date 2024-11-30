using ECommerceWebApp.Business.Interfaces;
using ECommerceWebApp.DataAccess.Repositories.Interfaces;
using ECommerceWebApp.Entities.Entities.Users;
using ECommerceWebApp.Shared.DTOs;
using System.Text;
namespace ECommerceWebApp.Business.Services;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<(int TotalCount, IEnumerable<UserDto>)> GetUsersWithPaginationAsync(int pageNumber, int pageSize, string? filter)
    {
        var (totalCount, users) = await _userRepository.GetUsersWithPaginationAsync(pageNumber, pageSize, filter);

        var userDtos = users.Select(user => new UserDto(user.Id, user.FirstName, user.LastName, user.Email));
        return (totalCount, userDtos);
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetAsync(id);
        if (user == null) return null;

        return new UserDto(user.Id, user.FirstName, user.LastName, user.Email);
    }

    public async Task<UserDto> CreateUserAsync(UserCreateDto userCreateDto, IEnumerable<string> roles)
    {
        // Validate the roles
        var roleList = roles.ToList();
        if (!roleList.Any())
        {
            throw new ArgumentException("At least one role must be assigned to the user.");
        }

        // Hash the password (placeholder, replace with secure hashing logic)
        var hashedPassword = HashPassword(userCreateDto.Password);

        var user = new User
        {
            FirstName = userCreateDto.FirstName,
            LastName = userCreateDto.LastName,
            Email = userCreateDto.Email,
            Password = hashedPassword,
        };

        // Create the user in the database
        await _userRepository.CreateAsync(user);

        // Assign roles to the user
        await _userRepository.AssignRolesAsync(user.Id, roleList);

        // Map back to DTO
        return new UserDto(user.Id, user.FirstName, user.LastName, user.Email);
    }

    public async Task<UserDto?> UpdateUserAsync(int id, UserUpdateDto userUpdateDto)
    {
        var user = await _userRepository.GetAsync(id);
        if (user == null) return null;

        // Update user properties
        user.FirstName = userUpdateDto.FirstName;
        user.LastName = userUpdateDto.LastName;
        user.Email = userUpdateDto.Email;

        await _userRepository.UpdateAsync(user);

        return new UserDto(user.Id, user.FirstName, user.LastName, user.Email);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var userExists = await _userRepository.ExistsAsync(id);
        if (!userExists) return false;

        await _userRepository.DeleteAsync(id);
        return true;
    }

    private string HashPassword(string password)
    {
        // Placeholder for password hashing logic (e.g., using BCrypt or similar libraries)
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
    }
}