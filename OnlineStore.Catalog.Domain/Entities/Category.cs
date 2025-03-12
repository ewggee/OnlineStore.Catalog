namespace OnlineStore.Catalog.Domain.Entities;

/// <summary>
/// Категория.
/// </summary>
public class Category
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Наименование.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// ID родительской категории.
    /// </summary>
    public int? ParentCategoryId { get; set; }

    /// <summary>
    /// Навигационное свойство родительской категории.
    /// </summary>
    public Category? ParentCategory { get; set; }

    /// <summary>
    /// Навигационное свойство товаров в категории.
    /// </summary>
    public ICollection<Product> Products { get; set; } = [];
}