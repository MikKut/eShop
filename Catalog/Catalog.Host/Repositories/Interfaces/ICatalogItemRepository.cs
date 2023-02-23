using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;

namespace Catalog.Host.Repositories.Interfaces;

public interface ICatalogItemRepository
{
    Task<int?> AddAsync(CatalogItem itemToAdd);
    Task<bool> DeleteAsync(CatalogItem itemToDelete);
    Task<bool> UpdateAsync(int id, CatalogItem itemToUpdate);
}