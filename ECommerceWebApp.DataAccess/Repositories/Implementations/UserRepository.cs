using ECommerceWebApp.DataAccess.Data;
using ECommerceWebApp.DataAccess.Repositories.Interfaces;
using ECommerceWebApp.Entities.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerceWebApp.DataAccess.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly ECommerceContext dbContext;
    private readonly ILogger<UserRepository> logger;

    public UserRepository(ECommerceContext dbContext, ILogger<UserRepository> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task<(int TotalCount, IEnumerable<User>)> GetUsersWithPaginationAsync(int pageNumber, int pageSize, string? filter)
    {
        if (pageNumber <= 0 || pageSize <= 0)
        {
            throw new ArgumentException("Page number and page size must be greater than zero.");
        }

        var filteredUsers = FilterUsers(filter);

        var totalCount = await filteredUsers.CountAsync();
        var users = await filteredUsers
            .OrderBy(user => user.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        return (totalCount, users);
    }

    public async Task<User?> GetAsync(int id)
    {
        return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task CreateAsync(User user)
    {
        user.CreatedDate = DateTime.UtcNow;
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Created User with ID {Id} and Name {FirstName}", user.Id, user.FirstName);
    }

    public async Task UpdateAsync(User user)
    {
        user.UpdatedDate = DateTime.UtcNow;
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Updated User with ID {Id}", user.Id);
    }

    public async Task DeleteAsync(int id, bool softDelete = true)
    {
        var user = await dbContext.Users.FindAsync(id);
        if (user == null)
        {
            logger.LogWarning("No user found to delete with ID {Id}", id);
            return;
        }

        if (softDelete)
        {
            user.IsDeleted = true; // Assuming IsDeleted is a property for soft deletion
        }
        else
        {
            dbContext.Users.Remove(user);
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await dbContext.Users.AnyAsync(user => user.Id == id);
    }

    public async Task<int> CountAsync(string? filter)
    {
        return await FilterUsers(filter).CountAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task AssignRolesAsync(int userId, IEnumerable<string> roleNames)
    {
        var user = await dbContext.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {userId} not found.");
        }

        // Clear existing roles
        user.Roles.Clear();

        // Add new roles
        foreach (var roleName in roleNames)
        {
            var role = await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
            {
                throw new ArgumentException($"Role with name {roleName} not found.");
            }

            user.Roles.Add(role);
        }

        await dbContext.SaveChangesAsync();
        logger.LogInformation("Assigned roles to User ID {UserId}", userId);
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(int userId)
    {
        var user = await dbContext.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {userId} not found.");
        }

        return user.Roles.Select(r => r.Name).ToList();
    }

    private IQueryable<User> FilterUsers(string? filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
        {
            return dbContext.Users.Where(user => !user.IsDeleted); // Exclude soft-deleted users
        }

        return dbContext.Users.Where(user => !user.IsDeleted &&
            (user.FirstName.Contains(filter) || user.LastName.Contains(filter) || user.Email.Contains(filter)));
    }

    public async Task<bool> UserNameExistsAsync(string userName)
    {
        return await dbContext.Users
            .AnyAsync(u => u.UserName.ToLower() == userName.ToLower());
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await dbContext.Users
            .AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }
}
