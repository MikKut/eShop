using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Options;
using Order.Host.Models.Dtos;
using Order.Host.Models.Requests;
using Order.Host.Models.Responses;
using Order.Host.Services.Interfaces;

namespace Order.Host.Services
{
    public class CatalogItemService : ICatalogItemService
    {
        private readonly IInternalHttpClientService _httpClientService;
        private readonly IOptions<AppSettings> _settings;
        public CatalogItemService(
            IInternalHttpClientService httpClientService,
            IOptions<AppSettings> settings)
        {
            _settings = settings;
            _httpClientService = httpClientService;
        }

        public async Task<SuccessfulResultResponse> ReduceQuantityOfItemsAsync(IEnumerable<CatalogItemDto> data)
        {
            Dictionary<int, int> catalogItems = await GetGroupedItems(data);
            List<CatalogItemResponse> listOfResponses = await GetCatalogItemResponsesAsync(catalogItems);

            return !CheckForAvailability(listOfResponses, catalogItems)
                ? new SuccessfulResultResponse() { IsCompletedSuccessfully = false, Message = "There are to many items in the basket: not enough items in the shop" }
                : await CommitReducing(listOfResponses.ToArray(), catalogItems);
        }
        private async Task<List<CatalogItemResponse>> GetCatalogItemResponsesAsync(Dictionary<int, int> catalogItems)
        {
            List<CatalogItemResponse> listOfResponses = new();
            foreach (KeyValuePair<int, int> item in catalogItems)
            {
                string url = $"{_settings.Value.CatalogBffUrl}/GetById";
                CatalogItemResponse result = await _httpClientService.SendAsync<CatalogItemResponse, GetByIdRequest>
                    (url,
                    HttpMethod.Post, new GetByIdRequest { ID = item.Key });
                listOfResponses.Add(result);
            }

            return listOfResponses;
        }

        private async Task<SuccessfulResultResponse> CommitReducing(CatalogItemResponse[] listOfResponses, Dictionary<int, int> catalogItems)
        {
            int count = 0;
            foreach (KeyValuePair<int, int> item in catalogItems)
            {
                string url = $"{_settings.Value.CatalogItemUrl}/UpdateAvailableStock";
                SuccessfulResultResponse result = await _httpClientService.SendAsync<SuccessfulResultResponse, UpdateAvailableStockRequest>
                    (url,
                    HttpMethod.Post, new UpdateAvailableStockRequest { Id = item.Key, AvailableStock = listOfResponses[count].AvailableStock - catalogItems[listOfResponses[count].Id] });
                count++;
                if (result == null || !result.IsCompletedSuccessfully)
                {
                    return new SuccessfulResultResponse { IsCompletedSuccessfully = false, Message = "Cannot aommit reducing available stock" };
                }
            }

            return new SuccessfulResultResponse { IsCompletedSuccessfully = true };
        }

        private bool CheckForAvailability(List<CatalogItemResponse> listOfResponses, Dictionary<int, int> catalogItems)
        {
            foreach (CatalogItemResponse item in listOfResponses)
            {
                if (item == null)
                {
                    return false;
                }

                if (!catalogItems.ContainsKey(item.Id))
                {
                    return false;
                }

                if (item.AvailableStock < catalogItems[item.Id])
                {
                    return false;
                }
            }

            return true;
        }

        private Task<Dictionary<int, int>> GetGroupedItems(IEnumerable<CatalogItemDto> data)
        {
            Dictionary<int, int> backetItemsDictionary = new();
            foreach (CatalogItemDto item in data)
            {
                if (!backetItemsDictionary.ContainsKey(item.Id))
                {
                    backetItemsDictionary.Add(item.Id, 1);
                }
                else
                {
                    backetItemsDictionary[item.Id]++;
                }
            }

            return Task.FromResult(backetItemsDictionary);
        }
    }
}
