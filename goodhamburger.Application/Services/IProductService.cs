using goodhamburger.Application.DTOs;
using goodhamburger.Domain.Enums;

namespace goodhamburger.Application.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task<IEnumerable<ProductDto>> GetByTypeAsync(ProductType type);
    Task<ProductDto?> GetByIdAsync(int id);
    Task<ProductDto> CreateAsync(CreateProductDto dto);
    Task UpdateAsync(int id, CreateProductDto dto);
    Task DeleteAsync(int id);
}
