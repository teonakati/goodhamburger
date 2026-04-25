using goodhamburger.Domain.Entities;
using goodhamburger.Domain.Enums;
using goodhamburger.Domain.Repositories;
using goodhamburger.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace goodhamburger.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync() =>
        await _context.Products.AsNoTracking().ToListAsync();

    public async Task<IEnumerable<Product>> GetByTypeAsync(ProductType type) =>
        await _context.Products.AsNoTracking().Where(p => p.Type == type).ToListAsync();

    public async Task<Product?> GetByIdAsync(int id) =>
        await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id)
            ?? throw new KeyNotFoundException($"Product with id {id} not found.");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}
