using goodhamburger.Domain.Enums;

namespace goodhamburger.Application.DTOs;

public record OrderProductDto(
    int ProductId,
    string ProductName,
    ProductType ProductType,
    decimal Price
);
