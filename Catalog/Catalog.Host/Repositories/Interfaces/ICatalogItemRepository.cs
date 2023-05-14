using Catalog.Host.Data.Entities;

namespace Catalog.Host.Repositories.Interfaces;

public interface ICatalogItemRepository
{
    Task<int?> AddAsync(CatalogItem itemToAdd);
    Task<bool> DeleteAsync(CatalogItem itemToDelete);
    Task<bool> UpdateAsync(int id, CatalogItem itemToUpdate);
    Task<bool> UpdateAvailableStockAsync(int id, int availableStock);
}