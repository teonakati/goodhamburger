namespace goodhamburger.Application.DTOs;

public record OrderDto(
    int Id,
    decimal Total,
    decimal Discount,
    decimal NetTotal,
    DateTime CreatedAt
);
