using AutoMapper;
using Infrastructure.Filters;
using MVC.Models.Dto;
using MVC.Services.Interfaces;
using MVC.Models.Domains;
using MVC.ViewModels.CatalogViewModels;
using MVC.ViewModels.Pagination;
using System.Net;

namespace MVC.Controllers;

public class CatalogController : Controller
{
    private readonly ICatalogService _catalogService;
    private readonly ILogger<CatalogController> _logger;
    private readonly IMapper _mapper;

    public CatalogController(ICatalogService catalogService, ILogger<CatalogController> logger, IMapper mapper)
    {
        _logger = logger;
        _catalogService = catalogService;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [ServiceFilter(typeof(LogActionFilterAttribute<Controllers.CatalogController>))]
    [ProducesResponseType(typeof(Task<IActionResult>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Index(int? brandFilterApplied, int? typesFilterApplied, int? page, int? itemsPage)
    {
        page ??= 0;
        itemsPage ??= 6;
        _logger.LogInformation($"Before receiving catalog items. Page.Value: {page.Value}; itmes.Value: {itemsPage.Value}, brandFilterAplied: {brandFilterApplied}, typesFilterApplied: {typesFilterApplied}");
        Catalog? catalog = await _catalogService.GetCatalogItems(page.Value, itemsPage.Value, brandFilterApplied, typesFilterApplied);
        _logger.LogInformation($"After receiving catalog items. They are null: {catalog is null}");
        if (catalog == null)
        {
            return View("Error");
        }
        PaginationInfo info = new()
        {
            ActualPage = page.Value,
            ItemsPerPage = catalog.Data.Count,
            TotalItems = catalog.Count,
            TotalPages = (int)Math.Ceiling((decimal)catalog.Count / itemsPage.Value)
        };
        IndexViewModel vm = new()
        {
            CatalogItems = catalog.Data,
            Brands = await _catalogService.GetBrands(),
            Types = await _catalogService.GetTypes(),
            PaginationInfo = info
        };

        vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
        vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> AddItemToBucket(int id)
    {
        _logger.LogInformation($"Catalog add item entered with id {id}");
        return id <= 0
            ? RedirectToAction("Index", "Catalog")
            : (IActionResult)RedirectToAction("AddToBasket", "Basket", id);
    }
}