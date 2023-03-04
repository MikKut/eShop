using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Data.Entities.Interfaces;
using Catalog.Host.Extensions;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class CatalogTypeRepository : ICatalogTypeRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogItemRepository> _logger;

    public CatalogTypeRepository(
        Services.Interfaces.IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogItemRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<int?> AddAsync(ICatalogType type)
    {
        var item = await _dbContext.AddAsync(type);
        await _dbContext.SaveChangesAsync();
        return item.Entity.Id;
    }

    public async Task<bool> DeleteAsync(ICatalogType type)
    {
        var item = await _dbContext.CatalogTypes
           .SingleAsync(t => t.Equal(type));
        if (item == null)
        {
            return false;
        }

        _dbContext.Remove(type);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(int id, ICatalogType type)
    {
        var item = await _dbContext.CatalogTypes
           .FindAsync(id);
        if (item == null)
        {
            return false;
        }

        item.Type = type.Type;
        await _dbContext.SaveChangesAsync();
        return true;
    }
}