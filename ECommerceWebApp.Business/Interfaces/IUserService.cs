using ECommerceWebApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWebApp.Business.Interfaces
{
    public interface IUserService
    {
        Task<(int TotalCount, IEnumerable<UserDto>)> GetUsersWithPaginationAsync(int pageNumber, int pageSize, string? filter);
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto> CreateUserAsync(UserCreateDto userCreateDto, IEnumerable<string> roles);
        Task<UserDto?> UpdateUserAsync(int id, UserUpdateDto userUpdateDto);
        Task<bool> DeleteUserAsync(int id);
    }
}
