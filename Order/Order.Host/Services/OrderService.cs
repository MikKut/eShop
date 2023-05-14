using Infrastructure.Models.Responses;
using Order.Host.Models.Dtos;
using Order.Host.Models.Requests;
using Order.Host.Models.Responses;
using Order.Host.Services.Interfaces;

namespace Order.Host.Services
{
    public class OrderService
        : IOrderService<CatalogItemDto>
    {
        private readonly IPaymentService _paymentService;
        private readonly ICatalogItemService _catalogItemService;
        public OrderService(
            IPaymentService paymentService,
            ICatalogItemService catalogItemService)
        {
            _paymentService = paymentService;
            _catalogItemService = catalogItemService;
        }

        public async Task<SuccessfulResultResponse> HandlePurchase(PurchaseRequest<CatalogItemDto> request)
        {
            decimal totalCost = request.Data.Sum(x => x.Price);
            SuccessfulResultResponse resultOfCommit = await _paymentService.CheckTrasactionForAvailabilityForUser(request.ID, totalCost);
            if (!resultOfCommit.IsSuccessful)
            {
                return resultOfCommit;
            }

            resultOfCommit = await _catalogItemService.ReduceQuantityOfItemsAsync(request.Data);
            if (!resultOfCommit.IsSuccessful)
            {
                return resultOfCommit;
            }

            resultOfCommit = await _paymentService.CommitTrasactionForTheUser(request.ID, totalCost);
            return resultOfCommit;
        }
    }
}
