using Basket.Host.Models;
using Basket.Host.Models.Dtos;
using Basket.Host.Models.Items;
using Basket.Host.Models.Requests;
using Basket.Host.Services.Interfaces;
using MVC.Models.Dto;

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
        var key = _keyGeneratorService.GenerateKey(data.User);
        await _cacheService.AddOrUpdateAsync(key, data);
    }

    public async Task<BasketDto<CatalogItem>> GetItems(UserDto user)
    {
        var key = _keyGeneratorService.GenerateKey(user);
        var result = await _cacheService.GetAsync<List<CatalogItem>>(key);
        if (result == null)
        {
            result = new List<CatalogItem>();
        }
        return new BasketDto<CatalogItem>() { Data = result };
    }
}