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
[Scope("catalog.catalogitem")]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogItemController : ControllerBase
{
    private readonly ILogger<CatalogItemController> _logger;
    private readonly ICatalogItemService _catalogItemService;

    public CatalogItemController(
        ILogger<CatalogItemController> logger,
        ICatalogItemService catalogItemService)
    {
        _logger = logger;
        _catalogItemService = catalogItemService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddItemResponse<int?>), (int)HttpStatusCode.OK)]
    [ServiceFilter(typeof(LogActionFilterAttribute<CatalogItemController>))]
    public async Task<IActionResult> Add(CreateProductRequest request)
    {
        int? result = await _catalogItemService.AddAsync(new CatalogItemDto() { Name = request.Name, AvailableStock = request.AvailableStock, Price = request.Price, PictureUrl = request.PictureFileName, Description = request.Description, CatalogBrandId = request.CatalogBrandId, CatalogTypeId = request.CatalogTypeId });
        return Ok(new AddItemResponse<int?>() { Id = result });
    }

    [HttpDelete]
    [ProducesResponseType(typeof(SuccessfulResultResponse), (int)HttpStatusCode.OK)]
    [ServiceFilter(typeof(LogActionFilterAttribute<CatalogItemController>))]
    public async Task<IActionResult> Delete(DeleteProductRequest request)
    {
        bool result = await _catalogItemService.DeleteAsync(new CatalogItemDto() { Name = request.Name, AvailableStock = request.AvailableStock, Price = request.Price, PictureUrl = request.PictureFileName, Description = request.Description, CatalogBrandId = request.CatalogBrandId, CatalogTypeId = request.CatalogTypeId });
        return Ok(new SuccessfulResultResponse { IsSuccessful = result });
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddItemResponse<int?>), (int)HttpStatusCode.OK)]
    [ServiceFilter(typeof(LogActionFilterAttribute<CatalogItemController>))]
    public async Task<IActionResult> Update(UpdateProductRequest request)
    {
        bool result = await _catalogItemService.UpdateAsync(request.ID, new CatalogItemDto() { Name = request.NewName, AvailableStock = request.NewAvailableStock, Price = request.NewPrice, PictureUrl = request.NewPictureFileName, Description = request.NewDescription, CatalogBrandId = request.NewCatalogBrandId, CatalogTypeId = request.NewCatalogTypeId });
        return Ok(new SuccessfulResultResponse() { IsSuccessful = result });
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddItemResponse<int?>), (int)HttpStatusCode.OK)]
    [ServiceFilter(typeof(LogActionFilterAttribute<CatalogItemController>))]
    public async Task<IActionResult> UpdateAvailableStock(UpdateAvailableStockRequest request)
    {
        bool result = await _catalogItemService.UpdateAvailableStockAsync(request);
        return Ok(new SuccessfulResultResponse() { IsSuccessful = result });
    }
}