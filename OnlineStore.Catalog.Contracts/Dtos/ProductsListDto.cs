using OnlineStore.Catalog.Contracts.Common;
using OnlineStore.Catalog.Contracts.Enums;

namespace OnlineStore.Catalog.Contracts.Dtos;

public class ProductsListDto : PagedResponse<ShortProductDto>
{
    public required CategoryDto CategoryDto { get; set; }

    public ProductsSortEnum Sorting { get; set; }
}
