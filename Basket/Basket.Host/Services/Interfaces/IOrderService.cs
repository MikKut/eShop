using Basket.Host.Models.Dtos;
using Basket.Host.Models.Responses;
using Infrastructure.Models.Responses;

namespace Basket.Host.Services.Interfaces
{
    public interface IOrderService<T>
        where T : class
    {
        Task<SuccessfulResultResponse> CommitPurchases(OrderDto<T> request);
    }
}
