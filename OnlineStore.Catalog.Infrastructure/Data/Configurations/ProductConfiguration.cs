using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Catalog.Domain.Entities;

namespace OnlineStore.Catalog.Infrastructure.Data.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        // Свойства
        builder.HasKey(p => p.Id);
        builder.Property(pat => pat.Id)
            .HasColumnName("product_id");

        builder.Property(p => p.Name)
            .HasColumnName("name")
            .HasMaxLength(1000)
            .IsRequired(true);

        builder.Property(p => p.Description)
            .HasColumnName("description")
            .IsRequired(true);

        builder.Property(p => p.Price)
            .HasColumnName("price")
            .HasPrecision(8, 2)
            .IsRequired(true);

        builder.Property(p => p.CategoryId)
            .HasColumnName("category_id")
            .IsRequired(true);

        builder.Property(p => p.ImageUrl)
            .HasColumnName("image_url");

        builder.Property(p => p.StockQuantity)
            .HasColumnName("stock_quantity")
            .IsRequired(true);

        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired(true);

        builder.Property(p => p.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(p => p.IsDeleted)
            .HasColumnName("deleted")
            .HasDefaultValue(false);

        // Связи
        builder.HasMany(p => p.Images)
            .WithOne(pi => pi.Product)
            .HasForeignKey(pi => pi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
