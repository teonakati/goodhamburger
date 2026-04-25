using goodhamburger.Domain.Entities;
using goodhamburger.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace goodhamburger.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("PRODUCT");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("ID")
            .UseIdentityAlwaysColumn();

        builder.Property(p => p.Name)
            .HasColumnName("NAME")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.Price)
            .HasColumnName("PRICE")
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        builder.Property(p => p.Type)
            .HasColumnName("TYPE")
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .HasColumnName("CREATED_AT")
            .IsRequired();

        builder.HasData(
            new { Id = 1, Name = "X-Burguer",    Price = 5m,   Type = ProductType.Sandwich, CreatedAt = new DateTime(2026, 4, 24, 0, 0, 0, DateTimeKind.Utc) },
            new { Id = 2, Name = "X-Bacon",       Price = 7m,   Type = ProductType.Sandwich, CreatedAt = new DateTime(2026, 4, 24, 0, 0, 0, DateTimeKind.Utc) },
            new { Id = 3, Name = "X-Egg",         Price = 4.5m, Type = ProductType.Sandwich, CreatedAt = new DateTime(2026, 4, 24, 0, 0, 0, DateTimeKind.Utc) },
            new { Id = 4, Name = "Batata Frita",  Price = 2m,   Type = ProductType.SideDish, CreatedAt = new DateTime(2026, 4, 24, 0, 0, 0, DateTimeKind.Utc) },
            new { Id = 5, Name = "Refrigerante",  Price = 2.5m, Type = ProductType.SideDish, CreatedAt = new DateTime(2026, 4, 24, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
