using Basket.Host.Models.Dtos;
using Basket.Host.Services.Interfaces;
using Xunit;

namespace Basket.Host.Services.Tests
{
    public class KeyGeneratorServiceTests
    {
        private readonly KeyGeneratorService _keyGeneratorService;

        public KeyGeneratorServiceTests()
        {
            _keyGeneratorService = new KeyGeneratorService();
        }

        [Fact]
        public void GenerateKey_Should_ReturnValidKey()
        {
            // Arrange
            var user = new UserDto { UserId = 1, UserName = "John Doe" };
            string expectedKey = "JohnDoe1";

            // Act
            string result = _keyGeneratorService.GenerateKey(user);

            // Assert
            Assert.Equal(expectedKey, result);
        }

        [Fact]
        public void GenerateKey_Should_ReturnValidKey_WhenUserNameContainsSpaces()
        {
            // Arrange
            var user = new UserDto { UserId = 2, UserName = "Jane Smith" };
            string expectedKey = "JaneSmith2";

            // Act
            string result = _keyGeneratorService.GenerateKey(user);

            // Assert
            Assert.Equal(expectedKey, result);
        }
    }
}