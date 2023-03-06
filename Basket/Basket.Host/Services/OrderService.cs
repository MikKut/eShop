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
        private readonly IInternalHttpClientService _httpClient;
        private readonly ILogger<OrderService<T>> _logger;

        public OrderService(
            IOptions<AppSettings> settings,
            IInternalHttpClientService httpClient,
            ILogger<OrderService<T>> logger)
        {
            _settings = settings;
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<SuccessfulResultResponse> CommitPurchases(OrderDto<T> request)
        {
            string url = $"{_settings.Value.OrderUrl}/CommitPurchases";
            _logger.LogInformation($"Sent information to the {url}");
            var result = await _httpClient.SendAsync<SuccessfulResultResponse, PurchaseRequest<T>>
                (url,
                HttpMethod.Post, new PurchaseRequest<T>() { Data = request.Orders, ID = request.User.UserId });
            if (result.IsSuccessful)
            {
                _logger.LogInformation($"The result of execution request by {url} is {result.IsSuccessful}");
            }
            else
            {
                _logger.LogWarning($"The result of execution request by {url} is {result.IsSuccessful}. Reason: {result!.Message}");
            }

            return result;
        }
    }
}
