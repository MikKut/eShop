using Basket.Host.Models;
using Basket.Host.Models.Dtos;
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
    private readonly IOrderService<CatalogItemDto> _orderService;

    public BasketBffController(
        ILogger<BasketBffController> logger,
        IBasketService basketService,
        IOrderService<CatalogItemDto> orderService)
    {
        _orderService = orderService;
        _logger = logger;
        _basketService = basketService;
    }
    
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> AddItemsToBasket(OrderDto<CatalogItemDto> request)
    {
        try
        {
            await _basketService.AddItems(request);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(GetBasketResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetBasketItems(UserDto user)
    {
        try
        {
            var response = await _basketService.GetItems(user);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(SuccessfulResultResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CommitPurchases(UserDto user)
    {
        try
        {
            var response = await _basketService.GetItems(user);
            var result = await _orderService.CommitPurchases(new OrderDto<CatalogItemDto>() { Orders = response.Data, User = user });
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }
}