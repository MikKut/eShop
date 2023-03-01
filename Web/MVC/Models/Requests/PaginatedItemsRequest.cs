namespace MVC.Dtos;

public class PaginatedItemsRequest<T>
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    public int PageIndex { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    public int PageSize { get; set; }
}