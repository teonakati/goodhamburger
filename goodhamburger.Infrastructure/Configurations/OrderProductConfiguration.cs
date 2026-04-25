using goodhamburger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace goodhamburger.Infrastructure.Configurations;

public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
{
    public void Configure(EntityTypeBuilder<OrderProduct> builder)
    {
        builder.ToTable("ORDER_PRODUCT");

        builder.HasKey(op => new { op.OrderId, op.ProductId });

        builder.Property(op => op.OrderId)
            .HasColumnName("ORDER_ID");

        builder.Property(op => op.ProductId)
            .HasColumnName("PRODUCT_ID");

        builder.Property(op => op.Price)
            .HasColumnName("PRICE")
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        builder.HasOne(op => op.Order)
            .WithMany(o => o.OrderProducts)
            .HasForeignKey(op => op.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(op => op.Product)
            .WithMany()
            .HasForeignKey(op => op.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
