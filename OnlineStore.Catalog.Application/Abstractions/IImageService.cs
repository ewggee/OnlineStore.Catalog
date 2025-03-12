using Microsoft.AspNetCore.Http;
using OnlineStore.Catalog.Contracts.Dtos;
using OnlineStore.Catalog.Domain.Entities;

namespace OnlineStore.Catalog.Application.Abstractions;

/// <summary>
/// Интерфейс сервиса по работе с изображениями.
/// </summary>
public interface IImageService
{
    /// <summary>
    /// Сохраняет изображение в БД и возвращает его URL.
    /// </summary>
    Task<string> SaveImageAsync(IFormFile imageFile, CancellationToken cancellation);

    /// <summary>
    /// Сохраняет список изображений в БД и возвращает их URL.
    /// </summary>
    Task<IReadOnlyCollection<string>> SaveImagesAsync(List<IFormFile> ImageFiles, CancellationToken cancellation);

    /// <summary>
    /// Возвращает изображение по ID.
    /// </summary>
    Task<ImageDto> GetAsync(int imageId, CancellationToken cancellation);

    /// <summary>
    /// Возвращает URL изображений из массива.
    /// </summary>
    /// <param name="images">Изображения товара.</param>
    string[] GetImagesUrls(ProductImage[] images);

    /// <summary>
    /// Связывает изображения товара.
    /// </summary>
    /// <param name="imagesUrls">Массив URL дополнительных изображений товара.</param>
    /// <param name="product">Товар, к которому будут прикреплены изображения.</param>
    Task<ProductImage[]> SaveProductImagesAsync(string[] imagesUrls, Product product, CancellationToken cancellation);

    /// <summary>
    /// Извлекает ID изображения из URL.
    /// </summary>
    /// <param name="url">URL изображения.</param>
    int ExtractImageId(string url);

    /// <summary>
    /// Удаляет изображение по ID.
    /// </summary>
    Task DeleteAsync(int imageId, CancellationToken cancellation);
}
