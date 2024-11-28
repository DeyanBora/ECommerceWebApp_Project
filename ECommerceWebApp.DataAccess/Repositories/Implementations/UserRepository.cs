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

    public async Task<IEnumerable<User>> GetAllAsync(int pageNumber, int pageSize, string? filter)
    {
        var skipCount = (pageNumber - 1) * pageSize;

        return await FilterUsers(filter)
                    .OrderBy(user => user.Id)
                    .Skip(skipCount)
                    .Take(pageSize)
                    .AsNoTracking().ToListAsync();
    }

    public async Task<User?> GetAsync(int id)
    {
        return await dbContext.Users.FindAsync(id);
    }

    public async Task CreateAsync(User user)
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        logger.LogInformation($"Created User {user.FirstName}.");
    }

    public async Task UpdateAsync(User addedUser)
    {
        dbContext.Update(addedUser);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        //Delete without loading into memory
        await dbContext.Users.Where(p => p.Id == id)
                                .ExecuteDeleteAsync();
    }

    public async Task<int> CountAsync(string? filter)
    {
        return await FilterUsers(filter).CountAsync();
    }

    private IQueryable<User> FilterUsers(string? filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
        {
            return dbContext.Users;
        }
        return dbContext.Users.Where(user => user.FirstName.Contains(filter) || user.LastName.Contains(filter));
    }
}
