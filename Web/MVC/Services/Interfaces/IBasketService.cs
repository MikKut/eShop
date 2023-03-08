using MVC.Models.Dto;
using MVC.Models.Responses;

namespace MVC.Services.Interfaces
{
    public interface IBasketService
    {
        Task<SuccessfulResultResponse> AddToBasket(OrderItemDto order);
        Task<SuccessfulResultResponse> RemoveFromBasket(OrderItemDto order);
        Task<SuccessfulResultResponse> CommitPurchases(UserDto user);
        Task<Dictionary<CatalogItemDto, int>> GetGroupedBasketItems(UserDto user);
    }
}
