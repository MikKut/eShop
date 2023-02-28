using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests;

public class PaginatedItemsRequest
{
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    public int PageIndex { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    public int PageSize { get; set; }
}