using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;

namespace Catalog.Host.Services.Interfaces;

public interface ICatalogItemService
{
    Task<int?> AddAsync(CatalogItemDto itemToAdd);
    Task<bool> DeleteAsync(CatalogItemDto itemToDelete);
    Task<bool> UpdateAsync(int id, CatalogItemDto itemToUpdate);
}