using Order.Host.Models.Dtos;
using Order.Host.Models.Responses;

namespace Order.Host.Services.Interfaces
{
    public interface ICatalogItemService
    {
        Task<SuccessfulResultResponse> ReduceQuantityOfItemsAsync(IEnumerable<CatalogItemDto> data);
    }
}
