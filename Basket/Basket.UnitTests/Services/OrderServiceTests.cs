using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Basket.Host;
using Basket.Host.Models.Dtos;
using Basket.Host.Models.Requests;
using Basket.Host.Services;
using Basket.Host.Services.Interfaces;
using Infrastructure.Models.Responses;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Basket.UnitTests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IOptions<AppSettings>> _mockSettings;
        private readonly Mock<IInternalHttpClientService> _mockHttpClient;
        private readonly Mock<ILogger<OrderService<CatalogItemDto>>> _mockLogger;

        public OrderServiceTests()
        {
            _mockSettings = new Mock<IOptions<AppSettings>>();
            _mockHttpClient = new Mock<IInternalHttpClientService>();
            _mockLogger = new Mock<ILogger<OrderService<CatalogItemDto>>>();
        }

        [Fact]
        public async Task CommitPurchases_Should_SendRequestAndReturnResult()
        {
            // Arrange
            var mockAppSettings = new AppSettings { OrderUrl = "https://example.com" };
            _mockSettings.Setup(x => x.Value).Returns(mockAppSettings);
            var orderService = new OrderService<CatalogItemDto>(_mockSettings.Object, _mockHttpClient.Object, _mockLogger.Object);
            var user = new UserDto { UserId = 1 };
            var orders = new List<CatalogItemDto> { new CatalogItemDto() }; // Replace with appropriate test data
            var request = new OrderDto<CatalogItemDto> { User = user, Orders = orders };
            var expectedResult = new SuccessfulResultResponse { IsSuccessful = true };

            _mockHttpClient.Setup(mock => mock.SendAsync<SuccessfulResultResponse, PurchaseRequest<CatalogItemDto>>(
                It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<PurchaseRequest<CatalogItemDto>>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await orderService.CommitPurchases(request);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockHttpClient.Verify(
                mock => mock.SendAsync<SuccessfulResultResponse, PurchaseRequest<CatalogItemDto>>(
                It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<PurchaseRequest<CatalogItemDto>>()), Times.Once);
        }

        [Fact]
        public async Task CommitPurchases_Should_LogErrorAndReturnErrorResponse_WhenExceptionOccurs()
        {
            // Arrange
            var mockAppSettings = new AppSettings { OrderUrl = "https://example.com" };
            _mockSettings.Setup(x => x.Value).Returns(mockAppSettings);
            var orderService = new OrderService<CatalogItemDto>(_mockSettings.Object, _mockHttpClient.Object, _mockLogger.Object);
            var user = new UserDto { UserId = 1 };
            var orders = new List<CatalogItemDto> { new CatalogItemDto() }; // Replace with appropriate test data
            var request = new OrderDto<CatalogItemDto> { User = user, Orders = orders };
            var expectedErrorMessage = "An error occurred.";
            var expectedErrorResponse = new SuccessfulResultResponse { IsSuccessful = false, ErrorMessage = expectedErrorMessage };

            _mockHttpClient.Setup(mock => mock.SendAsync<SuccessfulResultResponse, PurchaseRequest<CatalogItemDto>>(
                It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<PurchaseRequest<CatalogItemDto>>()))
                .ThrowsAsync(new Exception(expectedErrorMessage));

            // Act
            var result = await orderService.CommitPurchases(request);

            // Assert
            Assert.Equal(expectedErrorResponse.IsSuccessful, result.IsSuccessful);
            Assert.Equal(expectedErrorResponse.ErrorMessage, result.ErrorMessage);
        }
    }
}