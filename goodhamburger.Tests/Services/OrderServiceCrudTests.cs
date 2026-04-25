using goodhamburger.Application.DTOs;
using goodhamburger.Application.Services;
using goodhamburger.Domain.Entities;
using goodhamburger.Domain.Enums;
using goodhamburger.Domain.Repositories;
using goodhamburger.Tests.Helpers;
using Moq;

namespace goodhamburger.Tests.Services;

public class OrderServiceCrudTests
{
    private readonly Mock<IOrderRepository> _orderRepo = new();
    private readonly Mock<IProductRepository> _productRepo = new();
    private readonly OrderService _service;

    public OrderServiceCrudTests()
    {
        _service = new OrderService(_orderRepo.Object, _productRepo.Object);
    }

    // ── GetAll ─────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetAll_Returns_All_Orders_Mapped_To_Dto()
    {
        var orders = new List<Order>
        {
            EntityFactory.MakeOrder(1, 20m, 4m, 16m),
            EntityFactory.MakeOrder(2, 10m, 0m, 10m),
        };
        _orderRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);

        var result = (await _service.GetAllAsync()).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(2, result[1].Id);
    }

    [Fact]
    public async Task GetAll_Returns_Empty_When_No_Orders()
    {
        _orderRepo.Setup(r => r.GetAllAsync()).ReturnsAsync([]);

        var result = await _service.GetAllAsync();

        Assert.Empty(result);
    }

    // ── GetById ────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetById_Returns_Dto_When_Found()
    {
        var order = EntityFactory.MakeOrder(42, 15m, 1.5m, 13.5m);
        _orderRepo.Setup(r => r.GetByIdAsync(42)).ReturnsAsync(order);

        var result = await _service.GetByIdAsync(42);

        Assert.NotNull(result);
        Assert.Equal(42,    result.Id);
        Assert.Equal(15m,   result.Subtotal);
        Assert.Equal(1.5m,  result.Discount);
        Assert.Equal(13.5m, result.Total);
    }

    [Fact]
    public async Task GetById_Returns_Null_When_Not_Found()
    {
        _orderRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Order?)null);

        var result = await _service.GetByIdAsync(999);

        Assert.Null(result);
    }

    // ── Create ─────────────────────────────────────────────────────────────

    [Fact]
    public async Task Create_Calls_AddAsync_Once()
    {
        var products = new List<Product>
        {
            EntityFactory.MakeProduct(1, "X-Burger", 10m, ProductType.Sandwich),
        };
        SetupProductsAndReload(products);

        await _service.CreateAsync(new CreateOrderDto([1]));

        _orderRepo.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
    }

    [Fact]
    public async Task Create_Returns_Correct_Subtotal_And_Total()
    {
        var products = new List<Product>
        {
            EntityFactory.MakeProduct(1, "X-Burger",    10m, ProductType.Sandwich),
            EntityFactory.MakeProduct(2, "Batata Frita",  5m, ProductType.SideDish),
            EntityFactory.MakeProduct(3, "Refrigerante",  5m, ProductType.Drink),
        };
        SetupProductsAndReload(products);

        var result = await _service.CreateAsync(new CreateOrderDto([1, 2, 3]));

        Assert.Equal(20m, result.Subtotal);
        Assert.Equal(4m,  result.Discount);
        Assert.Equal(16m, result.Total);
    }

    // ── Update ─────────────────────────────────────────────────────────────

    [Fact]
    public async Task Update_Calls_UpdateAsync_Once()
    {
        var existing = EntityFactory.MakeOrder(1, 10m, 0m, 10m);
        var products = new List<Product>
        {
            EntityFactory.MakeProduct(1, "X-Burger", 10m, ProductType.Sandwich),
        };

        _orderRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _productRepo.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<int>>())).ReturnsAsync(products);
        _orderRepo.Setup(r => r.UpdateAsync(It.IsAny<Order>(), It.IsAny<IEnumerable<OrderProduct>>())).Returns(Task.CompletedTask);

        await _service.UpdateAsync(1, new CreateOrderDto([1]));

        _orderRepo.Verify(r => r.UpdateAsync(It.IsAny<Order>(), It.IsAny<IEnumerable<OrderProduct>>()), Times.Once);
    }

    [Fact]
    public async Task Update_Throws_KeyNotFoundException_When_Order_Not_Found()
    {
        _orderRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Order?)null);

        var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.UpdateAsync(999, new CreateOrderDto([1])));

        Assert.Contains("999", ex.Message);
    }

    // ── Delete ─────────────────────────────────────────────────────────────

    [Fact]
    public async Task Delete_Calls_DeleteAsync_Once()
    {
        _orderRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        await _service.DeleteAsync(1);

        _orderRepo.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task Delete_Propagates_KeyNotFoundException_From_Repository()
    {
        _orderRepo
            .Setup(r => r.DeleteAsync(99))
            .ThrowsAsync(new KeyNotFoundException("Order with id 99 not found."));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(99));
    }

    // ── Setup ──────────────────────────────────────────────────────────────

    private void SetupProductsAndReload(List<Product> products)
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
