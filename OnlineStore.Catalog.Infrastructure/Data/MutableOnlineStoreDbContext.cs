using Microsoft.EntityFrameworkCore;

namespace OnlineStore.Catalog.Infrastructure.Data;

public class MutableOnlineStoreDbContext : DbContext
{
    public MutableOnlineStoreDbContext(DbContextOptions<MutableOnlineStoreDbContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MutableOnlineStoreDbContext).Assembly);
    }
}
