using Order.Host.Models.Requests;
using Order.Host.Models.Responses;

namespace Order.Host.Services.Interfaces
{
    public interface IOrderService<T>
        where T : class
    {
        public Task<SuccessfulResultResponse> HandlePurchase(PurchaseRequest<T> request);
    }
}
