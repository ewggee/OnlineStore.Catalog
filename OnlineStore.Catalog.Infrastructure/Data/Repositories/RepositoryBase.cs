using Microsoft.EntityFrameworkCore;
using OnlineStore.Catalog.Domain.Repositories;
using System.Linq.Expressions;

namespace OnlineStore.Catalog.Infrastructure.Data.Repositories;

/// <summary>
/// Базовый репозиторий для работы с БД через EF.
/// </summary>
public abstract class RepositoryBase<T> : IRepository<T> where T : class
{
    protected readonly MutableOnlineStoreDbContext MutableDbContext;
    protected readonly ReadonlyOnlineStoreDbContext ReadOnlyDbContext;

    /// <inheritdoc/>
    public RepositoryBase(
        MutableOnlineStoreDbContext mutableDbContext,
        ReadonlyOnlineStoreDbContext readOnlyDbContext)
    {
        MutableDbContext = mutableDbContext;
        ReadOnlyDbContext = readOnlyDbContext;
    }

    /// <inheritdoc/>
    public virtual Task<T?> GetAsync(int id)
    {
        return MutableDbContext.FindAsync<T>(id).AsTask();
    }

    /// <inheritdoc/>
    public virtual Task<T[]> GetAllAsync(CancellationToken cancellation)
    {
        return ReadOnlyDbContext.Set<T>()
            .ToArrayAsync(cancellation);
    }

    /// <inheritdoc/>
    public virtual Task<T[]> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellation)
    {
        return ReadOnlyDbContext.Set<T>()
            .Where(predicate)
            .ToArrayAsync(cancellation);
    }

    /// <inheritdoc/>
    public virtual async Task AddAsync(T entity, CancellationToken cancellation)
    {
        await MutableDbContext.AddAsync(entity, cancellation);
        await MutableDbContext.SaveChangesAsync(cancellation);
    }

    /// <inheritdoc/>
    public virtual Task UpdateAsync(T entity, CancellationToken cancellation)
    {
        MutableDbContext.Set<T>().Attach(entity);
        MutableDbContext.Entry(entity).State = EntityState.Modified;

        return MutableDbContext.SaveChangesAsync(cancellation);
    }

    /// <inheritdoc/>
    public virtual Task DeleteAsync(T entity, CancellationToken cancellation)
    {
        MutableDbContext.Remove(entity);
        return MutableDbContext.SaveChangesAsync(cancellation);
    }
}
