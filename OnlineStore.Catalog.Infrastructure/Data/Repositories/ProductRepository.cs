using Microsoft.EntityFrameworkCore;
using OnlineStore.Catalog.Domain.Entities;
using OnlineStore.Catalog.Domain.Repositories;
using OnlineStore.Catalog.Domain.Specifications;

namespace OnlineStore.Catalog.Infrastructure.Data.Repositories;

/// <summary>
/// Репозиторий по работе с товарами.
/// </summary>
public sealed class ProductRepository(
    MutableOnlineStoreDbContext mutableDbContext,
    ReadonlyOnlineStoreDbContext readOnlyDbContext) : RepositoryBase<Product>(mutableDbContext, readOnlyDbContext), IProductRepository
{
    /// <inheritdoc/>
    public Task<List<Product>> GetAllAsync(int[] ids, CancellationToken cancellation)
    {
        return ReadOnlyDbContext
            .Set<Product>()
            .Where(p => ids.Contains(p.Id))
            .Where(p => p.IsDeleted == false)
            .ToListAsync(cancellation);
    }

    /// <inheritdoc/>
    public Task<List<Product>> GetAllAsync(BaseSpecification<Product> spec, CancellationToken cancellation)
    {
        var query = ReadOnlyDbContext
            .Set<Product>()
            .AsQueryable();

        query = spec.ApplySpecification(query);

        return query.ToListAsync(cancellation);
    }

    /// <inheritdoc/>
    //public Task<List<Product>> GetByRequestAsync(ISpecification<Product> spec, CancellationToken cancellation)
    //{
    //    var query = ReadOnlyDbContext
    //        .Set<Product>()
    //        .AsQueryable();

    //    if (spec.Criteria != null)
    //    {
    //        query = query.Where(spec.Criteria);
    //    }

    //    if (spec.Ordering != null)
    //    {
    //        query = spec.Ordering(query);
    //    }

    //    if (spec.Includes != null)
    //    {
    //        query = spec.Includes(query);
    //    }

    //    if (spec.Pagination.HasValue)
    //    {
    //        var pageNumber = spec.Pagination.Value.Number;
    //        var pageSize = spec.Pagination.Value.Size;

    //        query = query
    //            .Skip((pageNumber - 1) * pageSize)
    //            .Take(pageSize);
    //    }

    //    return query.ToListAsync(cancellation);
    //}

    /// <inheritdoc/>
    //public Task<List<Product>> GetByRequestAsync(GetProductsRequest request, CancellationToken cancellation)
    //{
    //    var query = ReadOnlyDbContext
    //        .Set<Product>()
    //        .Where(p => p.CategoryId == request.CategoryId)
    //        .Where(p => p.IsDeleted == false);

    //    query = request.Sort switch
    //    {
    //        ProductsSortEnum.PriceDesc => query.OrderByDescending(p => p.Price),
    //        ProductsSortEnum.PriceAsc => query.OrderBy(p => p.Price),
    //        ProductsSortEnum.NoveltyDesc => query.OrderByDescending(p => p.CreatedAt),
    //        ProductsSortEnum.NoveltyAsc => query.OrderBy(p => p.CreatedAt),
    //        ProductsSortEnum.Default => query.OrderBy(p => p.Id),
    //        _ => throw new ArgumentException("Incorrect product sorting method.")
    //    };

    //    query = query
    //        .Skip((request.Page - 1) * _pageSize)
    //        .Take(_pageSize);

    //    return query.ToListAsync(cancellation);
    //}

    /// <inheritdoc/>
    public Task<List<Product>> GetAllAsync(int categoryId, CancellationToken cancellation)
    {
        var query =
            ReadOnlyDbContext
            .Set<Product>()
            .Where(p => p.CategoryId == categoryId)
            .Where(p => p.IsDeleted == false);

        return query.ToListAsync(cancellation);
    }

    /// <inheritdoc/>
    public Task<int> GetProductsTotalCountAsync(int categoryId, CancellationToken cancellation)
    {
        return ReadOnlyDbContext
            .Set<Product>()
            .Where(p => p.CategoryId == categoryId)
            .Where(p => p.IsDeleted == false)
            .CountAsync(cancellation);
    }

    /// <inheritdoc/>
    public override Task<Product?> GetAsync(int id, CancellationToken cancellation)
    {
        return ReadOnlyDbContext
            .Set<Product>()
            .Where(p => p.Id == id)
            .Where(p => p.IsDeleted == false)
            .Include(p => p.Images)
            .FirstOrDefaultAsync();
    }

    public new async Task<int> AddAsync(Product entity, CancellationToken cancellation)
    {
        var result = await MutableDbContext.AddAsync(entity, cancellation);
        await MutableDbContext.SaveChangesAsync();

        return result.Entity.Id;
    }

    /// <inheritdoc/>
    public async Task AddRangeAsync(Product[] products, CancellationToken cancellation)
    {
        await MutableDbContext.AddRangeAsync(products, cancellation);
        await MutableDbContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public override Task UpdateAsync(Product product, CancellationToken cancellation)
    {
        MutableDbContext.Set<Product>().Attach(product);

        MutableDbContext.Entry(product).State = EntityState.Modified;
        MutableDbContext.Entry(product).Property(p => p.CreatedAt).IsModified = false;

        return MutableDbContext.SaveChangesAsync(cancellation);
    }

    /// <inheritdoc/>
    public Task UpdateProductsCountAsync(Product[] products, CancellationToken cancellation)
    {
        //MutableDbContext.Set<Product>().AttachRange(products);
        //products.ForEach(p => MutableDbContext.Entry(p).Property(p => p.StockQuantity).IsModified = true);

        //return MutableDbContext.SaveChangesAsync(cancellation);

        foreach (Product product in products)
        {
            MutableDbContext.Set<Product>().Attach(product);
            MutableDbContext.Entry(product).Property(p => p.StockQuantity).IsModified = true;
        }

        return MutableDbContext.SaveChangesAsync(cancellation);
    }

    /// <summary>
    /// Мягко удаляет товар.
    /// </summary>
    public override Task DeleteAsync(Product product, CancellationToken cancellation)
    {
        MutableDbContext.Set<Product>().Attach(product);
        MutableDbContext.Entry(product).Property(p => p.IsDeleted).IsModified = true;

        return MutableDbContext.SaveChangesAsync(cancellation);
    }

    /// <inheritdoc/>
    public Task<bool> IsCategoryHasProductsAsync(int categoryId, CancellationToken cancellation)
    {
        return ReadOnlyDbContext.Set<Product>()
            .AnyAsync(p => p.CategoryId == categoryId, cancellation);
    }
}
