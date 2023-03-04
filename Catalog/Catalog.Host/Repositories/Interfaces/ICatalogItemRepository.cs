using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Data.Entities.Interfaces;

namespace Catalog.Host.Repositories.Interfaces;

public interface ICatalogItemRepository
{
    Task<int?> AddAsync(ICatalogItem itemToAdd);
    Task<bool> DeleteAsync(ICatalogItem itemToDelete);
    Task<bool> UpdateAsync(int id, ICatalogItem itemToUpdate);
}