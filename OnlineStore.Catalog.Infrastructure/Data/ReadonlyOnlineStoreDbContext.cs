using Microsoft.EntityFrameworkCore;

namespace OnlineStore.Catalog.Infrastructure.Data;

public class ReadonlyOnlineStoreDbContext : DbContext
{
    public ReadonlyOnlineStoreDbContext(DbContextOptions<ReadonlyOnlineStoreDbContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReadonlyOnlineStoreDbContext).Assembly);
    }
}
