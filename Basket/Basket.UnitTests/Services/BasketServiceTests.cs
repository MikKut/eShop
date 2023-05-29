using System.Collections.Generic;
using Basket.Host.Models.Dtos;
using Basket.Host.Services.Interfaces;
using Xunit;
using Moq;

namespace Basket.Host.Services.Tests
{
    public class BasketServiceTests
    {
        private readonly Mock<ICacheService> _mockCacheService;
        private readonly Mock<IKeyGeneratorService> _mockKeyGeneratorService;
        private readonly BasketService _basketService;

        public BasketServiceTests()
        {
            _mockCacheService = new Mock<ICacheService>();
            _mockKeyGeneratorService = new Mock<IKeyGeneratorService>();
            _basketService = new BasketService(_mockCacheService.Object, _mockKeyGeneratorService.Object);
        }

        [Fact]
        public async Task AddItems_Should_AddDataToCache()
        {
            // Arrange
            var user = new UserDto { UserId = 1, UserName = "John" };
            var orderDto = new OrderDto<string> { User = user, Orders = new List<string> { "Product1", "Product2" } };

            string expectedKey = "key";
            _mockKeyGeneratorService.Setup(k => k.GenerateKey(user)).Returns(expectedKey);

            // Act
            await _basketService.AddItems(orderDto);

            // Assert
            _mockCacheService.Verify(c => c.AddOrUpdateAsync(expectedKey, orderDto.Orders), Times.Once);
        }

        [Fact]
        public async Task GetItems_Should_ReturnBasketDtoFromCache()
        {
            // Arrange
            var user = new UserDto { UserId = 1, UserName = "John" };

            string expectedKey = "key";
            _mockKeyGeneratorService.Setup(k => k.GenerateKey(user)).Returns(expectedKey);

            var cachedData = new List<CatalogItemDto> { new CatalogItemDto { Id = 1, Name = "Product1" } };
            _mockCacheService.Setup(c => c.GetAsync<List<CatalogItemDto>>(expectedKey)).ReturnsAsync(cachedData);

            // Act
            var result = await _basketService.GetItems(user);

            // Assert
            _mockCacheService.Verify(c => c.GetAsync<List<CatalogItemDto>>(expectedKey), Times.Once);
            Assert.Equal(cachedData, result.Data);
        }

        [Fact]
        public async Task GetItems_WithUserIdAndUserName_Should_ReturnBasketDtoFromCache()
        {
            // Arrange
            int userId = 1;
            string userName = "John";

            string expectedKey = "key";
            _mockKeyGeneratorService.Setup(k => k.GenerateKey(It.IsAny<UserDto>())).Returns(expectedKey);

            var cachedData = new List<CatalogItemDto> { new CatalogItemDto { Id = 1, Name = "Product1" } };
            _mockCacheService.Setup(c => c.GetAsync<List<CatalogItemDto>>(expectedKey)).ReturnsAsync(cachedData);

            // Act
            var result = await _basketService.GetItems(userId, userName);

            // Assert
            _mockCacheService.Verify(c => c.GetAsync<List<CatalogItemDto>>(expectedKey), Times.Once);
            Assert.Equal(cachedData, result.Data);
        }

        [Fact]
        public async Task CleanCurrentBasket_Should_ClearCache()
        {
            // Arrange
            var user = new UserDto { UserId = 1, UserName = "John" };

            string expectedKey = "key";
            _mockKeyGeneratorService.Setup(k => k.GenerateKey(user)).Returns(expectedKey);

            // Act
            await _basketService.CleanCurrentBasket(user);

            // Assert
            _mockCacheService.Verify(c => c.ClearCacheByKeyAsync(expectedKey), Times.Once);
        }
    }
}