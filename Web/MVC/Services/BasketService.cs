using AutoMapper;
using MVC.Models.Dto;
using MVC.Models.Requests;
using MVC.Models.Responses;
using MVC.Services.Interfaces;
using MVC.ViewModels;
using MVC.ViewModels.Models;

namespace MVC.Services
{
    public class BasketService : IBasketService
    {
        private readonly IMapper _mapper;
        private readonly IOptions<AppSettings> _settings;
        private readonly IHttpClientService _httpClient;
        private readonly ILogger<BasketService> _logger;
        public BasketService(
            IMapper mapper,
            IOptions<AppSettings> settings,
            IHttpClientService httpClient,
            ILogger<BasketService> logger)
        {
            _mapper = mapper;
            _settings = settings;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<SuccessfulResultResponse> AddToBasket(OrderDto order)
        {
            var items = await GetBasketItems(order.User);
            var list = items.ToList();
            list.Add(_mapper.Map<CatalogBasketItem>(order.Item));

            var result = await _httpClient.SendAsync<SuccessfulResultResponse, OrderDto>
                ($"{_settings.Value.BasketUrl}/AddItemsToBasket",
                HttpMethod.Post, order);
            return result;
        }

        public async Task<SuccessfulResultResponse> RemoveFromBasket(OrderDto order)
        {
            var items = await GetBasketItems(order.User);
            var list = items.ToList();
            list.Remove(_mapper.Map<CatalogBasketItem>(order.Item));

            var result = await _httpClient.SendAsync<SuccessfulResultResponse, OrderDto>
                ($"{_settings.Value.BasketUrl}/AddItemsToBasket",
                HttpMethod.Post, order);
            return result;
        }

        public async Task<IEnumerable<CatalogBasketItem>> GetBasketItems(UserDto user)
        {
            var result = await _httpClient.SendAsync<GroupedItems<CatalogBasketItem>, UserDto>
                ($"{_settings.Value.BasketUrl}/GetBasketItems",
                HttpMethod.Post, user);
            return result.Data;
        }
        public async Task<SuccessfulResultResponse> CommitPurchases(UserDto user)
        {
            var result = await _httpClient.SendAsync<SuccessfulResultResponse, UserDto>
                  ($"{_settings.Value.BasketUrl}/CommitPurchases",
                  HttpMethod.Post, user);
            return result;
        }
    }
}
