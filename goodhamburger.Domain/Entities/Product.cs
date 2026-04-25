using goodhamburger.Domain.Enums;

namespace goodhamburger.Domain.Entities;

public class Product
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public ProductType Type { get; private set; }
    public DateTime CreatedAt { get; private set; }

    protected Product() { }

    public Product(string name, decimal price, ProductType type)
    {
        Name = name;
        Price = price;
        Type = type;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, decimal price, ProductType type)
    {
        Name = name;
        Price = price;
        Type = type;
    }
}
