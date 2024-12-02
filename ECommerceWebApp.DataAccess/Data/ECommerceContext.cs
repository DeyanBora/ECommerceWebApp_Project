using ECommerceWebApp.Entities.Entities.Products;
using ECommerceWebApp.Entities.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ECommerceWebApp.DataAccess.Data;

public class ECommerceContext : DbContext
{
    public ECommerceContext(DbContextOptions<ECommerceContext> options)
        : base(options)
    {

    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Manufacturer> Manufacturers => Set<Manufacturer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    }
}
