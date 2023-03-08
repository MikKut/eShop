using System.Net;
using AutoMapper;
using MVC.Models.Dto;
using MVC.Services.Interfaces;
using MVC.ViewModels.Models.CatalogBasketItem;

namespace MVC.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly ILogger<BasketController> _logger;

        public BasketController(
            IBasketService basketService,
            IMapper mapper,
            ILogger<BasketController> logger)
        {
            _logger = logger;
            _basketService = basketService;
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(Task<IActionResult>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Index()
        {
            UserDto? userDto = GetUserDto();
            if (userDto == null)
            {
                _logger.LogError("User dto is null");
                return View("Error");
            }


            BasketListOfItems basketListOfItems = new() { CatalogItems = await _basketService.GetGroupedBasketItems(userDto) };
            _logger.LogWarning($"inde basketlistofitem is null: {basketListOfItems.CatalogItems is null}");
            return View(basketListOfItems);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromBasket(int id)
        {            
            UserDto? userDto = GetUserDto();
            if (userDto == null)
            {
                _logger.LogError("User dto is null");
                return View("Error");
            }

            if (id == 0)
            {
                _logger.LogError("User id is 0");
                return View("Error");
            }

            _logger.LogInformation($"Id removing: {id}");
            Models.Responses.SuccessfulResultResponse isSuccessfulResultResponse = await _basketService.RemoveFromBasket(new OrderItemDto() { User = userDto, ItemId = id });
            // _logger.LogInformation($"Result of removing from bakset is {isSuccessfulResultResponse.IsSuccessful}:{isSuccessfulResultResponse?.Message}");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddToBasket(int id)
        {
            UserDto? userDto = GetUserDto();
            if (userDto == null)
            {
                _logger.LogError("User dto is null");
                return View("Error");
            }
            if (id == 0)
            {
                _logger.LogError("User id is 0");
                return View("Error");
            }

            _logger.LogInformation($"Id adding: {id}");
            Models.Responses.SuccessfulResultResponse isSuccessfulResultResponse = await _basketService.AddToBasket(new OrderItemDto() { User = userDto, ItemId = id });
            // _logger.LogInformation($"Result of adding to bakset is {isSuccessfulResultResponse.IsSuccessful}:{isSuccessfulResultResponse?.Message}");
            return RedirectToAction("Index", "Catalog");
        }

        [HttpPost]
        public async Task<IActionResult> CommitPurchases()
        {
            UserDto? userDto = GetUserDto();
            if (userDto == null)
            {
                return View("Error");
            }

            Models.Responses.SuccessfulResultResponse isSuccessfulResultResponse = await _basketService.CommitPurchases(userDto);
            _logger.LogInformation($"Result of commiting purchases of {userDto.UserId} \"{userDto.UserName}\" is {isSuccessfulResultResponse}");
            return View("PurchaseResult", await _basketService.CommitPurchases(userDto));
        }

        private UserDto? GetUserDto()
        {
            _logger.LogWarning($"User Is Authenticated: {User.Identity!.IsAuthenticated}");
            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }

            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int id))
            {
                _logger.LogWarning("Parsing is not successful");
            }

            _logger.LogInformation($"Id = {id}");
            string name = User.Claims.FirstOrDefault(x => x.Type == "name")!.Value;
            _logger.LogInformation($"Name = {name}");
            return new UserDto { UserId = id, UserName = name! };
        }
    }
}