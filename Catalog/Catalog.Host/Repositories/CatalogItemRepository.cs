using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Data.Entities.Interfaces;
using Catalog.Host.Extensions;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class CatalogItemRepository : ICatalogItemRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogItemRepository> _logger;

    public CatalogItemRepository(
        Services.Interfaces.IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogItemRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<int?> AddAsync(CatalogItem itemToAdd)
    {
        var item = await _dbContext.AddAsync(itemToAdd);
        await _dbContext.SaveChangesAsync();
        return item.Entity.Id;
    }

    public async Task<bool> DeleteAsync(CatalogItem itemToDelete)
    {
        var item = await _dbContext.CatalogItems
           .SingleAsync(t => t.Equal(itemToDelete));
        if (item == null)
        {
            return false;
        }

        _dbContext.Remove(item);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(int id, CatalogItem itemToUpdate)
    {
        var item = await _dbContext.CatalogItems
           .FindAsync(id);
        if (item == null)
        {
            return false;
        }

        item!.AvailableStock = itemToUpdate.AvailableStock!;
        item!.Name = itemToUpdate.Name;
        item!.Description = itemToUpdate.Description;
        item!.Price = itemToUpdate.Price;
        item!.CatalogTypeId = itemToUpdate.CatalogTypeId;
        item!.CatalogBrandId = itemToUpdate.CatalogBrandId;
        item!.PictureFileName = itemToUpdate.PictureFileName;
        await _dbContext.SaveChangesAsync();
        return true;
    }
}