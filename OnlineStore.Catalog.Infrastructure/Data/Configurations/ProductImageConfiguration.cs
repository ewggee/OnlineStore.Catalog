using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Catalog.Domain.Entities;

namespace OnlineStore.Catalog.Infrastructure.Data.Configurations;

public sealed class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable("product_images");

        // Свойства
        builder.HasKey(pi => pi.Id);
        builder.Property(pi => pi.Id)
            .HasColumnName("product_image_id");

        builder.Property(pi => pi.ProductId)
            .HasColumnName("product_id");

        builder.Property(pi => pi.Name)
            .HasColumnName("name")
            .HasMaxLength(1000)
            .IsRequired(true);

        builder.Property(pi => pi.Content)
            .HasColumnName("content");

        builder.Property(pi => pi.ContentType)
            .HasColumnName("content_type");

        // Связи
        builder.HasOne(pi => pi.Product)
            .WithMany(p => p.Images)
            .HasForeignKey(pi => pi.ProductId);
    }
}
