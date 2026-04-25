using goodhamburger.Application.DTOs;
using goodhamburger.Application.Services;
using goodhamburger.Domain.Entities;
using goodhamburger.Domain.Enums;
using goodhamburger.Domain.Repositories;
using goodhamburger.Tests.Helpers;
using Moq;

namespace goodhamburger.Tests.Services;

public class OrderServiceDiscountTests
{
    private readonly Mock<IOrderRepository> _orderRepo = new();
    private readonly Mock<IProductRepository> _productRepo = new();
    private readonly OrderService _service;

    public OrderServiceDiscountTests()
    {
        _service = new OrderService(_orderRepo.Object, _productRepo.Object);
    }

    [Fact]
    public async Task Sandwich_SideDish_Drink_Applies_20_Percent()
    {
        var products = new List<Product>
        {
            EntityFactory.MakeProduct(1, "X-Burger",     10m, ProductType.Sandwich),
            EntityFactory.MakeProduct(2, "Batata Frita",  5m, ProductType.SideDish),
            EntityFactory.MakeProduct(3, "Refrigerante",  5m, ProductType.Drink),
        };
        // subtotal = 20, discount = 20 * 0.20 = 4, total = 16
        SetupMocks(products);

        var result = await _service.CreateAsync(new CreateOrderDto([1, 2, 3]));

        Assert.Equal(20m, result.Subtotal);
        Assert.Equal(4m,  result.Discount);
        Assert.Equal(16m, result.Total);
    }

    [Fact]
    public async Task Sandwich_Drink_Applies_15_Percent()
    {
        var products = new List<Product>
        {
            EntityFactory.MakeProduct(1, "X-Burger",    10m, ProductType.Sandwich),
            EntityFactory.MakeProduct(3, "Refrigerante", 5m, ProductType.Drink),
        };
        // subtotal = 15, discount = 15 * 0.15 = 2.25, total = 12.75
        SetupMocks(products);

        var result = await _service.CreateAsync(new CreateOrderDto([1, 3]));

        Assert.Equal(15m,    result.Subtotal);
        Assert.Equal(2.25m,  result.Discount);
        Assert.Equal(12.75m, result.Total);
    }

    [Fact]
    public async Task Sandwich_SideDish_Applies_10_Percent()
    {
        var products = new List<Product>
        {
            EntityFactory.MakeProduct(1, "X-Burger",    10m, ProductType.Sandwich),
            EntityFactory.MakeProduct(2, "Batata Frita", 5m, ProductType.SideDish),
        };
        // subtotal = 15, discount = 15 * 0.10 = 1.50, total = 13.50
        SetupMocks(products);

        var result = await _service.CreateAsync(new CreateOrderDto([1, 2]));

        Assert.Equal(15m,   result.Subtotal);
        Assert.Equal(1.50m, result.Discount);
        Assert.Equal(13.50m, result.Total);
    }

    [Fact]
    public async Task No_Matching_Combo_Applies_No_Discount()
    {
        var products = new List<Product>
        {
            EntityFactory.MakeProduct(1, "X-Burger", 10m, ProductType.Sandwich),
        };
        SetupMocks(products);

        var result = await _service.CreateAsync(new CreateOrderDto([1]));

        Assert.Equal(10m, result.Subtotal);
        Assert.Equal(0m,  result.Discount);
        Assert.Equal(10m, result.Total);
    }

    [Fact]
    public async Task SideDish_Only_Applies_No_Discount()
    {
        var products = new List<Product>
        {
            EntityFactory.MakeProduct(2, "Batata Frita", 5m, ProductType.SideDish),
        };
        SetupMocks(products);

        var result = await _service.CreateAsync(new CreateOrderDto([2]));

        Assert.Equal(0m, result.Discount);
        Assert.Equal(result.Subtotal, result.Total);
    }

    [Fact]
    public async Task Discount_Is_Rounded_To_Two_Decimal_Places()
    {
        var products = new List<Product>
        {
            EntityFactory.MakeProduct(1, "X-Burger",    7m, ProductType.Sandwich),
            EntityFactory.MakeProduct(3, "Refrigerante", 4m, ProductType.Drink),
        };
        // subtotal = 11, discount = 11 * 0.15 = 1.65, total = 9.35
        SetupMocks(products);

        var result = await _service.CreateAsync(new CreateOrderDto([1, 3]));

        Assert.Equal(11m,   result.Subtotal);
        Assert.Equal(1.65m, result.Discount);
        Assert.Equal(9.35m, result.Total);
    }

    // ── Setup ──────────────────────────────────────────────────────────────

    private void SetupMocks(List<Product> products)
    {
        _productRepo
            .Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync(products);

        Order? captured = null;
        _orderRepo
            .Setup(r => r.AddAsync(It.IsAny<Order>()))
            .Callback<Order>(o => captured = o)
            .Returns(Task.CompletedTask);

        _orderRepo
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) =>
            {
                EntityFactory.Set(captured!, "Id", id);
                return captured!;
            });
    }
}
