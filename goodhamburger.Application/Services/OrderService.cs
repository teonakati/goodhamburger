using goodhamburger.Application.DTOs;
using goodhamburger.Domain.Entities;
using goodhamburger.Domain.Enums;
using goodhamburger.Domain.Repositories;

namespace goodhamburger.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return orders.Select(ToDto);
    }

    public async Task<OrderDto?> GetByIdAsync(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        return order is null ? null : ToDto(order);
    }

    public async Task<OrderDto> CreateAsync(CreateOrderDto dto)
    {
        var products = await LoadAndValidateProducts(dto.ProductIds);

        var (subtotal, discount, total) = Calculate(products);

        var order = new Order(subtotal, discount, total);
        foreach (var product in products)
            order.AddProduct(new OrderProduct(product.Id, product.Price));

        await _orderRepository.AddAsync(order);

        // Reload to populate Product navigation on each OrderProduct
        var created = await _orderRepository.GetByIdAsync(order.Id);
        return ToDto(created!);
    }

    public async Task UpdateAsync(int id, CreateOrderDto dto)
    {
        var existing = await _orderRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Order with id {id} not found.");

        var products = await LoadAndValidateProducts(dto.ProductIds);

        var (subtotal, discount, total) = Calculate(products);

        existing.Update(subtotal, discount, total);
        foreach (var product in products)
            existing.AddProduct(new OrderProduct(existing.Id, product.Id, product.Price));

        await _orderRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id) =>
        await _orderRepository.DeleteAsync(id);

    // ── Helpers ─────────────────────────────────────────────────────────────

    private async Task<List<Product>> LoadAndValidateProducts(List<int> productIds)
    {
        // Duplicate IDs in the request
        var duplicates = productIds.GroupBy(id => id).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
        if (duplicates.Count > 0)
            throw new InvalidOperationException(
                $"Duplicate product IDs in the request: {string.Join(", ", duplicates)}.");

        var products = (await _productRepository.GetByIdsAsync(productIds)).ToList();

        var missing = productIds.Except(products.Select(p => p.Id)).ToList();
        if (missing.Count > 0)
            throw new KeyNotFoundException(
                $"Products not found: {string.Join(", ", missing)}.");

        // Max 1 per type
        if (products.Count(p => p.Type == ProductType.Sandwich) > 1)
            throw new InvalidOperationException("An order can only contain one sandwich.");
        if (products.Count(p => p.Type == ProductType.SideDish) > 1)
            throw new InvalidOperationException("An order can only contain one side dish.");
        if (products.Count(p => p.Type == ProductType.Drink) > 1)
            throw new InvalidOperationException("An order can only contain one drink.");

        return products;
    }

    private static (decimal subtotal, decimal discount, decimal total) Calculate(List<Product> products)
    {
        var hasSandwich = products.Any(p => p.Type == ProductType.Sandwich);
        var hasSideDish = products.Any(p => p.Type == ProductType.SideDish);
        var hasDrink    = products.Any(p => p.Type == ProductType.Drink);

        var rate = (hasSandwich, hasSideDish, hasDrink) switch
        {
            (true, true, true)   => 0.20m,
            (true, false, true)  => 0.15m,
            (true, true, false)  => 0.10m,
            _                    => 0m
        };

        var subtotal = products.Sum(p => p.Price);
        var discount = Math.Round(subtotal * rate, 2);
        return (subtotal, discount, subtotal - discount);
    }

    private static OrderDto ToDto(Order o) => new(
        o.Id,
        o.Subtotal,
        o.Discount,
        o.Total,
        o.CreatedAt,
        o.OrderProducts.Select(op => new OrderProductDto(
            op.ProductId,
            op.Product?.Name ?? string.Empty,
            op.Product?.Type ?? default,
            op.Price
        ))
    );
}
