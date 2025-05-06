using OnlineStore.Catalog.Contracts.Dtos;

namespace OnlineStore.Catalog.Application.Abstractions;

/// <summary>
/// Сервис по работе с категориями.
/// </summary>
public interface ICategoryService
{
    /// <summary>
    /// Возвращает список главных категорий.
    /// </summary>
    Task<IReadOnlyList<CategoryDto>> GetMainCategoriesAsync(CancellationToken cancellation);

    /// <summary>
    /// Возвращает категорию по ID.
    /// </summary>
    Task<CategoryDto> GetAsync(int categoryId, CancellationToken cancellation);

    /// <summary>
    /// Возвращает список категорию с подкатегориям.
    /// </summary>
    Task<CategoryWithSubcategoriesDto> GetCategoryWithSubcategoriesAsync(int categoryId, CancellationToken cancellation);

    /// <summary>
    /// Возвращает список навигационных категорий (те, которые не содержат в себе товаров).
    /// <br>Исключает категорию с ID <paramref name="categoryId"/> и её подкатегории, которые нужно исключить из выборки, например, при обновлении данных (выбор родительской категории).</br>
    /// </summary>
    /// <param name="categoryId">ID категории, которую нужно исключить из выборки.</param>
    Task<IReadOnlyList<CategoryDto>> GetNavigationCategoriesAsync(CancellationToken cancellation, int? categoryId = null);

    /// <summary>
    /// Возвращает категории, не содержащие подкатегории.
    /// </summary>
    Task<IReadOnlyList<CategoryDto>> GetWithoutSubcategories(CancellationToken cancellation);

    /// <summary>
    /// Добавляет категорию в БД.
    /// </summary>
    Task<int> AddAsync(CategoryDto categoryDto, CancellationToken cancellation);

    /// <summary>
    /// Обновляет значения категории.
    /// </summary>
    Task UpdateAsync(CategoryDto categoryDto, CancellationToken cancellation);

    /// <summary>
    /// Удаляет категорию.
    /// </summary>
    Task DeleteAsync(int categoryId, CancellationToken cancellation);

    /// <summary>
    /// Возвращает <b>true</b>, если категория с ID <paramref name="categoryId"/> обладает подкатегориями, иначе <b>false</b>.
    /// </summary>
    /// <param name="categoryId">ID категории.</param>
    Task<bool> IsCategoryHasSubcategoriesAsync(int categoryId, CancellationToken cancellation);

    /// <summary>
    /// Возвращает <b>true</b>, если в категории с ID <paramref name="categoryId"/> есть товары, иначе <b>false</b>.
    /// </summary>
    /// <param name="categoryId">ID категории.</param>
    Task<bool> IsCategoryHasProductsAsync(int categoryId, CancellationToken cancellation);
}
