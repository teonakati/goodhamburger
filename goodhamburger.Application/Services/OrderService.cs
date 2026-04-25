using goodhamburger.Application.DTOs;
using goodhamburger.Domain.Entities;
using goodhamburger.Domain.Repositories;

namespace goodhamburger.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        var items = await _repository.GetAllAsync();
        return items.Select(ToDto);
    }

    public async Task<OrderDto?> GetByIdAsync(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        return item is null ? null : ToDto(item);
    }

    public async Task<OrderDto> CreateAsync(CreateOrderDto dto)
    {
        var order = new Order(dto.Total, dto.Discount);
        await _repository.AddAsync(order);
        return ToDto(order);
    }

    public async Task UpdateAsync(int id, CreateOrderDto dto)
    {
        var order = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Order with id {id} not found.");

        order.Update(dto.Total, dto.Discount);
        await _repository.UpdateAsync(order);
    }

    public async Task DeleteAsync(int id) =>
        await _repository.DeleteAsync(id);

    private static OrderDto ToDto(Order o) =>
        new(o.Id, o.Total, o.Discount, o.Total - o.Discount, o.CreatedAt);
}
