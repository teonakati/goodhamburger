namespace goodhamburger.Domain.Entities;

public class Order
{
    private readonly List<OrderProduct> _orderProducts = [];

    public int Id { get; private set; }
    public decimal Subtotal { get; private set; }
    public decimal Discount { get; private set; }
    public decimal Total { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<OrderProduct> OrderProducts => _orderProducts.AsReadOnly();

    protected Order() { }

    public Order(decimal subtotal, decimal discount, decimal total)
    {
        Subtotal = subtotal;
        Discount = discount;
        Total = total;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddProduct(OrderProduct orderProduct) =>
        _orderProducts.Add(orderProduct);

    public void Update(decimal subtotal, decimal discount, decimal total)
    {
        Subtotal = subtotal;
        Discount = discount;
        Total = total;
    }
}
