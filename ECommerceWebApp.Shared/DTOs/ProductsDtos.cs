using System.ComponentModel.DataAnnotations;

namespace ECommerceWebApp.Shared.DTOs;

// DTO for fetching a list of products (Version 1)
public record GetProductsDtoV1(
    int PageNumber = 1,
    int PageSize = 5,
    string? Filter = null
);

// DTO for product details (Version 1)
public record ProductDtoV1(
    int Id,
    string Title,
    string Description,
    decimal Price,
    string ImageUri
);

// DTO for fetching a list of products (Version 2)
public record GetProductsDtoV2(
    int PageNumber = 1,
    int PageSize = 5,
    string? Filter = null
);

// Extended DTO for product details (Version 2)
public record ProductDtoV2(
    int Id,
    string Title,
    string Description,
    decimal Price,
    decimal RetailPrice,
    string ImageUri,
    int Stock,
    int CategoryId,
    int BrandId,
    int ManufacturerId,
    int SellerId,
    string? Slug
);

// DTO for creating a product
public record CreateProductDto(
    [Required][StringLength(50)] string Title,
    [Required][StringLength(100)] string Description,
    [Required][Range(0, double.MaxValue)] decimal Price,
    [Required][Range(0, int.MaxValue)] int Stock,
    [Required] int CategoryId,
    [Required] int BrandId,
    [Required] int ManufacturerId,
    [Required] int SellerId,
    [Required][Url][StringLength(100)] string ImageUri,
    [StringLength(100)] string? Slug
);

// DTO for updating a product
public record UpdateProductDto(
    [Required][StringLength(50)] string Title,
    [Required][StringLength(100)] string Description,
    [Required][Range(0, double.MaxValue)] decimal Price,
    [Required][Range(0, int.MaxValue)] int Stock,
    [Required] int CategoryId,
    [Required] int BrandId,
    [Required] int ManufacturerId,
    [Required] int SellerId,
    [Required][Url][StringLength(100)] string ImageUri,
    [StringLength(100)] string? Slug
);