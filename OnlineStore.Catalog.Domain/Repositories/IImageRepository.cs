using OnlineStore.Catalog.Domain.Entities;

namespace OnlineStore.Catalog.Domain.Repositories;

/// <summary>
/// Интерфейс репозитория изображений.
/// </summary>
public interface IImageRepository : IRepository<ProductImage>
{
    /// <summary>
    /// Сохраняет изображение в БД и возвращает ID для формирования URL.
    /// </summary>
    /// <returns>ID изображения.</returns>
    Task<int> SaveAsync(ProductImage image, CancellationToken cancellation);

    /// <summary>
    /// Удаляет все изображения в БД с ID == null.
    /// </summary>
    /// <returns></returns>
    Task<int> RemoveImagesWithProductIdNull();
}
