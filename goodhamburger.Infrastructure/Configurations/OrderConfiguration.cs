using goodhamburger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace goodhamburger.Infrastructure.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("ORDER");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasColumnName("ID")
            .UseIdentityAlwaysColumn();

        builder.Property(o => o.Total)
            .HasColumnName("TOTAL")
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        builder.Property(o => o.Discount)
            .HasColumnName("DISCOUNT")
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        builder.Property(o => o.CreatedAt)
            .HasColumnName("CREATED_AT")
            .IsRequired();
    }
}
