using AutoMapper;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Requests;
using Catalog.Host.Repositories;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogItemService : BaseDataService<ApplicationDbContext>, ICatalogItemService
{
    private readonly ICatalogItemRepository _catalogItemRepository;
    private readonly IMapper _mapper;

    public CatalogItemService(
        Interfaces.IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogItemRepository catalogItemRepository,
        IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _mapper = mapper;
        _catalogItemRepository = catalogItemRepository;
    }

    public async Task<int?> AddAsync(CatalogItemDto itemToAdd)
    {
        return await ExecuteSafeAsync(() => _catalogItemRepository.AddAsync(_mapper.Map<CatalogItem>(itemToAdd)));
    }

    public async Task<bool> DeleteAsync(CatalogItemDto itemToDelete)
    {
        return await ExecuteSafeAsync(() => _catalogItemRepository.DeleteAsync(_mapper.Map<CatalogItem>(itemToDelete)));
    }

    public async Task<bool> UpdateAsync(int id, CatalogItemDto itemToUpdate)
    {
        return await ExecuteSafeAsync(() => _catalogItemRepository.UpdateAsync(id, _mapper.Map<CatalogItem>(itemToUpdate)));
    }

    public async Task<bool> UpdateAvailableStockAsync(UpdateAvailableStockRequest itemToUpdate)
    {
        return await ExecuteSafeAsync(() => _catalogItemRepository.UpdateAvailableStockAsync(itemToUpdate.Id, itemToUpdate.AvailableStock));
    }
}