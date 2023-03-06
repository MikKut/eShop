using Order.Host.Models.Dtos;
using Order.Host.Models.Responses;

namespace Order.Host.Services.Interfaces
{
    public interface ICatalogItemService
    {
        Task<SuccessfulResultResponse> ReduceQuantityOfItems(IEnumerable<CatalogItemDto> Data);
    }
}
