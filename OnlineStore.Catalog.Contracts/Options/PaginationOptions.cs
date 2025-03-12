using Microsoft.Extensions.Configuration;

namespace OnlineStore.Catalog.Contracts.Options;

public class PaginationOptions
{
    [ConfigurationKeyName("ProductsPageSize")]
    public int ProductsPageSize { get; set; }

    [ConfigurationKeyName("OrdersPageSize")]
    public int OrdersPageSize { get; set; }
}
