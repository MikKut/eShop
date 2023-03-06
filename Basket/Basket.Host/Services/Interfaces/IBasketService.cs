using Basket.Host.Models;
using Basket.Host.Models.Dtos;
using Basket.Host.Models.Requests;
using MVC.Models.Dto;

namespace Basket.Host.Services.Interfaces;

public interface IBasketService
{
    Task AddItems<T>(OrderDto<T> data);
    Task<BasketDto<CatalogItemDto>> GetItems(UserDto user);
}