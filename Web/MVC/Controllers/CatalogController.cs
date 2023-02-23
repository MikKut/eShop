using Infrastructure.Filters;
using Infrastructure.Identity;
using MVC.Services.Interfaces;
using MVC.ViewModels.CatalogViewModels;
using MVC.ViewModels.Pagination;
using System.Net;

namespace MVC.Controllers;

public class CatalogController : Controller
{
    private  readonly ICatalogService _catalogService;
    private readonly ILogger<CatalogController> _logger;

    public CatalogController(ICatalogService catalogService, ILogger<CatalogController> logger)
    {
        _logger = logger;
        _catalogService = catalogService;
    }

    [AllowAnonymous]
    [ServiceFilter(typeof(LogActionFilterAttribute<CatalogController>))]
    [ProducesResponseType(typeof(Task<IActionResult>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Index(int? brandFilterApplied, int? typesFilterApplied, int? page, int? itemsPage)
    {   
        page ??= 0;
        itemsPage ??= 6;
        var catalog = await _catalogService.GetCatalogItems(page.Value, itemsPage.Value, brandFilterApplied, typesFilterApplied);
        _logger.LogInformation($"After resievieng catalog items. They are null: {catalog is null}");
        if (catalog == null)
        {
            return View("Error");
        }
        var info = new PaginationInfo()
        {
            ActualPage = page.Value,
            ItemsPerPage = catalog.Data.Count,
            TotalItems = catalog.Count,
            TotalPages = (int)Math.Ceiling((decimal)catalog.Count / itemsPage.Value)
        };
        var vm = new IndexViewModel()
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
}