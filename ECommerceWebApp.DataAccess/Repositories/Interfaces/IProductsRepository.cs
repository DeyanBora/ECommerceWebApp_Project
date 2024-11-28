﻿using ECommerceWebApp.Entities.Entities.Products;

namespace ECommerceWebApp.DataAccess.Repositories.Interfaces
{
    public interface IProductsRepository
    {
        Task CreateAsync(Product product);
        Task DeleteAsync(int id);
        Task<Product?> GetAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync(int pageNumber, int pagesize, string? filter);
        Task UpdateAsync(Product addedProduct);

        Task<int> CountAsync(string? filter);
    }
}