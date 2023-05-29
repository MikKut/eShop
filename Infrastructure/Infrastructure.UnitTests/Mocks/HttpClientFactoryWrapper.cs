// using Flurl.Http.Configuration;
using System;
using System.Net.Http;

namespace Infrastructure.UnitTests.Mocks
{
    public class HttpClientFactoryWrapper : IHttpClientFactoryWrapper
    {
        private readonly IHttpClientFactory _clientFactory;

        public HttpClientFactoryWrapper(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public HttpClientFactoryWrapper()
        {
            _clientFactory = new SimpleHttpClientFactory();
        }

        public HttpClient CreateClient()
        {
            return _clientFactory.CreateClient();
        }

        public HttpClient CreateClient(string name)
        {
            throw new NotImplementedException();
        }

        private class SimpleHttpClientFactory : IHttpClientFactory
        {
            private readonly HttpClient _httpClient;

            public SimpleHttpClientFactory()
            {
            }

            public SimpleHttpClientFactory(HttpClient httpClient)
            {
                _httpClient = httpClient;
            }

            public HttpClient CreateClient(string name)
            {
                return _httpClient;
            }
        }
    }
}