namespace OnlineStore.Catalog.Contracts.Dtos;

/// <summary>
/// Краткая информация о товаре.
/// </summary>
public sealed class ShortProductDto
{
    public int Id { get; set; }

    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Описание.
    /// </summary>
    public string Description { get; set; } = default!;

    /// <summary>
    /// Цена.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Имеющееся количество.
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// ID категории.
    /// </summary>
    public int? CategoryId { get; set; }

    /// <summary>
    /// Титульное изображение.
    /// </summary>
    public string MainImageUrl { get; set; } = default!;

    /// <summary>
    /// Список изображений.
    /// </summary>
    public string[] ImagesUrls { get; set; } = [];
}
