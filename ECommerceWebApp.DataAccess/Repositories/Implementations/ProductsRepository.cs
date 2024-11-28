using ECommerceWebApp.DataAccess.Repositories.Interfaces;
using ECommerceWebApp.Entities.Entities.Products;

namespace ECommerceWebApp.DataAccess.Repositories.Implementations;

public class ProductsRepository : IProductsRepository
{
    private readonly List<Product> products = new List<Product>();

    public async Task<IEnumerable<Product>> GetAllAsync(int pageNumber, int pageSize, string? filter)
    {
        var skipCount = (pageNumber - 1) * pageSize;

        return await Task.FromResult(FilterProducts(filter).Skip(skipCount).Take(pageSize));
    }

    public async Task<Product?> GetAsync(int id)
    {
        return await Task.FromResult(products.Find(x => x.Id == id));
    }

    public async Task CreateAsync(Product product)
    {
        product.Id = products.Max(x => x.Id) + 1;
        products.Add(product);

        await Task.CompletedTask;
    }
    public async Task UpdateAsync(Product addedProduct)
    {
        var index = products.FindIndex(product => product.Id == addedProduct.Id);
        products[index] = addedProduct;

        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var index = products.FindIndex(product => product.Id == id);
        products.RemoveAt(index);

        await Task.CompletedTask;
    }

    public async Task<int> CountAsync(string? filter)
    {
        return await Task.FromResult(FilterProducts(filter).Count());
    }

    private IEnumerable<Product> FilterProducts(string? filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
        {
            return products;
        }
        return products.Where(product => product.Title.Contains(filter) || product.Description.Contains(filter));
    }
}
