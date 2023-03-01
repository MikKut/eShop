using Infrastructure.UnitTests.Mocks;
using Microsoft.Extensions.Options;

namespace Infrastructure.UnitTests.Services
{
    public class InternalHttpClientServiceTest
    {
        private readonly IInternalHttpClientService _internalHttpClientService;
        private readonly Mock<IHttpClientFactory> _clientFactory;
        private readonly Mock<AuthorizationConfig> _authConfig;
        private readonly Mock<ClientConfig> _clientConfig;
        private readonly MockResponse _response;
        private readonly MockRequest _request;
        public InternalHttpClientServiceTest()
        {
            _request = new () { Id = 1 };
            _response = new () { Id = 2 };
            _clientFactory = new Mock<IHttpClientFactory>();
            _authConfig = new Mock<AuthorizationConfig>();
            _clientConfig = new Mock<ClientConfig>();
            _internalHttpClientService = new InternalHttpClientService(_clientFactory.Object, new MockIOptions<ClientConfig>() { Value = _clientConfig.Object }, new MockIOptions<AuthorizationConfig>() { Value = _authConfig.Object });
        }

        [Fact]
        public async Task SendAsync_SuccessfulHttpResponseMessage()
        {
            // arrange
            var httpResponseMessage = new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK };
            var httpClient = new HttpClient();
            var httpClientMock = new Mock<HttpClient>();
            var wrapper = new Mock<JsonConvertWrapper>();
            _clientFactory.Setup(_ => _.CreateClient()).Returns(httpClient);
            httpClientMock.Setup(_ => _.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(httpResponseMessage);
            wrapper.Setup(_ => _.Deserialize<MockResponse>(It.IsAny<string>())).Returns(_response);

            // act
            var result = await _internalHttpClientService.SendAsync<MockResponse, MockRequest>(It.IsAny<string>(), It.IsAny<HttpMethod>(), _request);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<MockResponse>();
            result.Id.Should().Be(_response.Id);
        }

        [Fact]
        public async Task SendAsync_NonSuccessfulHttpResponseMessage()
        {
            // arrange
            var httpResponseMessage = new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.BadRequest };
            var httpClient = new Mock<HttpClient>();
            _clientFactory.Setup(c => c.CreateClient())
                .Returns(httpClient.Object);
            httpClient.Setup(c => c.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(httpResponseMessage);
            var wrapper = new Mock<JsonConvertWrapper>();
            wrapper.Setup(x => x.Deserialize<MockResponse>(It.IsAny<string>())).Returns(_response);

            // act
            var result = await _internalHttpClientService.SendAsync<MockResponse, MockRequest>(It.IsAny<string>(), It.IsAny<HttpMethod>(), _request);

            // assert
            result.Should().BeNull();
        }
    }
}
