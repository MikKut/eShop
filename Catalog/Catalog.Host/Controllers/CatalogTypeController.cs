using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;
using Infrastructure.Filters;
using Infrastructure.Identity;
using Infrastructure.Models.Responses;
using Microsoft.AspNetCore.Authorization;

namespace Catalog.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowClientPolicy)]
[Scope("catalog.catalogtype")]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogTypeController : ControllerBase
{
    private readonly ILogger<CatalogItemController> _logger;
    private readonly ICatalogTypeService _catalogTypeService;

    public CatalogTypeController(
        ILogger<CatalogItemController> logger,
        ICatalogTypeService catalogTypeService)
    {
        _logger = logger;
        _catalogTypeService = catalogTypeService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddItemResponse<int?>), (int)HttpStatusCode.OK)]
    [ServiceFilter(typeof(LogActionFilterAttribute<CatalogTypeController>))]
    public async Task<IActionResult> Add(CreateRequest<CatalogTypeDto> request)
    {
        int? result = await _catalogTypeService.AddAsync(request.Data);
        return Ok(new AddItemResponse<int?>() { Id = result });
    }

    [HttpDelete]
    [ProducesResponseType(typeof(SuccessfulResultResponse), (int)HttpStatusCode.OK)]
    [ServiceFilter(typeof(LogActionFilterAttribute<CatalogTypeController>))]
    public async Task<IActionResult> Delete(DeleteRequest<CatalogTypeDto> request)
    {
        bool result = await _catalogTypeService.DeleteAsync(request.Data);
        return Ok(new SuccessfulResultResponse() { IsSuccessful = result });
    }

    [HttpPost]
    [ProducesResponseType(typeof(SuccessfulResultResponse), (int)HttpStatusCode.OK)]
    [ServiceFilter(typeof(LogActionFilterAttribute<CatalogTypeController>))]
    public async Task<IActionResult> Update(UpdateRequest<CatalogTypeDto> request)
    {
        bool result = await _catalogTypeService.UpdateAsync(request.ID, request.NewData);
        return Ok(new SuccessfulResultResponse() { IsSuccessful = result });
    }
}