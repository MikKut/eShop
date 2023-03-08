using Basket.Host.Models.Dtos;

namespace Basket.Host.Services.Interfaces;

public interface IBasketService
{
    Task AddItems<T>(OrderDto<T> data);
    Task<BasketDto<CatalogItemDto>> GetItems(UserDto user);
    Task<BasketDto<CatalogItemDto>> GetItems(int userId, string userName);
}