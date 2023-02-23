using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class CatalogRepository : ICatalogRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogItemRepository> _logger;

    public CatalogRepository(
        Services.Interfaces.IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogItemRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize)
    {
        var totalItems = await _dbContext.CatalogItems
            .LongCountAsync();

        var itemsOnPage = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<List<CatalogBrand>> GetBrandsAsync()
    {
        return await _dbContext.CatalogBrands
            .ToListAsync();
    }

    public async Task<CatalogItem?> GetByBrandAsync(int catalogBrandId)
    {
        var itemByBrand = _dbContext.CatalogItems
           .Include(i => i.CatalogBrand)
           .Include(i => i.CatalogType)
           .FirstOrDefaultAsync(t => t.CatalogBrand.Id == catalogBrandId);

        return await itemByBrand;
    }

    public async Task<CatalogItem?> GetByIDAsync(int id)
    {
        var itemWithID = _dbContext.CatalogItems
           .Include(i => i.CatalogBrand)
           .Include(i => i.CatalogType)
           .FirstOrDefaultAsync(t => t.Id == id);

        return await itemWithID;
    }

    public async Task<CatalogItem?> GetByTypeAsync(int catalogTypeId)
    {
        var itemByType = _dbContext.CatalogItems
          .Include(i => i.CatalogBrand)
          .Include(i => i.CatalogType)
          .FirstOrDefaultAsync(i => i.CatalogType.Id == catalogTypeId);

        return await itemByType;
    }

    public async Task<List<CatalogType>> GetTypesAsync()
    {
        return await _dbContext.CatalogTypes
            .ToListAsync();
    }
}