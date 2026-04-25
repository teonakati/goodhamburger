using goodhamburger.Application.DTOs;
using goodhamburger.Domain.Entities;
using goodhamburger.Domain.Enums;
using goodhamburger.Domain.Repositories;

namespace goodhamburger.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var items = await _repository.GetAllAsync();
        return items.Select(ToDto);
    }

    public async Task<IEnumerable<ProductDto>> GetByTypeAsync(ProductType type)
    {
        var items = await _repository.GetByTypeAsync(type);
        return items.Select(ToDto);
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        return item is null ? null : ToDto(item);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var product = new Product(dto.Name, dto.Price, dto.Type);
        await _repository.AddAsync(product);
        return ToDto(product);
    }

    public async Task UpdateAsync(int id, CreateProductDto dto)
    {
        var product = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Product with id {id} not found.");

        product.Update(dto.Name, dto.Price, dto.Type);
        await _repository.UpdateAsync(product);
    }

    public async Task DeleteAsync(int id) =>
        await _repository.DeleteAsync(id);

    private static ProductDto ToDto(Product p) =>
        new(p.Id, p.Name, p.Price, p.Type, p.Type.ToString(), p.CreatedAt);
}
