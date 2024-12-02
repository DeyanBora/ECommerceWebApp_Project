using System.ComponentModel.DataAnnotations;

namespace ECommerceWebApp.Shared.DTOs;


public record GetProductsDto(
    int PageNumber = 1,
    int PageSize = 5,
    string? Filter = null
);

// DTO for product details
public record ProductDto(
    int Id,
    string ErpCode,
    string Title,
    string Description,
    decimal Price,
    string ImageUrl,
    int Stock,
    int CategoryId,
    int BrandId,
    int ManufacturerId,
    string? Slug
);

// DTO for creating a product
public record CreateProductDto(
    [Required][StringLength(50)] string ErpCode, 
    [Required][StringLength(50)] string Title,
    [Required][StringLength(100)] string Description,
    [Required][Range(0, double.MaxValue)] decimal Price,
    [Required][Range(0, int.MaxValue)] int Stock,
    [Required] int CategoryId,
    [Required] int BrandId,
    [Required] int ManufacturerId,
    [Required][Url][StringLength(100)] string ImageUrl,
    [StringLength(100)] string? Slug
);


public record UpdateProductDto(
    [Required][StringLength(50)] string ErpCode,
    [Required][StringLength(50)] string Title,
    [Required][StringLength(100)] string Description,
    [Required][Range(0, double.MaxValue)] decimal Price,
    [Required][Range(0, int.MaxValue)] int Stock,
    [Required] int CategoryId,
    [Required] int BrandId,
    [Required] int ManufacturerId,
    [Required][Url][StringLength(100)] string ImageUrl,
    [StringLength(100)] string? Slug
);

public record ProductDtoV1(
    int Id,
    string ErpCode,
    string Title,
    string Description,
    decimal Price,
    string ImageUrl,
    int Stock,
    int CategoryId,
    int BrandId,
    int ManufacturerId,
    string? Slug
);

// DTO for product details
public record ProductDtoV2(
    int Id,
    string ErpCode,
    string Title,
    string Description,
    decimal Price,
    string ImageUrl,
    int Stock,
    int CategoryId,
    int BrandId,
    int ManufacturerId,
    string? Slug
);


public record GetProductsDtoV1(
    int PageNumber = 1,
    int PageSize = 5,
    string? Filter = null
);


public record ProductDtov1(
    int Id,
    string ErpCode,
    string Title,
    string Description,
    decimal Price,
    string ImageUrl,
    int Stock,
    int CategoryId,
    int BrandId,
    int ManufacturerId,
    string? Slug
); 
public record GetProductsDtoV2(
    int PageNumber = 1,
    int PageSize = 5,
    string? Filter = null
);


public record ProductDtov2(
    int Id,
    string ErpCode, 
    string Title,
    string Description,
    decimal Price,
    string ImageUrl,
    int Stock,
    int CategoryId,
    int BrandId,
    int ManufacturerId,
    string? Slug
);