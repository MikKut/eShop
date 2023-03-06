using AutoMapper;
using MVC.Models.Dto;
using MVC.Services.Interfaces;
using MVC.ViewModels.Models;
using MVC.ViewModels.Models.CatalogBasketItem;

namespace MVC.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly ILogger<BasketController> _logger;
        private readonly IMapper _mapper;

        public BasketController(
            IBasketService basketService,
            IMapper mapper,
            ILogger<BasketController> logger)
        {
            _logger = logger;
            _basketService = basketService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var userDto = GetUserDto();
            if (userDto == null)
            {
                _logger.LogError("User dto is null");
                return View("Error");
            }
            var basketListOfItems = new BasketListOfItems() { CatalogItems = await _basketService.GetGroupedBasketItems(userDto) };
            return View(basketListOfItems);
        }
        public async Task<IActionResult> RemoveFromBasket(CatalogItemDto order)
        {
            var userDto = GetUserDto();
            if (userDto == null)
            {
                _logger.LogError("User dto is null");
                return View("Error");
            }

            var isSuccessfulResultResponse = await _basketService.RemoveFromBasket(new OrderItemDto() { User = userDto, Item = order });
            _logger.LogInformation($"Result of removing {order.TypeName} \"{order.Name}\" of {order.BrandName} to bakset is {isSuccessfulResultResponse}");
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> AddToBasket(CatalogItemDto order)
        {
            var userDto = GetUserDto();
            if (userDto == null)
            {
                _logger.LogError("User dto is null");
                return View("Error");
            }

            var isSuccessfulResultResponse = await _basketService.AddToBasket(new OrderItemDto() { User = userDto, Item = order });
            _logger.LogInformation($"Result of adding {order.TypeName} \"{order.Name}\" of {order.BrandName} to basket is {isSuccessfulResultResponse}");
            return RedirectToAction("Index", "Catalog");
        }
        public async Task<IActionResult> CommitPurchases()
        {
            var userDto = GetUserDto();
            if (userDto == null)
            {
                return View("Error");
            }

            var isSuccessfulResultResponse = _basketService.CommitPurchases(userDto);
            _logger.LogInformation($"Result of commiting purchases of {userDto.UserId} \"{userDto.UserName}\" is {isSuccessfulResultResponse}");
            return View(_basketService.CommitPurchases(userDto));
        }
        private UserDto? GetUserDto()
        {
            _logger.LogWarning($"User Is Authenticated: {User.Identity.IsAuthenticated}");
            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }

            int id;
            if (!Int32.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out id))
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