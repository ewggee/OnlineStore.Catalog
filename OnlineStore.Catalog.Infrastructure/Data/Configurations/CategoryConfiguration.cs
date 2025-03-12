using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Catalog.Domain.Entities;

namespace OnlineStore.Catalog.Infrastructure.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        // Свойства
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("category_id");

        builder.Property(c => c.Name)
            .HasColumnName("name")
            .IsRequired(true);

        builder.Property(c => c.ParentCategoryId)
            .HasColumnName("parent_category_id");

        // Связи
        builder.HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId);

        // Сидирование
        builder.HasData(
            new Category
            {
                Id = 1,
                Name = "Музыкальные инструменты"
            },
            new Category
            {
                Id = 2,
                Name = "Гитары",
                ParentCategoryId = 1
            },
            new Category
            {
                Id = 3,
                Name = "Клавишные",
                ParentCategoryId = 1
            },
            new Category
            {
                Id = 4,
                Name = "Ударные",
                ParentCategoryId = 1
            },
            new Category
            {
                Id = 5,
                Name = "Бас гитары",
                ParentCategoryId = 2
            },
            new Category
            {
                Id = 6,
                Name = "Электрогитары",
                ParentCategoryId = 2
            }
        );
    }
}
