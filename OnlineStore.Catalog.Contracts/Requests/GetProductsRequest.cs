using Microsoft.AspNetCore.Mvc;
using OnlineStore.Catalog.Contracts.Common;
using OnlineStore.Catalog.Contracts.Enums;

namespace OnlineStore.Catalog.Contracts.Requests;

public sealed class GetProductsRequest : PagedRequest
{
    /// <summary>
    /// ID категории.
    /// </summary>
    [FromRoute(Name = "categoryId")]
    public int CategoryId { get; set; }

    /// <summary>
    /// Тип сортировки.
    /// </summary>
    [FromQuery]
    public ProductsSortEnum Sort { get; set; }
}
