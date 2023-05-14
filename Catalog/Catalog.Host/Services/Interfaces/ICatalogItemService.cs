using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Requests;

namespace Catalog.Host.Services.Interfaces;

public interface ICatalogItemService
{
    Task<int?> AddAsync(CatalogItemDto itemToAdd);
    Task<bool> DeleteAsync(CatalogItemDto itemToDelete);
    Task<bool> UpdateAvailableStockAsync(UpdateAvailableStockRequest itemToUpdate);
    Task<bool> UpdateAsync(int id, CatalogItemDto itemToUpdate);
}