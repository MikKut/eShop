using Basket.Host.Models.Items;
using System.ComponentModel.DataAnnotations;

namespace Basket.Host.Models.Requests
{
    public class AddItemsRequest
    {
        [Required]
        public IEnumerable<CatalogItem> Data { get; set; } = null!;
    }
}
