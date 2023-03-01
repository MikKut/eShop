using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Enums;
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

    /// <summary>
    /// Gets elements by page with some filters.
    /// </summary>
    /// <typeparam name="T">Enum that represents filter type.</typeparam>
    /// <param name="pageIndex">Represents index of the page to take.</param>
    /// <param name="pageSize">Represents size of the pages.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns><param name="filters">Dicrionary of filters with key of type <see cref="CatalogTypeFilter"> and id of filter as a value.</param>.
    public async Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize, Dictionary<CatalogTypeFilter, int>? filters)
    {
        long totalItems;
        if (filters == null)
        {
            filters = new Dictionary<CatalogTypeFilter, int>();
            totalItems = await _dbContext.CatalogItems
                .LongCountAsync();
        }
        else
        {
            totalItems = await _dbContext.CatalogItems
            .Where(i => !filters.ContainsKey(CatalogTypeFilter.Brand) || i.CatalogBrandId == filters[CatalogTypeFilter.Brand])
            .Where(i => !filters.ContainsKey(CatalogTypeFilter.Type) || i.CatalogTypeId == filters[CatalogTypeFilter.Type])
            .LongCountAsync();
        }

        var itemsOnPage = await _dbContext.CatalogItems
            .Where(i => !filters.ContainsKey(CatalogTypeFilter.Brand) || i.CatalogBrandId == filters[CatalogTypeFilter.Brand])
            .Where(i => !filters.ContainsKey(CatalogTypeFilter.Type) || i.CatalogTypeId == filters[CatalogTypeFilter.Type])
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        _logger.LogInformation("\nFilters\n");
        foreach (var filter in filters)
        {
            _logger.LogInformation($"{filter.Key}({filter.Value.ToString()}) - {filter.Value}");
        }

        _logger.LogInformation("\nValues\n");
        foreach (var item in itemsOnPage)
        {
            _logger.LogInformation($"{item.Id} - {item.Name} of {item.CatalogBrand.Brand}({item.CatalogBrand.Id}) brand and {item.CatalogType.Type}({item.CatalogType.Id}) type");
        }

        _logger.LogInformation($"\nEnded with {totalItems} total items and {itemsOnPage.Count} items on page\n");
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