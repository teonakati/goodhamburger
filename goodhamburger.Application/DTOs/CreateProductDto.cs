using goodhamburger.Domain.Enums;

namespace goodhamburger.Application.DTOs;

public record CreateProductDto(
    string Name,
    decimal Price,
    ProductType Type
);
