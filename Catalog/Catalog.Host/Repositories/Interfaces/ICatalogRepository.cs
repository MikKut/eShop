using Catalog.Host.Data;
using Catalog.Host.Data.Entities;

namespace Catalog.Host.Repositories.Interfaces;

public interface ICatalogRepository
{
    Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize);
    Task<CatalogItem?> GetByIDAsync(int id);
    Task<CatalogItem?> GetByBrandAsync(int catalogBrandId);
    Task<CatalogItem?> GetByTypeAsync(int catalogTypeId);
    Task<List<CatalogBrand>> GetBrandsAsync();
    Task<List<CatalogType>> GetTypesAsync();
}