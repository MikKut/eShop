using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using global::Order.Host.Models.Dtos;
    using global::Order.Host.Models.Requests;
    using global::Order.Host.Services;
    using global::Order.Host.Services.Interfaces;
    using Infrastructure.Models.Responses;
    using Moq;
    using Xunit;
    using Xunit.Sdk;

    namespace Order.UnitTests.Services
    {
        public class OrderServiceTests
        {
            private readonly Mock<IPaymentService> _paymentServiceMock;
            private readonly Mock<ICatalogItemService> _catalogItemServiceMock;
            private readonly OrderService _orderService;

            public OrderServiceTests()
            {
                _paymentServiceMock = new Mock<IPaymentService>();
                _catalogItemServiceMock = new Mock<ICatalogItemService>();
                _orderService = new OrderService(_paymentServiceMock.Object, _catalogItemServiceMock.Object);
            }

            [Fact]
            public async Task HandlePurchase_ReturnsSuccessfulResultResponse_WhenTransactionAndCatalogItemUpdatesAreSuccessful()
            {
                // Arrange
                var resultResonse = new SuccessfulResultResponse() { IsSuccessful = true };
                var request = new PurchaseRequest<CatalogItemDto>
                {
                    ID = 3,
                    Data = new List<CatalogItemDto>
                {
                    new CatalogItemDto { Id = 1, Price = 10 },
                    new CatalogItemDto { Id = 1, Price = 20 }
                }
                };
                var totalCost = request.Data.Sum(x => x.Price);

                _paymentServiceMock.Setup(x => x.CheckTrasactionForAvailabilityForUser(request.ID, totalCost))
                    .ReturnsAsync(resultResonse);
                _catalogItemServiceMock.Setup(x => x.ReduceQuantityOfItemsAsync(request.Data))
                    .ReturnsAsync(resultResonse);
                _paymentServiceMock.Setup(x => x.CommitTrasactionForTheUser(request.ID, totalCost))
                    .ReturnsAsync(resultResonse);

                // Act
                var result = await _orderService.HandlePurchase(request);

                // Assert
                Assert.IsType<SuccessfulResultResponse>(result);
                Assert.True(result.IsSuccessful);
            }

            [Fact]
            public async Task HandlePurchase_ReturnsFailedResultResponse_WhenTransactionAvailabilityCheckFails()
            {
                // Arrange
                string errorMessage = "Test error";
                var resultResonse = new SuccessfulResultResponse() { IsSuccessful = false, ErrorMessage = errorMessage };
                var request = new PurchaseRequest<CatalogItemDto>
                {
                    ID = 3,
                    Data = new List<CatalogItemDto>
                {
                    new CatalogItemDto { Id = 1, Price = 10 },
                    new CatalogItemDto { Id = 1, Price = 20 }
                }
                };
                var totalCost = request.Data.Sum(x => x.Price);

                _paymentServiceMock.Setup(x => x.CheckTrasactionForAvailabilityForUser(request.ID, totalCost))
                    .ReturnsAsync(resultResonse);

                // Act
                var result = await _orderService.HandlePurchase(request);

                // Assert
                Assert.IsType<SuccessfulResultResponse>(result);
                Assert.False(result.IsSuccessful);
                Assert.Equal(errorMessage, result.ErrorMessage);
            }

            [Fact]
            public async Task HandlePurchase_ReturnsFailedResultResponse_WhenCatalogItemReductionFails()
            {
                // Arrange
                string errorMessage = "Test error";
                var failedResultResonse = new SuccessfulResultResponse() { IsSuccessful = false, ErrorMessage = errorMessage };
                var successfulResultResponse = new SuccessfulResultResponse() { IsSuccessful = true };
                var request = new PurchaseRequest<CatalogItemDto>
                {
                    ID = 3,
                    Data = new List<CatalogItemDto>
                {
                    new CatalogItemDto { Id = 1, Price = 10 },
                    new CatalogItemDto { Id = 1, Price = 20 }
                }
                };
                var totalCost = request.Data.Sum(x => x.Price);

                _paymentServiceMock.Setup(x => x.CheckTrasactionForAvailabilityForUser(request.ID, totalCost))
                    .ReturnsAsync(successfulResultResponse);
                _catalogItemServiceMock.Setup(x => x.ReduceQuantityOfItemsAsync(request.Data))
                    .ReturnsAsync(failedResultResonse);

                // Act
                var result = await _orderService.HandlePurchase(request);

                // Assert
                Assert.IsType<SuccessfulResultResponse>(result);
                Assert.False(result.IsSuccessful);
                Assert.Equal(errorMessage, result.ErrorMessage);
            }

            [Fact]
            public async Task HandlePurchase_ReturnsFailedResultResponse_WhenTransactionCommitFails()
            {
                // Arrange
                string errorMessage = "Test error";
                var failedResultResonse = new SuccessfulResultResponse() { IsSuccessful = false, ErrorMessage = errorMessage };
                var successfulResultResponse = new SuccessfulResultResponse() { IsSuccessful = true };
                var request = new PurchaseRequest<CatalogItemDto>
                {
                    ID = 3,
                    Data = new List<CatalogItemDto>
                {
                    new CatalogItemDto { Id = 1, Price = 10 },
                    new CatalogItemDto { Id = 1, Price = 20 }
                }
                };
                var totalCost = request.Data.Sum(x => x.Price);

                _paymentServiceMock.Setup(x => x.CheckTrasactionForAvailabilityForUser(request.ID, totalCost))
                    .ReturnsAsync(successfulResultResponse);
                _catalogItemServiceMock.Setup(x => x.ReduceQuantityOfItemsAsync(request.Data))
                    .ReturnsAsync(successfulResultResponse);
                _paymentServiceMock.Setup(x => x.CommitTrasactionForTheUser(request.ID, totalCost))
                    .ReturnsAsync(failedResultResonse);

                // Act
                var result = await _orderService.HandlePurchase(request);

                // Assert
                Assert.IsType<SuccessfulResultResponse>(result);
                Assert.False(result.IsSuccessful);
                Assert.Equal(errorMessage, result.ErrorMessage);
            }
        }
    }
}
