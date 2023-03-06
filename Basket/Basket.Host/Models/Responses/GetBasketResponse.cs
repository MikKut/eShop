using Basket.Host.Models.Dtos;

namespace Basket.Host.Models.Responses
{
    public class GetBasketResponse
    {
        public IEnumerable<CatalogItemDto> Data { get; set; } = null!;
    }
}
