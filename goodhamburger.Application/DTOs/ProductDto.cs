using goodhamburger.Domain.Enums;

namespace goodhamburger.Application.DTOs;

public record ProductDto(
    int Id,
    string Name,
    decimal Price,
    ProductType Type,
    string TypeDescription,
    DateTime CreatedAt
);
