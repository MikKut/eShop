using AutoMapper;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services
{
    public class CatalogTypeService : BaseDataService<ApplicationDbContext>, ICatalogTypeService
    {
        private readonly ICatalogTypeRepository _catalogTypeRepository;
        private readonly IMapper _mapper;
        public CatalogTypeService(
            Interfaces.IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> logger,
            ICatalogTypeRepository catalogItemRepository,
            IMapper mapper)
            : base(dbContextWrapper, logger)
        {
            _mapper = mapper;
            _catalogTypeRepository = catalogItemRepository;
        }

        public async Task<int?> AddAsync(CatalogTypeDto itemToAdd)
        {
            return await ExecuteSafeAsync(() => _catalogTypeRepository.AddAsync(_mapper.Map<CatalogType>(itemToAdd)));
        }

        public async Task<bool> DeleteAsync(CatalogTypeDto itemToDelete)
        {
            return await ExecuteSafeAsync(() => _catalogTypeRepository.DeleteAsync(_mapper.Map<CatalogType>(itemToDelete)));
        }

        public async Task<bool> UpdateAsync(int id, CatalogTypeDto itemToAdd)
        {
            return await ExecuteSafeAsync(() => _catalogTypeRepository.UpdateAsync(id, _mapper.Map<CatalogType>(itemToAdd)));
        }
    }
}
