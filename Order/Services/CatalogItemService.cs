using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Options;
using Order.Host.Models.Dtos;
using Order.Host.Models.Requests;
using Order.Host.Models.Responses;
using Order.Host.Services.Interfaces;
using System.Net.Http;
using System.Runtime;

namespace Order.Host.Services
{
    public class CatalogItemService : ICatalogItemService
    {
        private readonly IInternalHttpClientService _httpClientService;
        private readonly IOptions<AppSettings> _settings;
        public CatalogItemService(IInternalHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        public async Task<SuccessfulResultResponse> ReduceQuantityOfItemsAsync(IEnumerable<CatalogItemDto> data)
        {
            var catalogItems = await GetGroupedItems(data);
            var listOfResponses = await GetCatalogItemResponsesAsync(catalogItems);

            if (!CheckForAvailability(listOfResponses, catalogItems))
            {
                return new SuccessfulResultResponse() { IsCompletedSuccessfully = false, Message = "There are to many items in the basket: not enough items in the shop" };
            }

            return await CommitReducing(listOfResponses.ToArray(), catalogItems);
        }
        private async Task<List<CatalogItemResponse>> GetCatalogItemResponsesAsync(Dictionary<int, int> catalogItems)
        {
            var listOfResponses = new List<CatalogItemResponse>();
            foreach (var item in catalogItems)
            {
                string url = $"{_settings.Value.CatalogBffUrl}/GetById";
                var result = await _httpClientService.SendAsync<CatalogItemResponse, GetByIdRequest>
                    (url,
                    HttpMethod.Post, new GetByIdRequest { ID = item.Key });
                listOfResponses.Add(result);
            }

            return listOfResponses;
        }

        private async Task<SuccessfulResultResponse> CommitReducing(CatalogItemResponse[] listOfResponses, Dictionary<int, int> catalogItems)
        {
            int count = 0;
            foreach (var item in catalogItems)
            {
                string url = $"{_settings.Value.CatalogItemUrl}/UpdateAvailableStock";
                var result = await _httpClientService.SendAsync<SuccessfulResultResponse, UpdateAvailableStockRequest>
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
            foreach (var item in listOfResponses)
            {
                if (item.AvailableStock < catalogItems[item.Id])
                {
                    return false;
                }
            }

            return true;
        }

        private async Task<Dictionary<int, int>> GetGroupedItems(IEnumerable<CatalogItemDto> data)
        {
            Dictionary<int, int> backetItemsDictionary = new();
            foreach (var item in data)
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

            return backetItemsDictionary;
        }
    }
}
