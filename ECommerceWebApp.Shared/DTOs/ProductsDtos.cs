using System.ComponentModel.DataAnnotations;

namespace ECommerceWebApp.Shared.DTOs;

public record GetProductsDtoV1(
    int PageNumber = 1,
    int PageSize = 5,
    string? Filter = null
    );

public record ProductDtoV1(
    int Id,
    string Title,
    string Description,
    decimal Price,
    string ImageUri
);

public record GetProductsDtoV2(
    int PageNumber = 1,
    int PageSize = 5,
    string? Filter = null
    );
public record ProductDtoV2(
    int Id,
    string Title,
    string Description,
    decimal Price,
    decimal RetailPrice,
    string ImageUri
);


public record CreateProductDto(
    [Required][StringLength(50)] string Title,
    [Required][StringLength(100)] string Description,
    decimal Price
    );

public record UpdateProductDto(
    [Required][StringLength(50)] string Title,
    [Required][StringLength(100)] string Description,
    decimal Price
    );
