using AutoMapper;
using MVC.Models.Dto;
using MVC.Models.Responses;
using MVC.Services.Interfaces;
using MVC.ViewModels;
using MVC.ViewModels.Models;
using System.Linq;

namespace MVC.Services
{
    public class BasketService : IBasketService
    {
        private readonly IOptions<AppSettings> _settings;
        private readonly IHttpClientService _httpClient;
        public BasketService(
            IOptions<AppSettings> settings,
            IHttpClientService httpClient)
        {
            _settings = settings;
            _httpClient = httpClient;
        }

        public async Task<SuccessfulResultResponse> AddToBasket(OrderItemDto order)
        {
            var items = await GetBasketItems(order.User);
            items = items.Concat(new[] { order.Item });

            var result = await _httpClient.SendAsync<SuccessfulResultResponse, OrderDto>
                ($"{_settings.Value.BasketUrl}/AddItemsToBasket",
                HttpMethod.Post, new OrderDto { User = order.User, Items = items});
            return result;
        }

        public async Task<SuccessfulResultResponse> RemoveFromBasket(OrderItemDto order)
        {
            var items = await GetBasketItems(order.User);
            if (items.FirstOrDefault(order.Item) == null)
            {
                return new SuccessfulResultResponse() { IsSuccessful = false, Message = "There is no such item in bucket" };
            }

            var itemsList = items.ToList();
            itemsList.Remove(order.Item);
            items = itemsList;
            var result = await _httpClient.SendAsync<SuccessfulResultResponse, OrderDto>
                ($"{_settings.Value.BasketUrl}/AddItemsToBasket",
                HttpMethod.Post, new OrderDto { User = order.User, Items = items });
            return result;
        }

        public async Task<SuccessfulResultResponse> CommitPurchases(UserDto user)
        {
            var result = await _httpClient.SendAsync<SuccessfulResultResponse, UserDto>
                  ($"{_settings.Value.BasketUrl}/CommitPurchases",
                  HttpMethod.Post, user);
            return result;
        }

        public async Task<Dictionary<CatalogItemDto, int>> GetGroupedBasketItems(UserDto user)
        {
            var backetItems = await GetBasketItems(user);
            Dictionary<CatalogItemDto, int> backetItemsDictionary = new();
            foreach(var item in backetItems)
            {
                if (!backetItemsDictionary.ContainsKey(item))
                {
                    backetItemsDictionary.Add(item, 1);
                }
                else
                {
                    backetItemsDictionary[item]++;
                }
            }

            return backetItemsDictionary;
        }

        private async Task<IEnumerable<CatalogItemDto>> GetBasketItems(UserDto user)
        {
            var result = await _httpClient.SendAsync<GroupedItems<CatalogItemDto>, UserDto>
                ($"{_settings.Value.BasketUrl}/GetBasketItems",
                HttpMethod.Post, user);
            return result.Data;
        }
    }
}
