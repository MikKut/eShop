using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Extensions;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class CatalogBrandRepository : ICatalogBrandRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogBrandRepository> _logger;

    public CatalogBrandRepository(
        Services.Interfaces.IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogBrandRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<int?> AddAsync(CatalogBrand brand)
    {
        var item = await _dbContext.CatalogBrands.AddAsync(brand);
        await _dbContext.SaveChangesAsync();
        return item.Entity.Id;
    }

    public async Task<bool> DeleteAsync(CatalogBrand brand)
    {
        var item = await _dbContext.CatalogBrands
           .SingleAsync(t => t.Equal(brand));
        if (item == null)
        {
            return false;
        }

        _dbContext.CatalogBrands.Remove(brand);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(int id, CatalogBrand brand)
    {
        var item = await _dbContext.CatalogBrands
           .FindAsync(id);
        if (item == null)
        {
            return false;
        }

        item.Brand = brand.Brand;
        await _dbContext.SaveChangesAsync();
        return true;
    }
}