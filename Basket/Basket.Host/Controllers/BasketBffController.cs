using Basket.Host.Models.Dtos;
using Basket.Host.Models.Responses;
using Basket.Host.Services.Interfaces;
using Infrastructure.Identity;
using Infrastructure.Models.Responses;
using Microsoft.AspNetCore.Authorization;

namespace Basket.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
[Scope("basket.basketbff")]
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
            _logger.LogInformation($"Request to add item:\nUser: {request.User.UserId}/{request.User.UserName},\nrequest orders are null: {request.Orders is null}");
            _logger.LogInformation($"request orders count: {request.Orders?.Count()}(may be null)");
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
            _logger.LogInformation($"Request to get item:\nUser: {user.UserId}/{user.UserName}");
            BasketDto<CatalogItemDto> response = await _basketService.GetItems(user.UserId, user.UserName);
            _logger.LogInformation($"Get data null:{response.Data is null}");
            _logger.LogInformation($"Get data count:{response.Data!.Count()}");
            return Ok(new GetBasketResponse() { Data = response.Data! });
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
            _logger.LogInformation($"Request to commit purchases item:\nUser: {user.UserId}/{user.UserName}");
            BasketDto<CatalogItemDto> response = await _basketService.GetItems(user.UserId, user.UserName);
            SuccessfulResultResponse result = await _orderService.CommitPurchases(new OrderDto<CatalogItemDto>() { Orders = response.Data, User = user });
            if (result.IsSuccessful)
            {
                await _basketService.CleanCurrentBasket(user);
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }
}