using ECommerceWebApp.Business.Interfaces;
using ECommerceWebApp.DataAccess.Repositories.Interfaces;
using ECommerceWebApp.Entities.Entities.Users;
using ECommerceWebApp.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
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

        var userDtos = users.Select(user => new UserDto(user.Id, user.FirstName, user.LastName, user.UserName, user.Email));
        return (totalCount, userDtos);
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetAsync(id);
        if (user == null) return null;

        return new UserDto(user.Id, user.FirstName, user.LastName,user.UserName, user.Email);
    }

    public async Task<UserDto> CreateUserAsync(UserRegistrationRequestDto userCreateDto)
    {
        //Db Control
        if (await _userRepository.UserNameExistsAsync(userCreateDto.UserName))
        {
            throw new ArgumentException("A user with this user name already exists.");
        }

        if (await _userRepository.EmailExistsAsync(userCreateDto.Email))
        {
            throw new ArgumentException("A user with this email already exists.");
        }

        // Create an instance of PasswordHasher
        var passwordHasher = new PasswordHasher<User>();

        var user = new User
        {
            FirstName = userCreateDto.FirstName,
            LastName = userCreateDto.LastName,
            UserName = userCreateDto.UserName,
            Email = userCreateDto.Email,
            Password = "",
            SecurityStamp = Guid.NewGuid(),
        };
        user.Password = passwordHasher.HashPassword(user, userCreateDto.Password);

        // Create the user in the database
        await _userRepository.CreateAsync(user);

        // Map back to DTO
        return new UserDto(user.Id, user.FirstName, user.LastName, user.UserName, user.Email);
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

        return new UserDto(user.Id, user.FirstName, user.LastName, user.UserName, user.Email);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var userExists = await _userRepository.ExistsAsync(id);
        if (!userExists) return false;

        await _userRepository.DeleteAsync(id);
        return true;
    }
}