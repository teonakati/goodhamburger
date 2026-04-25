namespace goodhamburger.Application.DTOs;

public record CreateOrderDto(
    decimal Total,
    decimal Discount
);
