namespace goodhamburger.Domain.Entities;

public class Order
{
    public int Id { get; private set; }
    public decimal Total { get; private set; }
    public decimal Discount { get; private set; }
    public DateTime CreatedAt { get; private set; }

    protected Order() { }

    public Order(decimal total, decimal discount)
    {
        Total = total;
        Discount = discount;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(decimal total, decimal discount)
    {
        Total = total;
        Discount = discount;
    }
}
