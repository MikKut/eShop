using AutoMapper;
using Catalog.Host.Data;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Enums;
using Catalog.Host.Models.Response;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogService : BaseDataService<ApplicationDbContext>, ICatalogService
{
    private readonly ICatalogRepository _catalogRepository;
    private readonly IMapper _mapper;

    public CatalogService(
        Interfaces.IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogRepository catalogRepository,
        IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _catalogRepository = catalogRepository;
        _mapper = mapper;
    }

    public async Task<List<CatalogBrandDto>> GetBrandsAsync()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogRepository.GetBrandsAsync();
            var listOfCatalogItemDtos = result
            .Select(item => _mapper.Map<CatalogBrandDto>(item))
            .ToList();
            return listOfCatalogItemDtos;
        });
    }

    public async Task<CatalogItemDto> GetByBrandAsync(int catalogTypeId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogRepository.GetByBrandAsync(catalogTypeId);
            return _mapper.Map<CatalogItemDto>(result);
        });
    }

    public async Task<CatalogItemDto> GetByIDAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogRepository.GetByIDAsync(id);
            return _mapper.Map<CatalogItemDto>(result);
        });
    }

    public async Task<CatalogItemDto> GetByTypeAsync(int catalogTypeId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogRepository.GetByTypeAsync(catalogTypeId);
            return _mapper.Map<CatalogItemDto>(result);
        });
    }

    public async Task<PaginatedItemsResponse<CatalogItemDto>> GetCatalogItemsAsync(int pageSize, int pageIndex, Dictionary<CatalogTypeFilter, int>? filters)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogRepository.GetByPageAsync(pageIndex, pageSize, filters);
            return new PaginatedItemsResponse<CatalogItemDto>()
            {
                Count = result.TotalCount,
                Data = result.Data.Select(s => _mapper.Map<CatalogItemDto>(s)).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        });
    }

    public async Task<List<CatalogTypeDto>> GetTypesAsync()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogRepository.GetTypesAsync();
            var listOfCatalogTypeDtos = result
            .Select(item => _mapper.Map<CatalogTypeDto>(item))
            .ToList();
            return listOfCatalogTypeDtos;
        });
    }
}