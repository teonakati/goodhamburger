namespace goodhamburger.Application.DTOs;

public record OrderDto(
    int Id,
    decimal Subtotal,
    decimal Discount,
    decimal Total,
    DateTime CreatedAt,
    IEnumerable<OrderProductDto> Products
);
