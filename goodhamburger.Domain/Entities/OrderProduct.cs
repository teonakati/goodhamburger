namespace goodhamburger.Domain.Entities;

public class OrderProduct
{
    public int OrderId { get; private set; }
    public int ProductId { get; private set; }
    public decimal Price { get; private set; }

    public Order Order { get; private set; } = null!;
    public Product Product { get; private set; } = null!;

    protected OrderProduct() { }

    public OrderProduct(int productId, decimal price)
    {
        ProductId = productId;
        Price = price;
    }

    public OrderProduct(int orderId, int productId, decimal price)
    {
        OrderId = orderId;
        ProductId = productId;
        Price = price;
    }
}
