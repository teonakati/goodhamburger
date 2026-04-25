using goodhamburger.Domain.Entities;
using goodhamburger.Domain.Repositories;
using goodhamburger.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace goodhamburger.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetAllAsync() =>
        await _context.Orders
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .AsNoTracking()
            .ToListAsync();

    public async Task<Order?> GetByIdAsync(int id) =>
        await _context.Orders
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id);

    public async Task AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order order, IEnumerable<OrderProduct> newProducts)
    {
        var tracked = await _context.Orders
            .Include(o => o.OrderProducts)
            .FirstAsync(o => o.Id == order.Id);

        tracked.Update(order.Subtotal, order.Discount, order.Total);

        _context.OrderProducts.RemoveRange(tracked.OrderProducts);
        _context.OrderProducts.AddRange(newProducts);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id)
            ?? throw new KeyNotFoundException($"Order with id {id} not found.");

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }
}
