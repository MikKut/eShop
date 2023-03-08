using Basket.Host.Models.Dtos;
using Basket.Host.Services.Interfaces;

namespace Basket.Host.Services;

public class BasketService : IBasketService
{
    private readonly ICacheService _cacheService;
    private readonly IKeyGeneratorService _keyGeneratorService;

    public BasketService(
        ICacheService cacheService,
        IKeyGeneratorService keyGeneratorService)
    {
        _cacheService = cacheService;
        _keyGeneratorService=keyGeneratorService;
    }

    public async Task AddItems<T>(OrderDto<T> data)
    {
        string key = _keyGeneratorService.GenerateKey(data.User);
        await _cacheService.AddOrUpdateAsync(key, data.Orders);
    }

    public async Task<BasketDto<CatalogItemDto>> GetItems(UserDto user)
    {
        string key = _keyGeneratorService.GenerateKey(user);
        List<CatalogItemDto> result = await _cacheService.GetAsync<List<CatalogItemDto>>(key);
        result ??= new List<CatalogItemDto>();

        return new BasketDto<CatalogItemDto>() { Data = result };
    }

    public async Task<BasketDto<CatalogItemDto>> GetItems(int userId, string userName)
    {
        string key = _keyGeneratorService.GenerateKey(new UserDto() { UserId = userId, UserName = userName });
        List<CatalogItemDto> result = await _cacheService.GetAsync<List<CatalogItemDto>>(key);
        result ??= new List<CatalogItemDto>();

        return new BasketDto<CatalogItemDto>() { Data = result };
    }
}