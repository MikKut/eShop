using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Order.Host.Models.Dtos;
using Order.Host.Models.Requests;
using Order.Host.Services.Interfaces;
using Order.Host.Models.Responses;
using System.Net;

namespace Order.Host.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
    [Scope("order.makeorder")]
    [Route(ComponentDefaults.DefaultRoute)]
    public class OrderBffController
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
        public async Task<SuccessfulResultResponse> CommitPurchases(PurchaseRequest<CatalogItemDto> request)
        {
            return await _orderService.HandlePurchase(request);
        }
    }
}
