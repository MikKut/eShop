using System.Text;
using IdentityModel.Client;
using Infrastructure.Configuration;
using Infrastructure.JsonConverterWrapper;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class InternalHttpClientService : IInternalHttpClientService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly AuthorizationConfig _authConfig;
    private readonly ClientConfig _clientConfig;
    private static readonly JsonConvertWrapper _jsonConvertWrapper;
    static InternalHttpClientService()
    {
        _jsonConvertWrapper = new JsonConvertWrapper();
    }

    public InternalHttpClientService(
        IHttpClientFactory clientFactory,
        IOptions<ClientConfig> clientConfig,
        IOptions<AuthorizationConfig> authConfig)
    {
        _clientFactory = clientFactory;
        _authConfig = authConfig.Value;
        _clientConfig = clientConfig.Value;
    }

    public async Task<TResponse> SendAsync<TResponse, TRequest>(string url, HttpMethod method, TRequest? content)
    {
        try
        {
            HttpClient client = _clientFactory.CreateClient();
            TokenResponse tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = $"{_authConfig.Authority}/connect/token",
                ClientId = _clientConfig.Id,
                ClientSecret = _clientConfig.Secret
            });

            client.SetBearerToken(tokenResponse.AccessToken);
            HttpRequestMessage httpMessage = new ()
            {
                RequestUri = new Uri(url),
                Method = method
            };

            if (content != null)
            {
                httpMessage.Content =
                    new StringContent(_jsonConvertWrapper.Serialize(content), Encoding.UTF8, "application/json");
            }

            HttpResponseMessage result = await client.SendAsync(httpMessage);

            if (result.IsSuccessStatusCode)
            {
                string resultContent = await result.Content.ReadAsStringAsync();
                TResponse? response = _jsonConvertWrapper.Deserialize<TResponse>(resultContent);
                return response!;
            }

            return default!;
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return default!;
        }
    }
}
