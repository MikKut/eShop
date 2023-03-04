using Basket.Host.Models.Items;

namespace Basket.Host.Models.Responses
{
    public class GetBasketResponse
    {
        public IEnumerable<CatalogItem> Data { get; set; } = null!;
    }
}
