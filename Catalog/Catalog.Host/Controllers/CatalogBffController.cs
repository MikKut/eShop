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
[AllowAnonymous]
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
    public async Task<IActionResult> Items(PaginatedItemsRequest<CatalogTypeFilter> request)
    {
        _logger.LogDebug("Entered Items from catalog bff");
        PaginatedItemsResponse<CatalogItemDto> result = await _catalogService.GetCatalogItemsAsync(request.PageSize, request.PageIndex, request.Filters);
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ItemResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    [ServiceFilter(typeof(LogActionFilterAttribute<CatalogBffController>))]
    public async Task<IActionResult> GetByID(GetByIdRequest request)
    {
        CatalogItemDto result = await _catalogService.GetByIDAsync(request.ID);
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ItemResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    [ServiceFilter(typeof(LogActionFilterAttribute<CatalogBffController>))]
    public async Task<IActionResult> GetByBrand(GetByIdRequest request)
    {
        CatalogItemDto result = await _catalogService.GetByBrandAsync(request.ID);
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ItemResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    [ServiceFilter(typeof(LogActionFilterAttribute<CatalogBffController>))]
    public async Task<IActionResult> GetByType(GetByIdRequest request)
    {
        CatalogItemDto result = await _catalogService.GetByTypeAsync(request.ID);
        return Ok(result);
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(DataCollectionResponse<CatalogBrand>), (int)HttpStatusCode.OK)]
    [ServiceFilter(typeof(LogActionFilterAttribute<CatalogBffController>))]
    public async Task<IActionResult> GetBrands()
    {
        List<CatalogBrandDto> result = await _catalogService.GetBrandsAsync();
        return Ok(result);
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(DataCollectionResponse<CatalogType>), (int)HttpStatusCode.OK)]
    [ServiceFilter(typeof(LogActionFilterAttribute<CatalogBffController>))]
    public async Task<IActionResult> GetTypes()
    {
        List<CatalogTypeDto> result = await _catalogService.GetTypesAsync();
        return Ok(result);
    }
}