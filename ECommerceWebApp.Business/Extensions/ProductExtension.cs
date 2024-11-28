using ECommerceWebApp.Entities.Entities.Products;
using ECommerceWebApp.Shared.DTOs;

namespace ECommerceWebApp.Business.Extensions;

public static class ProductExtension
{
    public static ProductDtoV1 ToDtoV1(this Product product)
    {
        return new ProductDtoV1(
            product.Id,
            product.Title,
            product.Description,
            product.Price,
            product.ImageUri
        );
    }

    public static ProductDtoV2 ToDtoV2(this Product product)
    {
        return new ProductDtoV2(
            product.Id,
            product.Title,
            product.Description,
            product.Price - product.Price * .3m, // 30% discount
            product.Price,
            product.ImageUri
        );
    }
}