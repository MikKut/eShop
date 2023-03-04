using Basket.Host.Models.Dtos;
using Basket.Host.Models.Requests;
using Basket.Host.Services.Interfaces;
using MVC.Models.Responses;
using System.Net.Http;
using System.Runtime;

namespace Basket.Host.Services
{
    public class OrderService<T> 
        : IOrderService<T>
        where T : class
    {
        private readonly IOptions<AppSettings> _settings;
        private readonly IHttpClientService _httpClient;
        private readonly ILogger<OrderService<T>> _logger;

        public OrderService(
            IOptions<AppSettings> settings,
            IHttpClientService httpClient,
            ILogger<OrderService<T>> logger)
        {
            _settings = settings;
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<SuccessfulResultResponse> CommitPurchases(BasketDto<T> request)
        {
            var result = await _httpClient.SendAsync<SuccessfulResultResponse, PurchaseRequest<T>>
                ($"{_settings.Value.OrderUrl}/CommitPurchases",
                HttpMethod.Post, new PurchaseRequest<T>() { Data = request.Data, );
            return result;
        }
    }
}
