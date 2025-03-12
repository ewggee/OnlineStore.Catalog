using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.Catalog.Contracts.Common;

/// <summary>
/// Пагинированный запрос.
/// </summary>
public class PagedRequest
{
    /// <summary>
    /// Номер страницы.
    /// </summary>
    [FromQuery]
    public int Page { get; set; } = 1;
}
