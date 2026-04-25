using goodhamburger.Application.DTOs;

namespace goodhamburger.Application.Services;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetAllAsync();
    Task<OrderDto?> GetByIdAsync(int id);
    Task<OrderDto> CreateAsync(CreateOrderDto dto);
    Task UpdateAsync(int id, CreateOrderDto dto);
    Task DeleteAsync(int id);
}
