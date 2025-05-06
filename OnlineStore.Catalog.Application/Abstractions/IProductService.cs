using Microsoft.AspNetCore.Http;
using OnlineStore.Catalog.Contracts.Dtos;
using OnlineStore.Catalog.Contracts.Requests;

namespace OnlineStore.Catalog.Application.Abstractions;

/// <summary>
/// Интерфейс сервиса по работе с товарами.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Возвращает информацию о товаре по его ID.
    /// </summary>
    /// <param name="productId">Идентификатор продукта.</param>
    Task<ShortProductDto?> GetAsync(int productId, CancellationToken cancellation);

    /// <summary>
    /// Возвращает список товаров в категории по запросу.
    /// </summary>
    /// <param name="request">Запрос на получение списка товаров.</param>
    /// <param name="categoryDto">Существующуая категория.</param>
    Task<ProductsListDto> GetAllAsync(GetProductsRequest request, CategoryDto categoryDto, CancellationToken cancellation);

    /// <summary>
    /// Возвращает список товаров по ID категории.
    /// </summary>
    Task<List<ShortProductDto>> GetAllAsync(int categoryId, CancellationToken cancellation);

    /// <summary>
    /// Возвращает список товаров по их ID.
    /// </summary>
    /// <param name="productsIds">Массив ID товаров.</param>
    Task<List<ShortProductDto>> GetAllAsync(int[] productsIds, CancellationToken cancellation);

    /// <summary>
    /// Добавляет товар и возвращает сгенерированный ID.
    /// </summary>
    /// <param name="productDto">Транспортная модель товара.</param>
    Task<int> AddProductAsync(ShortProductDto productDto, CancellationToken cancellation);

    /// <summary>
    /// Добавляет товары.
    /// </summary>
    Task AddProductsAsync(ShortProductDto[] productDtos, CancellationToken cancellation);

    ///// <summary>
    ///// Добавляет массив товаров.
    ///// </summary>
    //Task AddProductsAsync(ShortProductDto[] productDto, CancellationToken cancellation);

    /// <summary>
    /// Обновляет данные о товаре.
    /// </summary>
    Task UpdateAsync(ShortProductDto productDto, CancellationToken cancellation);

    /// <summary>
    /// Обновляет данные о товарах.
    /// </summary>
    Task UpdateProductsCountAsync(ShortProductDto[] productsDtos, CancellationToken cancellation);

    /// <summary>
    /// "Мягко" удаляет товар по ID.
    /// </summary>
    Task DeleteAsync(int productId, CancellationToken cancellation);
}
