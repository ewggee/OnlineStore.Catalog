using OnlineStore.Catalog.Domain.Entities;

namespace OnlineStore.Catalog.Domain.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    /// <summary>
    /// Возвращает навигационные категории (которые не имеют товаров, служат для построения иерархии категорий).
    /// <br>Исключает категорию по ID, а также её подкатегории.</br>
    /// </summary>
    /// <param name="categoryId">ID категории, которую нужно исключить из выборки. Исключаются также её подкатегории.</param>
    Task<List<Category>> GetNavigationCategoriesAsync(CancellationToken cancellation, int? categoryId);

    /// <summary>
    /// Возвращает категории, которые не имеют подкатегорий.
    /// </summary>
    Task<List<Category>> GetWithoutSubsAsync(CancellationToken cancellation);

    new Task<int> AddAsync(Category category, CancellationToken cancellation);

    /// <summary>
    /// Возвращает <b>true</b>, если категория с ID <paramref name="categoryId"/> обладает подкатегориями, иначе <b>false</b>.
    /// </summary>
    /// <param name="categoryId">ID категории.</param>
    Task<bool> IsCategoryHasSubcategoriesAsync(int categoryId, CancellationToken cancellation);
}
