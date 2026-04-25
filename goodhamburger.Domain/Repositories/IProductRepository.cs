using goodhamburger.Domain.Entities;
using goodhamburger.Domain.Enums;

namespace goodhamburger.Domain.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<Product>> GetByTypeAsync(ProductType type);
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<int> ids);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
}
