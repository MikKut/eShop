using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Order.Host.Services.Interfaces;
using Order.Models.Responses;
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
        private readonly IOrderService _orderService;
        private readonly IOptions<AppSettings> _config;

        public OrderBffController(
            ILogger<OrderBffController> logger,
            IOrderService catalogService,
            IOptions<AppSettings> config)
        {
            _logger = logger;
            _orderService = catalogService;
            _config = config;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SuccessfulResultResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CommitPurchases()
        {

            return Ok(result);
        }
    }
}
