using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;
using Infrastructure.Filters;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Catalog.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowClientPolicy)]
[Scope("catalog.catalogbrand")]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogBrandController : ControllerBase
{
    private readonly ILogger<CatalogItemController> _logger;
    private readonly ICatalogBrandService _catalogBrandService;

    public CatalogBrandController(
        ILogger<CatalogItemController> logger,
        ICatalogBrandService catalogBrandService)
    {
        _logger = logger;
        _catalogBrandService = catalogBrandService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddItemResponse<int?>), (int)HttpStatusCode.OK)]
    [ServiceFilter(typeof(LogActionFilterAttribute<CatalogBrandController>))]
    public async Task<IActionResult> Add(CreateRequest<CatalogBrandDto> request)
    {
        var result = await _catalogBrandService.AddAsync(request.Data);
        return Ok(new AddItemResponse<int?>() { Id = result });
    }

    [HttpDelete]
    [ProducesResponseType(typeof(IsSuccededResponse), (int)HttpStatusCode.OK)]
    [ServiceFilter(typeof(LogActionFilterAttribute<CatalogBrandController>))]
    public async Task<IActionResult> Delete(DeleteRequest<CatalogBrandDto> request)
    {
        var result = await _catalogBrandService.DeleteAsync(request.Data);
        return Ok(new IsSuccededResponse() { IsSucceeded = result });
    }

    [HttpPost]
    [ProducesResponseType(typeof(IsSuccededResponse), (int)HttpStatusCode.OK)]
    [ServiceFilter(typeof(LogActionFilterAttribute<CatalogBrandController>))]
    public async Task<IActionResult> Update(UpdateRequest<CatalogBrandDto> request)
    {
        var result = await _catalogBrandService.UpdateAsync(request.ID, request.NewData);
        return Ok(new IsSuccededResponse() { IsSucceeded = result });
    }
}