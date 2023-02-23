using Catalog.Host.Configurations;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Enums;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;
using Infrastructure.Filters;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Catalog.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogBffController : ControllerBase
{
    private readonly ILogger<CatalogBffController> _logger;
    private readonly ICatalogService _catalogService;
    private readonly IOptions<CatalogConfig> _config;

    public CatalogBffController(
        ILogger<CatalogBffController> logger,
        ICatalogService catalogService,
        IOptions<CatalogConfig> config)
    {
        _logger = logger;
        _catalogService = catalogService;
        _config = config;
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ItemResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    [ServiceFilter(typeof(LogActionFilterAttribute<CatalogBffController>))]
    public async Task<IActionResult> Items(PaginatedItemsRequest request)
    {
        var result = await _catalogService.GetCatalogItemsAsync(request.PageSize, request.PageIndex);
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ItemResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    [ServiceFilter(typeof(LogActionFilterAttribute<CatalogBffController>))]
    public async Task<IActionResult> GetByID(GetByIdRequest request)
    {
        var result = await _catalogService.GetByIDAsync(request.ID);
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ItemResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    [ServiceFilter(typeof(LogActionFilterAttribute<CatalogBffController>))]
    public async Task<IActionResult> GetByBrand(GetByIdRequest request)
    {
        var result = await _catalogService.GetByBrandAsync(request.ID);
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ItemResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    [ServiceFilter(typeof(LogActionFilterAttribute<CatalogBffController>))]
    public async Task<IActionResult> GetByType(GetByIdRequest request)
    {
        var result = await _catalogService.GetByTypeAsync(request.ID);
        return Ok(result);
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(DataCollectionResponse<CatalogBrand>), (int)HttpStatusCode.OK)]
    [ServiceFilter(typeof(LogActionFilterAttribute<CatalogBffController>))]
    public async Task<IActionResult> GetBrands()
    {
        var result = await _catalogService.GetBrandsAsync();
        return Ok(result);
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(DataCollectionResponse<CatalogType>), (int)HttpStatusCode.OK)]
    [ServiceFilter(typeof(LogActionFilterAttribute<CatalogBffController>))]
    public async Task<IActionResult> GetTypes()
    {
        var result = await _catalogService.GetTypesAsync();
        return Ok(result);
    }
}