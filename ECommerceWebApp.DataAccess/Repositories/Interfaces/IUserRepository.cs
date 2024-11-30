using ECommerceWebApp.Entities.Entities.Users;

namespace ECommerceWebApp.DataAccess.Repositories.Interfaces;

public interface IUserRepository
{
    Task<(int TotalCount, IEnumerable<User>)> GetUsersWithPaginationAsync(int pageNumber, int pageSize, string? filter);
    Task<User?> GetAsync(int id);
    Task CreateAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id, bool softDelete = true);
    Task<bool> ExistsAsync(int id);
    Task<int> CountAsync(string? filter);
    Task<User?> GetByEmailAsync(string email);
    Task AssignRolesAsync(int userId, IEnumerable<string> roleNames);
    Task<IEnumerable<string>> GetUserRolesAsync(int userId);
    Task<bool> UserNameExistsAsync(string userName);
    Task<bool> EmailExistsAsync(string email);
}
