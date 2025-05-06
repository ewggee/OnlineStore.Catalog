namespace OnlineStore.Catalog.Domain.Entities;

/// <summary>
/// Изображение товара.
/// </summary>
public class ProductImage
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Наименование.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Контент изображения.
    /// </summary>
    public byte[]? Content { get; set; }

    /// <summary>
    /// Тип контента.
    /// </summary>
    public string? ContentType { get; set; }

    /// <summary>
    /// URL изображения.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Идентификатор товара.
    /// </summary>
    public int? ProductId { get; set; }

    /// <summary>
    /// Товар.
    /// </summary>
    public Product Product { get; set; } = default!;
}
