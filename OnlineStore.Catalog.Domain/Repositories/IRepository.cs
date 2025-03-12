using System.Linq.Expressions;

namespace OnlineStore.Catalog.Domain.Repositories;

/// <summary>
/// Интерфейс общего репозитория.
/// </summary>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Получает сущность по ID.
    /// </summary>
    Task<T?> GetAsync(int id);

    /// <summary>
    /// Добавляет сущность в БД.
    /// </summary>
    /// <param name="entity">Сущность.</param>
    Task AddAsync(T entity, CancellationToken cancellation);

    /// <summary>
    /// Получает все записи.
    /// </summary>
    Task<T[]> GetAllAsync(CancellationToken cancellation);

    /// <summary>
    /// Получает все записи через условие выборки.
    /// </summary>
    /// <param name="predicate">Условие выборки.</param>
    Task<T[]> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellation);

    /// <summary>
    /// Обновляет существующую сущность.
    /// </summary>
    /// <param name="entity">Сущность.</param>
    Task UpdateAsync(T entity, CancellationToken cancellation);

    /// <summary>
    /// Удаляет существующую сущность.
    /// </summary>
    /// <param name="entity">Сущность.</param>
    Task DeleteAsync(T entity, CancellationToken cancellation);
}
