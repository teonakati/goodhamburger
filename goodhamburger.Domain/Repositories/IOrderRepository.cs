using goodhamburger.Domain.Entities;

namespace goodhamburger.Domain.Repositories;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order, IEnumerable<OrderProduct> newProducts);
    Task DeleteAsync(int id);
}
