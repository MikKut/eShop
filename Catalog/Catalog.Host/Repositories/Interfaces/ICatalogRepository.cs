using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Enums;

namespace Catalog.Host.Repositories.Interfaces;

public interface ICatalogRepository
{
    Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize, Dictionary<CatalogTypeFilter, int>? filters);
    Task<CatalogItem?> GetByIDAsync(int id);
    Task<CatalogItem?> GetByBrandAsync(int catalogBrandId);
    Task<CatalogItem?> GetByTypeAsync(int catalogTypeId);
    Task<List<CatalogBrand>> GetBrandsAsync();
    Task<List<CatalogType>> GetTypesAsync();
}