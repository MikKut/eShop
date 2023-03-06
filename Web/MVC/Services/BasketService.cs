using AutoMapper;
using MVC.Controllers;
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
        private readonly ILogger<BasketController> _logger;
        public BasketService(
            IOptions<AppSettings> settings,
            IHttpClientService httpClient,
            ILogger<BasketController> logger)
        {
            _logger = logger;
            _settings = settings;
            _httpClient = httpClient;
        }

        public async Task<SuccessfulResultResponse> AddToBasket(OrderItemDto order)
        {
            var items = await GetBasketItems(order.User);
            _logger.LogInformation($"Before adding from basket: items are null:{items is null}");
            _logger.LogWarning($"- {items.Count()}");
            items = items.Concat(new[] { order.Item });
            _logger.LogInformation($"After adding to basket: items are null:{items is null}");
            _logger.LogWarning($"- {items.Count()}");
            var result = await _httpClient.SendAsync<SuccessfulResultResponse, OrderDto<CatalogItemDto>>
                ($"{_settings.Value.BasketUrl}/AddItemsToBasket",
                HttpMethod.Post, new OrderDto<CatalogItemDto> { User = order.User, Orders = items});
            _logger.LogInformation($"After add-request to basket");

            return result;
        }

        public async Task<SuccessfulResultResponse> RemoveFromBasket(OrderItemDto order)
        {
            var items = await GetBasketItems(order.User);
            if (items.FirstOrDefault(order.Item) == null)
            {
                return new SuccessfulResultResponse() { IsSuccessful = false, Message = "There is no such item in bucket" };
            }

            _logger.LogInformation($"Before removing from basket: items are null:{items is null}");
            _logger.LogWarning($"- {items.Count()}");
            var itemsList = items.ToList();
            itemsList.Remove(order.Item);
            items = itemsList;
            _logger.LogInformation($"After removing from basket: items are null:{items is null}");
            _logger.LogWarning($"- {items.Count()}");
            var result = await _httpClient.SendAsync<SuccessfulResultResponse, OrderDto<CatalogItemDto>>
                ($"{_settings.Value.BasketUrl}/AddItemsToBasket",
                HttpMethod.Post, new OrderDto<CatalogItemDto> { User = order.User, Orders = items });
            _logger.LogInformation($"After delete-request to basket");
            return result;
        }

        public async Task<SuccessfulResultResponse> CommitPurchases(UserDto user)
        {
            _logger.LogInformation($"Before commiting purchase");
            var result = await _httpClient.SendAsync<SuccessfulResultResponse, UserDto>
                  ($"{_settings.Value.BasketUrl}/CommitPurchases",
                  HttpMethod.Post, user);
            _logger.LogInformation($"After commiting purchase");
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
            _logger.LogInformation($"Before getting from basket");
            var result = await _httpClient.SendAsync<GroupedItems<CatalogItemDto>, UserDto>
                ($"{_settings.Value.BasketUrl}/GetBasketItems",
                HttpMethod.Post, user);
            _logger.LogInformation($"After getting from basket: items are null:{result is null}");
            _logger.LogWarning($"- {result.Data.Count()}");
            return result.Data;
        }
    }
}
