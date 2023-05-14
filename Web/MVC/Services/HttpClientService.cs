using IdentityModel.Client;
using MVC.Services.Interfaces;
using Newtonsoft.Json;

namespace MVC.Services;

public class HttpClientService : IHttpClientService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<HttpClientService> _logger;

    public HttpClientService(
        IHttpClientFactory clientFactory,
        IHttpContextAccessor httpContextAccessor,
        ILogger<HttpClientService> logger)
    {
        _clientFactory = clientFactory;
        _httpContextAccessor = httpContextAccessor;
        _logger=logger;
    }

    public async Task<TResponse> SendAsync<TResponse, TRequest>(string url, HttpMethod method, TRequest? content)
    {
        _logger.LogInformation($"In process of sending {method}-request to {url}");
        HttpClient client = _clientFactory.CreateClient();

        string? token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
        if (!string.IsNullOrEmpty(token))
        {
            client.SetBearerToken(token);
        }

        HttpRequestMessage httpMessage = new()
        {
            RequestUri = new Uri(url),
            Method = method
        };

        if (content != null)
        {
            httpMessage.Content =
                new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        }

        _logger.LogInformation($"Content of the request:\n{httpMessage.Content?.ToString()} {httpMessage.Content}\n");
        HttpResponseMessage result = await client.SendAsync(httpMessage);

        if (result.IsSuccessStatusCode)
        {
            string resultContent = await result.Content.ReadAsStringAsync();
            TResponse? response = JsonConvert.DeserializeObject<TResponse>(resultContent);
            _logger.LogInformation($"Sent {httpMessage.Method}-request on {httpMessage.RequestUri} with the next result: \n {response}\n");
            return response!;
        }

        _logger.LogInformation($"Sent {httpMessage.Method}-request on {httpMessage.RequestUri} with default result");
        return default!;
    }
}
