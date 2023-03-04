using AutoMapper;
using MVC.Models.Dto;
using MVC.Services.Interfaces;
using MVC.ViewModels.BasketViewModels;
using MVC.ViewModels.Models;

namespace MVC.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IIdentityParser<ApplicationUser> _identityParser;
        private readonly ILogger<BasketController> _logger;
        private readonly IMapper _mapper;

        public BasketController(
            IBasketService basketService,
            IIdentityParser<ApplicationUser> identityParser,
            IMapper mapper)
        {
            _basketService = basketService;
            _identityParser = identityParser;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var basketViewModel = new BasketIndexViewModel()
            {
                CatalogBasketItems = await _basketService.GetBasketItems(),
                Total = 12
            };

            return View(basketViewModel);
        }
        public async Task<IActionResult> RemoveFromBasket(OrderDto order)
        {
            var user = GetUser();
            var isSuccessfulResultResponse = await _basketService.RemoveFromBasket(new OrderDto() { User = _mapper.Map<UserDto>(user), Item = order.Item });
            _logger.LogInformation($"Result of removing {order.Item.} \"{order.Item.Name}\" of {order.Item.CatalogBrand.Brand} to bakset is {isSuccessfulResultResponse}");
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> AddToBasket(OrderDto order)
        {
            var user = GetUser();
            var isSuccessfulResultResponse = await _basketService.AddToBasket(new OrderDto() { User = _mapper.Map<UserDto>(user), Item = order.Item});
            _logger.LogInformation($"Result of adding {order.Item.CatalogType.Type} \"{order.Item.Name}\" of {order.Item.CatalogBrand.Brand} to basket is {isSuccessfulResultResponse}");
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> CommitPurchases()
        {
            var user = GetUser();
            var isSuccessfulResultResponse = _basketService.CommitPurchases(_mapper.Map<UserDto>(user));
            _logger.LogInformation($"Result of commiting purchases of {user.Id} \"{user.Name}\" is {isSuccessfulResultResponse}");
            return View(_basketService.CommitPurchases());
        }
        private ApplicationUser GetUser()
        {
            return _identityParser.Parse(User);
        }
    }
}
