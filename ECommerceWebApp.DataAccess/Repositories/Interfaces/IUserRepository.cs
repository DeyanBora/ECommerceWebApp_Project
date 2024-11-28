using ECommerceWebApp.Entities.Entities.Users;

namespace ECommerceWebApp.DataAccess.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task CreateAsync(User user);
        Task DeleteAsync(int id);
        Task<User> GetAsync(int id);
        Task<IEnumerable<User>> GetAllAsync(int pageNumber, int pageSize, string? filter);
        Task UpdateAsync(User addedUser);
        Task<int> CountAsync(string? filter);
    }
}
