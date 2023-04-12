using Infrastructure;
using Infrastructure.Filters;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Order.Host.Models.Dtos;
using Order.Host.Models.Requests;
using Order.Host.Models.Responses;
using Order.Host.Services.Interfaces;
using System.Net;

namespace Order.Host.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowClientPolicy)]
    [Scope("order.makeorder")]
    [Route(ComponentDefaults.DefaultRoute)]
    public class OrderBffController : ControllerBase
    {
        private readonly ILogger<OrderBffController> _logger;
        private readonly IOrderService<CatalogItemDto> _orderService;
        private readonly IOptions<AppSettings> _config;

        public OrderBffController(
            ILogger<OrderBffController> logger,
            IOrderService<CatalogItemDto> catalogService,
            IOptions<AppSettings> config)
        {
            _logger = logger;
            _orderService = catalogService;
            _config = config;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SuccessfulResultResponse), (int)HttpStatusCode.OK)]
        [ServiceFilter(typeof(LogActionFilterAttribute<OrderBffController>))]
        public async Task<SuccessfulResultResponse> CommitPurchases(PurchaseRequest<CatalogItemDto> request)
        {
            _logger.LogInformation($"Recieved request: User id - {request.ID}, Data count - {request.Data.Count()}");
            return await _orderService.HandlePurchase(request);
        }
    }
}
