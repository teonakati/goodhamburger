using goodhamburger.Application.DTOs;
using goodhamburger.Application.Services;
using goodhamburger.Domain.Entities;
using goodhamburger.Domain.Enums;
using goodhamburger.Domain.Repositories;
using goodhamburger.Tests.Helpers;
using Moq;

namespace goodhamburger.Tests.Services;

public class OrderServiceValidationTests
{
    private readonly Mock<IOrderRepository> _orderRepo = new();
    private readonly Mock<IProductRepository> _productRepo = new();
    private readonly OrderService _service;

    public OrderServiceValidationTests()
    {
        _service = new OrderService(_orderRepo.Object, _productRepo.Object);
    }

    [Fact]
    public async Task Duplicate_ProductIds_Throws_InvalidOperationException()
    {
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateAsync(new CreateOrderDto([1, 1])));

        Assert.Contains("Duplicate product IDs", ex.Message);
    }

    [Fact]
    public async Task Missing_Product_Throws_KeyNotFoundException()
    {
        _productRepo
            .Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync([]);

        var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.CreateAsync(new CreateOrderDto([99])));

        Assert.Contains("99", ex.Message);
    }

    [Fact]
    public async Task Two_Sandwiches_Throws_InvalidOperationException()
    {
        _productRepo
            .Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync([
                EntityFactory.MakeProduct(1, "X-Burger", 10m, ProductType.Sandwich),
                EntityFactory.MakeProduct(2, "X-Bacon",  12m, ProductType.Sandwich),
            ]);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateAsync(new CreateOrderDto([1, 2])));

        Assert.Contains("sandwich", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Two_SideDishes_Throws_InvalidOperationException()
    {
        _productRepo
            .Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync([
                EntityFactory.MakeProduct(3, "Batata Frita",  5m, ProductType.SideDish),
                EntityFactory.MakeProduct(4, "Onion Rings",   6m, ProductType.SideDish),
            ]);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateAsync(new CreateOrderDto([3, 4])));

        Assert.Contains("side dish", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Two_Drinks_Throws_InvalidOperationException()
    {
        _productRepo
            .Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync([
                EntityFactory.MakeProduct(5, "Coca-Cola", 4m, ProductType.Drink),
                EntityFactory.MakeProduct(6, "Suco",      4m, ProductType.Drink),
            ]);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateAsync(new CreateOrderDto([5, 6])));

        Assert.Contains("drink", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Empty_ProductIds_Returns_Order_With_No_Discount()
    {
        _productRepo
            .Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync([]);

        _orderRepo.Setup(r => r.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);
        _orderRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(EntityFactory.MakeOrder(1, 0m, 0m, 0m));

        var result = await _service.CreateAsync(new CreateOrderDto([]));

        Assert.Equal(0m, result.Discount);
        Assert.Equal(0m, result.Total);
    }
}
