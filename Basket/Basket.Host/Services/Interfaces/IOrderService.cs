using Basket.Host.Models.Dtos;
using Basket.Host.Models.Requests;
using MVC.Models.Responses;

namespace Basket.Host.Services.Interfaces
{
    public interface IOrderService<T>
        where T : class
    {
        Task<SuccessfulResultResponse> CommitPurchases(BasketDto<T> request);
    }
}
