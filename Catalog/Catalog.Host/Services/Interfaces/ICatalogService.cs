using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Enums;
using Catalog.Host.Models.Response;

namespace Catalog.Host.Services.Interfaces;

public interface ICatalogService
{
    Task<PaginatedItemsResponse<CatalogItemDto>> GetCatalogItemsAsync(int pageSize, int pageIndex, Dictionary<CatalogTypeFilter, int>? filters);
    Task<List<CatalogBrandDto>> GetBrandsAsync();
    Task<CatalogItemDto> GetByBrandAsync(int catalogTypeId);
    Task<CatalogItemDto> GetByIDAsync(int id);
    Task<CatalogItemDto> GetByTypeAsync(int catalogTypeId);
    Task<List<CatalogTypeDto>> GetTypesAsync();
}