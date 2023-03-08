using AutoMapper;
using MVC.Models.Enums;
using MVC.Models.Requests;
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
        Dictionary<CatalogTypeFilter, int> filters = new();

        if (brand.HasValue)
        {
            filters.Add(CatalogTypeFilter.Brand, brand.Value);
        }

        if (type.HasValue)
        {
            filters.Add(CatalogTypeFilter.Type, type.Value);
        }

        string request = $"{_settings.Value.CatalogUrl}/Items";
        _logger.LogInformation($"Before sending request on {request} to take page #{page} with {take} page size with");
        Catalog? result = await _httpClient.SendAsync<Catalog, PaginatedItemsRequest<CatalogTypeFilter>>(request,
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
        string request = $"{_settings.Value.CatalogUrl}/GetBrands";
        _logger.LogInformation($"Entered {nameof(GetBrands)} of {nameof(CatalogBrand)}. It is going to send request on {request}");
        List<CatalogBrand>? result = await _httpClient.SendAsync<List<CatalogBrand>, object>(request,
            HttpMethod.Get, null);
        _logger.LogInformation($"Got result in {nameof(GetBrands)} of {nameof(CatalogBrand)}: after sending request on {request} result is null: {result == null}");
        return result.Select(x => _mapper.Map<CatalogBrand, SelectListItem>(x));
    }

    public async Task<IEnumerable<SelectListItem>> GetTypes()
    {
        string request = $"{_settings.Value.CatalogUrl}/GetTypes";
        _logger.LogInformation($"Entered {nameof(GetTypes)} of {nameof(CatalogBrand)}. It is going to send request on {request}");
        List<CatalogType>? result = await _httpClient.SendAsync<List<CatalogType>, object>(request,
            HttpMethod.Get, null);
        _logger.LogInformation($"Got result in {nameof(GetTypes)} of {nameof(CatalogBrand)}: after sending request on {request} result is null: {result == null}");
        return result.Select(x => _mapper.Map<CatalogType, SelectListItem>(x));
    }

    public async Task<CatalogItem?> GetCatalogItemById(int id)
    {
        string request = $"{_settings.Value.CatalogUrl}/GetByID";
        _logger.LogInformation($"Entered {nameof(GetTypes)} of {nameof(CatalogBrand)}. It is going to send request on {request}");
        CatalogItem? result = await _httpClient.SendAsync<CatalogItem, GetByIdRequest>(
            request,
            HttpMethod.Post,
            new GetByIdRequest() { ID = id });
        _logger.LogInformation($"Got result in {nameof(GetTypes)} of {nameof(CatalogBrand)}: after sending request on {request} result is null: {result == null}");
        return result;
    }
}
