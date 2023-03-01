using System.ComponentModel.DataAnnotations;
using Catalog.Host.Models.Enums;

namespace Catalog.Host.Models.Requests;

public class PaginatedItemsRequest<T>
    where T : struct, IConvertible
{
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    public int PageIndex { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    public int PageSize { get; set; }
    public Dictionary<CatalogTypeFilter, int>? Filters { get; set; }
}