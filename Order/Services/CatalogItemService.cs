using Infrastructure.Services.Interfaces;
using Order.Host.Models.Dtos;
using Order.Host.Models.Responses;
using Order.Host.Services.Interfaces;

namespace Order.Host.Services
{
    public class CatalogItemService : ICatalogItemService
    {
        private readonly IInternalHttpClientService _httpClientService;
        public CatalogItemService(IInternalHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        public Task<SuccessfulResultResponse> ReduceQuantityOfItems(IEnumerable<CatalogItemDto> Data)
        {
            var groupedData = Data.GroupBy(x => x.Id);
            throw new NotImplementedException();
        }

        private Task<> CheckForAvailability(Dictionary<int, CatalogItemDto> items)
        {
            var catalogItems = new List<CatalogItemDto>();
            foreach (var item in items)
            {
                _httpClientService.SendAsync<CatalogItemResponse,>()
                item.Key
            }
        }
    }
}
