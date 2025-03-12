using Microsoft.EntityFrameworkCore;
using OnlineStore.Catalog.Domain.Entities;
using OnlineStore.Catalog.Domain.Repositories;

namespace OnlineStore.Catalog.Infrastructure.Data.Repositories;

public sealed class ImageRepository(
    MutableOnlineStoreDbContext mutableDbContext,
    ReadonlyOnlineStoreDbContext readOnlyDbContext)
    : RepositoryBase<ProductImage>(mutableDbContext, readOnlyDbContext), IImageRepository
{
    public Task<int> RemoveImagesWithProductIdNull()
    {
        return MutableDbContext
            .Set<ProductImage>()
            .Where(pi => pi.ProductId == null)
            .ExecuteDeleteAsync();
    }

    public async Task<int> SaveAsync(ProductImage image, CancellationToken cancellation)
    {
        await MutableDbContext.AddAsync(image, cancellation);
        await MutableDbContext.SaveChangesAsync(cancellation);

        return image.Id;
    }
}
