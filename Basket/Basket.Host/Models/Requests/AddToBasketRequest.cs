using Basket.Host.Models.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Basket.Host.Models.Requests
{
    public class AddItemsRequest
    {
        [Required]
        public IEnumerable<CatalogItemDto> Data { get; set; } = null!;
    }
}
