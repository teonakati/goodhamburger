using goodhamburger.Domain.Entities;
using goodhamburger.Domain.Enums;

namespace goodhamburger.Tests.Helpers;

public static class EntityFactory
{
    public static Product MakeProduct(int id, string name, decimal price, ProductType type)
    {
        var product = new Product(name, price, type);
        Set(product, "Id", id);
        return product;
    }

    public static Order MakeOrder(int id, decimal subtotal, decimal discount, decimal total)
    {
        var order = new Order(subtotal, discount, total);
        Set(order, "Id", id);
        return order;
    }

    public static OrderProduct MakeOrderProduct(int orderId, int productId, decimal price, Product? product = null)
    {
        var op = new OrderProduct(orderId, productId, price);
        if (product is not null)
            Set(op, "Product", product);
        return op;
    }

    public static void Set(object obj, string property, object value) =>
        obj.GetType().GetProperty(property)!.SetValue(obj, value);
}
