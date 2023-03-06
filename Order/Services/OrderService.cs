using Order.Host.Models.Dtos;
using Order.Host.Models.Requests;
using Order.Host.Models.Responses;
using Order.Host.Services.Interfaces;
using System.Net.Http;
using System.Runtime;

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
            var totalCost = request.Data.Sum(x => x.Price);
            var resultOfCommit = await _paymentService.CheckTrasactionForAvailabilityForUser(request.ID, totalCost);
            if (!resultOfCommit.IsCompletedSuccessfully)
            {
                return resultOfCommit;
            }

            resultOfCommit = await _catalogItemService.ReduceQuantityOfItemsAsync(request.Data);
            if (!resultOfCommit.IsCompletedSuccessfully)
            {
                return resultOfCommit;
            }

            resultOfCommit = await _paymentService.CommitTrasactionForTheUser(request.ID, totalCost);
            return resultOfCommit;
        }
    }
}
