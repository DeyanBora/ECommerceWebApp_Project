using ECommerceWebApp.Entities.Entities.Products;
using ECommerceWebApp.Shared.DTOs;

namespace ECommerceWebApp.Business.Extensions;
public static class ProductExtension
{
    public static ProductDto ToDto(this Product product)
    {
        if (product == null) throw new ArgumentNullException(nameof(product));

        return new ProductDto(
            Id: product.Id,
            ErpCode: product.ErpCode,
            Title: product.Title,
            Description: product.Description,
            Price: product.Price,
            ImageUrl: product.ImageUrl,
            Stock: product.Stock,
            CategoryId: product.CategoryId,
            BrandId: product.BrandId,
            ManufacturerId: product.ManufacturerId,
            Slug: product.Slug
        );
    }

    public static ProductDto WithDiscount(this Product product, decimal discountPercentage)
    {
        if (product == null) throw new ArgumentNullException(nameof(product));
        if (discountPercentage < 0 || discountPercentage > 100)
            throw new ArgumentOutOfRangeException(nameof(discountPercentage), "Discount must be between 0 and 100.");

        decimal discountedPrice = product.Price - (product.Price * discountPercentage / 100);

        return new ProductDto(
            Id: product.Id,
            ErpCode: product.ErpCode,
            Title: product.Title,
            Description: product.Description,
            Price: discountedPrice,
            ImageUrl: product.ImageUrl,
            Stock: product.Stock,
            CategoryId: product.CategoryId,
            BrandId: product.BrandId,
            ManufacturerId: product.ManufacturerId,
            Slug: product.Slug
        );
    }


    public static ProductDtoV1 ToDtoV1(this Product product)
    {
        if (product == null) throw new ArgumentNullException(nameof(product));

        return new ProductDtoV1(
            Id: product.Id,
            ErpCode: product.ErpCode,
            Title: product.Title,
            Description: product.Description,
            Price: product.Price,
            ImageUrl: product.ImageUrl,
            Stock: product.Stock,
            CategoryId: product.CategoryId,
            BrandId: product.BrandId,
            ManufacturerId: product.ManufacturerId,
            Slug: product.Slug
        );
    }

    public static ProductDtoV2 ToDtoV2(this Product product)
    {
        if (product == null) throw new ArgumentNullException(nameof(product));

        return new ProductDtoV2(
            Id: product.Id,
            ErpCode: product.ErpCode,
            Title: product.Title,
            Description: product.Description,
            Price: product.Price,
            ImageUrl: product.ImageUrl,
            Stock: product.Stock,
            CategoryId: product.CategoryId,
            BrandId: product.BrandId,
            ManufacturerId: product.ManufacturerId,
            Slug: product.Slug
        );
    }
}