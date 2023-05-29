using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using global::Order.Host;
    using global::Order.Host.Models.Dtos;
    using global::Order.Host.Models.Requests;
    using global::Order.Host.Models.Responses;
    using global::Order.Host.Services;
    using Infrastructure.Models.Responses;
    using Infrastructure.Services.Interfaces;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    namespace Order.UnitTests.Services
    {
        public class CatalogItemServiceTests
        {
            private readonly Mock<IOptions<AppSettings>> _mockSettings;
            private readonly Mock<IInternalHttpClientService> _httpClientServiceMock;
            private readonly CatalogItemService _catalogItemService;
            private readonly List<CatalogItemDto> _data;
            private readonly Dictionary<int, int> _catalogItems;
            private readonly List<CatalogItemResponse> _listOfResponses;
            public CatalogItemServiceTests()
            {
                _mockSettings = new Mock<IOptions<AppSettings>>();
                _httpClientServiceMock = new Mock<IInternalHttpClientService>();
                _catalogItemService= new CatalogItemService(_httpClientServiceMock.Object, _mockSettings.Object);
                _data = new List<CatalogItemDto>
                {
                    new CatalogItemDto { Id = 1 },
                    new CatalogItemDto { Id = 1 },
                    new CatalogItemDto { Id = 2 }
                };
                _catalogItems = new Dictionary<int, int>
                {
                    { 1, 2 },
                    { 2, 3 }
                };
                _listOfResponses = new List<CatalogItemResponse>
                {
                    new CatalogItemResponse { Id = 1, AvailableStock = 2 },
                    new CatalogItemResponse { Id = 2, AvailableStock = 4 }
                };
            }

            [Fact]
            public async Task ReduceQuantityOfItemsAsync_ReturnsSuccessfulResultResponse_WhenAvailabilityCheckPasses()
            {
                // Arrange
                var mockAppSettings = new AppSettings { OrderUrl = "https://example.com" };
                _mockSettings.Setup(x => x.Value).Returns(mockAppSettings);
                _httpClientServiceMock.SetupSequence(x => x.SendAsync<CatalogItemResponse, GetByIdRequest>(It.IsAny<string>(), HttpMethod.Post, It.IsAny<GetByIdRequest>()))
                    .ReturnsAsync(new CatalogItemResponse { Id = 1, AvailableStock = 2 })
                    .ReturnsAsync(new CatalogItemResponse { Id = 2, AvailableStock = 4 });

                _httpClientServiceMock.SetupSequence(x => x.SendAsync<SuccessfulResultResponse, UpdateAvailableStockRequest>(It.IsAny<string>(), HttpMethod.Post, It.IsAny<UpdateAvailableStockRequest>()))
                    .ReturnsAsync(new SuccessfulResultResponse { IsSuccessful = true })
                    .ReturnsAsync(new SuccessfulResultResponse { IsSuccessful = true });

                // Act
                var result = await _catalogItemService.ReduceQuantityOfItemsAsync(_data);

                // Assert
                Assert.IsType<SuccessfulResultResponse>(result);
                Assert.True(result.IsSuccessful);
                _httpClientServiceMock.Verify(x => x.SendAsync<CatalogItemResponse, GetByIdRequest>(It.IsAny<string>(), HttpMethod.Post, It.IsAny<GetByIdRequest>()), Times.Exactly(2));
                _httpClientServiceMock.Verify(x => x.SendAsync<SuccessfulResultResponse, UpdateAvailableStockRequest>(It.IsAny<string>(), HttpMethod.Post, It.IsAny<UpdateAvailableStockRequest>()), Times.Exactly(2));
            }

            [Fact]
            public async Task ReduceQuantityOfItemsAsync_ReturnsFailedResultResponse_WhenAvailabilityCheckFails()
            {
                // Arrange
                var mockAppSettings = new AppSettings { OrderUrl = "https://example.com" };
                _mockSettings.Setup(x => x.Value).Returns(mockAppSettings);
                _httpClientServiceMock.SetupSequence(x => x.SendAsync<CatalogItemResponse, GetByIdRequest>(It.IsAny<string>(), HttpMethod.Post, It.IsAny<GetByIdRequest>()))
                    .ReturnsAsync(new CatalogItemResponse { Id = 1, AvailableStock = 1 })
                    .ReturnsAsync(new CatalogItemResponse { Id = 2, AvailableStock = 1 });

                // Act
                var result = await _catalogItemService.ReduceQuantityOfItemsAsync(_data);

                // Assert
                Assert.IsType<SuccessfulResultResponse>(result);
                Assert.False(result.IsSuccessful);
                //Assert.Equal("There are too many items in the basket: not enough items in the shop", result.ErrorMessage);
                _httpClientServiceMock.Verify(x => x.SendAsync<CatalogItemResponse, GetByIdRequest>(It.IsAny<string>(), HttpMethod.Post, It.IsAny<GetByIdRequest>()), Times.Exactly(2));
                _httpClientServiceMock.Verify(x => x.SendAsync<SuccessfulResultResponse, UpdateAvailableStockRequest>(It.IsAny<string>(), HttpMethod.Post, It.IsAny<UpdateAvailableStockRequest>()), Times.Never);
            }

            [Fact]
            public async Task ReduceQuantityOfItemsAsync_ReturnsFailedResultResponse_WhenCommitReducingFails()
            {
                // Arrange
                var mockAppSettings = new AppSettings { OrderUrl = "https://example.com" };
                _mockSettings.Setup(x => x.Value).Returns(mockAppSettings);
                _httpClientServiceMock.SetupSequence(x => x.SendAsync<CatalogItemResponse, GetByIdRequest>(It.IsAny<string>(), HttpMethod.Post, It.IsAny<GetByIdRequest>()))
                    .ReturnsAsync(new CatalogItemResponse { Id = 1, AvailableStock = 2 })
                    .ReturnsAsync(new CatalogItemResponse { Id = 2, AvailableStock = 4 });

                _httpClientServiceMock.SetupSequence(x => x.SendAsync<SuccessfulResultResponse, UpdateAvailableStockRequest>(It.IsAny<string>(), HttpMethod.Post, It.IsAny<UpdateAvailableStockRequest>()))
                    .ReturnsAsync(new SuccessfulResultResponse { IsSuccessful = true })
                    .ReturnsAsync((SuccessfulResultResponse)null);

                // Act
                var result = await _catalogItemService.ReduceQuantityOfItemsAsync(_data);

                // Assert
                Assert.IsType<SuccessfulResultResponse>(result);
                Assert.False(result.IsSuccessful);
                Assert.Equal("Cannot commit reducing available stock", result.ErrorMessage);
                _httpClientServiceMock.Verify(x => x.SendAsync<CatalogItemResponse, GetByIdRequest>(It.IsAny<string>(), HttpMethod.Post, It.IsAny<GetByIdRequest>()), Times.Exactly(2));
                _httpClientServiceMock.Verify(x => x.SendAsync<SuccessfulResultResponse, UpdateAvailableStockRequest>(It.IsAny<string>(), HttpMethod.Post, It.IsAny<UpdateAvailableStockRequest>()), Times.Exactly(2));
            }
        }
    }
}
