using AutoMapper;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;
using MVC.Dtos;
using MVC.Models.Enums;
using MVC.Services.Interfaces;
using MVC.ViewModels.Models;

namespace MVC.Services;

public class CatalogService : ICatalogService
{
    private readonly IOptions<AppSettings> _settings;
    private readonly IHttpClientService _httpClient;
    private readonly ILogger<CatalogService> _logger;
    private readonly IMapper _mapper;
    public CatalogService(IHttpClientService httpClient, ILogger<CatalogService> logger, IOptions<AppSettings> settings, IMapper mapper)
    {
        _mapper = mapper;
        _httpClient = httpClient;
        _settings = settings;
        _logger = logger;
    }

    public async Task<Catalog> GetCatalogItems(int page, int take, int? brand, int? type)
    {
        var filters = new Dictionary<CatalogTypeFilter, int>();

        if (brand.HasValue)
        {
            filters.Add(CatalogTypeFilter.Brand, brand.Value);
        }
        
        if (type.HasValue)
        {
            filters.Add(CatalogTypeFilter.Type, type.Value);
        }
        
        var request = $"{_settings.Value.CatalogUrl}/Items";
        _logger.LogInformation($"Before sending request on {request} to take page #{page} with {take} page size with");
        var result = await _httpClient.SendAsync<Catalog, PaginatedItemsRequest<CatalogTypeFilter>>(request,
           HttpMethod.Post, 
           new PaginatedItemsRequest<CatalogTypeFilter>()
            {
                PageIndex = page,
                PageSize = take,
                Filters = filters
           });
        _logger.LogInformation($"After sending request on {request} to take {page} pages with {take} page size result is null: {result == null}");
        return result;
    }

    public async Task<IEnumerable<SelectListItem>> GetBrands()
    {
        var request = $"{_settings.Value.CatalogUrl}/GetBrands";
        _logger.LogInformation($"Entered {nameof(GetBrands)} of {nameof(CatalogBrand)}. It is goint to send request on {request}");
        var result = await _httpClient.SendAsync<List<CatalogBrand>, object>(request,
            HttpMethod.Get, null);
        _logger.LogInformation($"Got result in {nameof(GetBrands)} of {nameof(CatalogBrand)}: after sending request on {request} result is null: {result == null}");
        return result.Select(x => _mapper.Map<CatalogBrand, SelectListItem>(x));
    }

    public async Task<IEnumerable<SelectListItem>> GetTypes()
    {
        var request = $"{_settings.Value.CatalogUrl}/GetTypes";
        _logger.LogInformation($"Entered {nameof(GetTypes)} of {nameof(CatalogBrand)}. It is goint to send request on {request}");
        var result = await _httpClient.SendAsync<List<CatalogType>, object>(request,
            HttpMethod.Get, null);
        _logger.LogInformation($"Got result in {nameof(GetTypes)} of {nameof(CatalogBrand)}: after sending request on {request} result is null: {result == null}");
        return result.Select(x => _mapper.Map<CatalogType, SelectListItem>(x));
    }
}
