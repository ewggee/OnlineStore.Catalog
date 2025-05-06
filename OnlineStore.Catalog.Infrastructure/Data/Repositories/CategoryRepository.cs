using Microsoft.EntityFrameworkCore;
using OnlineStore.Catalog.Domain.Entities;
using OnlineStore.Catalog.Domain.Repositories;

namespace OnlineStore.Catalog.Infrastructure.Data.Repositories;

public sealed class CategoryRepository(
    MutableOnlineStoreDbContext mutableDbContext,
    ReadonlyOnlineStoreDbContext readOnlyDbContext)
    : RepositoryBase<Category>(mutableDbContext, readOnlyDbContext), ICategoryRepository
{
    public Task<List<Category>> GetNavigationCategoriesAsync(CancellationToken cancellation, int? categoryId)
    {
        var query = ReadOnlyDbContext
            .Set<Category>()
            .Where(c => c.Products.Count == 0);

        if (categoryId != null)
        {
            query = query
                .Where(c => c.Id != categoryId);

            var dependentCategoriesIds = GetSubcategoriesIds(categoryId.Value).ToList();

            query = query
                .Where(c => !dependentCategoriesIds.Contains(c.Id));
        }

        return query.ToListAsync(cancellation);
    }

    public Task<List<Category>> GetWithoutSubsAsync(CancellationToken cancellation)
    {
        var categories = ReadOnlyDbContext
            .Set<Category>()
            .AsQueryable();

        var query = categories
            .Where(c => categories
                .All(subc => subc.ParentCategoryId != c.Id)
            );

        return query.ToListAsync(cancellation);
    }

    public new async Task<int> AddAsync(Category category, CancellationToken cancellation)
    {
        var result = await MutableDbContext.AddAsync(category, cancellation);
        await MutableDbContext.SaveChangesAsync(cancellation);

        return result.Entity.Id;
    }

    public Task<bool> IsCategoryHasSubcategoriesAsync(int categoryId, CancellationToken cancellation)
    {
        return ReadOnlyDbContext
            .Set<Category>()
            .AnyAsync(subc => subc.ParentCategoryId == categoryId, cancellation);
    }

    private IEnumerable<int> GetSubcategoriesIds(int categoryId)
    {
        var subcategoriesIds = ReadOnlyDbContext
            .Set<Category>()
            .Where(c => c.ParentCategoryId == categoryId)
            .Select(subc => subc.Id)
            .ToList();

        return subcategoriesIds
            .Union(subcategoriesIds.SelectMany(GetSubcategoriesIds));
    }
}