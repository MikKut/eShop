using Basket.Host.Models;
using Basket.Host.Models.Dtos;
using Basket.Host.Models.Items;
using Basket.Host.Models.Requests;
using Basket.Host.Models.Responses;
using Basket.Host.Services.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using MVC.Models.Dto;
using MVC.Models.Responses;

namespace Basket.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
[Scope("catalog.catalogbrand")]
[Route(ComponentDefaults.DefaultRoute)]
public class BasketBffController : ControllerBase
{
    private readonly ILogger<BasketBffController> _logger;
    private readonly IBasketService _basketService;
    private readonly IOrderService<CatalogItem> _orderService;
    private readonly IKeyGeneratorService _keyGeneratorService;

    public BasketBffController(
        ILogger<BasketBffController> logger,
        IBasketService basketService,
        IOrderService<CatalogItem> orderService)
    {
        _orderService = orderService;
        _logger = logger;
        _basketService = basketService;
    }
    
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> AddItemsToBasket(OrderDto<CatalogItem> request)
    {
        await _basketService.AddItems(request);
        return Ok();
    }

    [HttpPost]
    [ProducesResponseType(typeof(GetBasketResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetBasketItems(UserDto user)
    {
        var basketId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        var response = await _basketService.GetItems(user);
        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SuccessfulResultResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CommitPurchases(UserDto user)
    {
        var response = await _basketService.GetItems(user);
        var result = await _orderService.CommitPurchases(response);
        return Ok(result);
    }
}