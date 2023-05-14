using AutoMapper;
using MVC.Controllers;
using MVC.Models.Dto;
using MVC.Services.Interfaces;
using MVC.ViewModels;
using MVC.EqualityComparers;
using Infrastructure.Models.Responses;

namespace MVC.Services
{
    public class BasketService : IBasketService
    {
        private readonly IOptions<AppSettings> _settings;
        private readonly IHttpClientService _httpClient;
        private readonly ILogger<BasketController> _logger;
        private readonly ICatalogService _catalogService;
        private readonly IMapper _mapper;
        public BasketService(
            IOptions<AppSettings> settings,
            IHttpClientService httpClient,
            ILogger<BasketController> logger,
            ICatalogService catalogService,
            IMapper mapper)
        {
            _mapper = mapper;
            _catalogService = catalogService;
            _logger = logger;
            _settings = settings;
            _httpClient = httpClient;
        }

        public async Task<SuccessfulResultResponse> AddToBasket(OrderItemDto order)
        {
            IEnumerable<CatalogItemDto>? items = await GetBasketItems(order.User);
            _logger.LogInformation($"Before adding to basket: items are null:{items is null}");
            _logger.LogWarning($"- {items!.Count()}");
            CheckItemsForNull(items);
            var itemToAdd = _mapper.Map<CatalogItemDto>(await _catalogService.GetCatalogItemById(order.ItemId)!);
            _logger.LogInformation($"item to add is null: {itemToAdd is null}. Order id is: {order.ItemId}");
            items = items!.Concat(new[] { itemToAdd });
            _logger.LogInformation($"After adding to basket: items are null:{items is null}");
            _logger.LogWarning($"- {items!.Count()}");
            SuccessfulResultResponse result = await _httpClient.SendAsync<SuccessfulResultResponse, OrderDto<CatalogItemDto>>
                ($"{_settings.Value.BasketUrl}/AddItemsToBasket",
                HttpMethod.Post, new OrderDto<CatalogItemDto> { User = order.User, Orders = items! });
            _logger.LogInformation($"After add-request to basket");

            return result;
        }

        public async Task<SuccessfulResultResponse> RemoveFromBasket(OrderItemDto order)
        {
            IEnumerable<CatalogItemDto>? items = await GetBasketItems(order.User);
            _logger.LogInformation($"Before removing from basket: items are null:{items is null}");
            _logger.LogWarning($"- {items.Count()}");
            CheckItemsForNull(items);
            List<CatalogItemDto> itemsList = items.ToList();
            var itemToDelete = await _catalogService.GetCatalogItemById(order.ItemId);
            if (itemToDelete == null)
            {
                return new SuccessfulResultResponse() { IsSuccessful = false, ErrorMessage = "There is no such item" };
            }

            bool removeResult = itemsList.Remove(_mapper.Map<CatalogItemDto>(itemToDelete!));
            _logger.LogInformation($"Removing is successful: {removeResult}");
            items = itemsList;
            _logger.LogInformation($"After removing from basket: items are null:{items is null}");
            _logger.LogWarning($"- {items.Count()}");
            SuccessfulResultResponse result = await _httpClient.SendAsync<SuccessfulResultResponse, OrderDto<CatalogItemDto>>
                ($"{_settings.Value.BasketUrl}/AddItemsToBasket",
                HttpMethod.Post, new OrderDto<CatalogItemDto> { User = order.User, Orders = items! });
            _logger.LogInformation($"After delete-request to basket");
            return result;
        }

        public async Task<SuccessfulResultResponse> CommitPurchases(UserDto user)
        {
            _logger.LogInformation($"Before commiting purchase");
            SuccessfulResultResponse result = await _httpClient.SendAsync<SuccessfulResultResponse, UserDto>
                  ($"{_settings.Value.BasketUrl}/CommitPurchases",
                  HttpMethod.Post, user);
            _logger.LogInformation($"After commiting purchase");
            return result;
        }

        public async Task<Dictionary<CatalogItemDto, int>> GetGroupedBasketItems(UserDto user)
        {
            IEnumerable<CatalogItemDto> backetItems = await GetBasketItems(user);
            _logger.LogInformation($"in grouped recieved {backetItems.Count()} items.");
            Dictionary<CatalogItemDto, int> backetItemsDictionary = new(new CatalogItemEqualityComparer());
            foreach (CatalogItemDto item in backetItems)
            {
                _logger.LogInformation($"item is null: {item is null}");
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
            GroupedItems<CatalogItemDto>? result = await _httpClient.SendAsync<GroupedItems<CatalogItemDto>, UserDto>
                ($"{_settings.Value.BasketUrl}/GetBasketItems",
                HttpMethod.Post, user);
            _logger.LogInformation($"After getting from basket: items are null:{result is null}");
            _logger.LogWarning($"- {result!.Data.Count()}");
            return result.Data;
        }

        private Task CheckItemsForNull(IEnumerable<CatalogItemDto> items)
        {
            _logger.LogInformation($"{items is null}");
            foreach (var item in items)
            {
                _logger.LogInformation($"item is null: {item is null}");
                _logger.LogError($"Id: {item.Id}");
            }
            return Task.CompletedTask;
        }
    }
}
