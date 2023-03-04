using MVC.Models.Dto;
using MVC.Models.Responses;
using MVC.ViewModels.Models;

namespace MVC.Services.Interfaces
{
    public interface IBasketService
    {
        Task<SuccessfulResultResponse> AddToBasket(OrderDto order);
        Task<SuccessfulResultResponse> RemoveFromBasket(OrderDto order);
        Task<SuccessfulResultResponse> CommitPurchases();
        Task<IEnumerable<CatalogBasketItem>> GetBasketItems();
    }
}
