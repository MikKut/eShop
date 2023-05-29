using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Repositories;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Catalog.Host.UnitTests.Services
{
    public class CatalogBrandServiceTest
    {
        private readonly CatalogBrand _testBrand;
        private readonly CatalogBrandDto _testBrandDto;
        private readonly ICatalogBrandService _catalogBrandService;
        private readonly Mock<ICatalogBrandRepository> _catalogBrandRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<Host.Services.Interfaces.IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<CatalogBrandService>> _logger;

        public CatalogBrandServiceTest()
        {
            _catalogBrandRepository = new Mock<ICatalogBrandRepository>();
            _mapper = new Mock<IMapper>();
            _dbContextWrapper = new Mock<Host.Services.Interfaces.IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<CatalogBrandService>>();
            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(dbContextTransaction.Object);
            _catalogBrandService = new CatalogBrandService(_dbContextWrapper.Object, _logger.Object, _catalogBrandRepository.Object, _mapper.Object);
            _testBrand = new CatalogBrand
            {
                Id = 1,
                Brand = "Test"
            };
            _testBrandDto = new CatalogBrandDto
            {
                Id = 1,
                Brand = "Test"
            };
        }

        [Fact]
        public async Task AddAsync_ShouldReturnId_WhenSuccessfullyAdded()
        {
            // Arrange
            _catalogBrandRepository.Setup(s => s.AddAsync(It.IsAny<CatalogBrand>())).ReturnsAsync(_testBrand.Id);
            _mapper.Setup(s => s.Map<CatalogBrandDto>(_testBrand)).Returns(_testBrandDto);

            // Act
            var result = await _catalogBrandService.AddAsync(_testBrandDto);

            // Assert
            result.Should().Be(_testBrand.Id);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnNull_WhenAddingFails()
        {
            // Arrange
            // _catalogBrandRepository.Setup(s => s.AddAsync(_testBrand)).ReturnsAsync(null);
            _catalogBrandRepository.Setup(s => s.AddAsync(_testBrand)).ReturnsAsync((int?)null);
            _mapper.Setup(s => s.Map<CatalogBrandDto>(_testBrand)).Returns(_testBrandDto);

            // Act
            var result = await _catalogBrandService.AddAsync(_testBrandDto);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenSuccessfullyUpdated()
        {
            // Arrange
            _catalogBrandRepository.Setup(s => s.UpdateAsync(_testBrand.Id, It.IsAny<CatalogBrand>())).ReturnsAsync(true);
            _mapper.Setup(s => s.Map<CatalogBrandDto>(_testBrand)).Returns(_testBrandDto);

            // Act
            var result = await _catalogBrandService.UpdateAsync(_testBrand.Id, _testBrandDto);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenUpdateFails()
        {
            // Arrange
            _catalogBrandRepository.Setup(s => s.UpdateAsync(_testBrand.Id, _testBrand)).ReturnsAsync(false);
            _mapper.Setup(s => s.Map<CatalogBrandDto>(_testBrand)).Returns(_testBrandDto);

            // Act
            var result = await _catalogBrandService.UpdateAsync(_testBrand.Id, _testBrandDto);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenSuccessfullyDeleted()
        {
            // Arrange
            _mapper.Setup(s => s.Map<CatalogBrandDto>(_testBrand)).Returns(_testBrandDto);
            _catalogBrandRepository.Setup(s => s.DeleteAsync(It.IsAny<CatalogBrand>())).ReturnsAsync(true);

            // Act
            var result = await _catalogBrandService.DeleteAsync(_testBrandDto);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenDeleteFails()
        {
            // Arrange
            _catalogBrandRepository.Setup(s => s.DeleteAsync(_testBrand)).ReturnsAsync(false);
            _mapper.Setup(s => s.Map<CatalogBrandDto>(_testBrand)).Returns(_testBrandDto);

            // Act
            var result = await _catalogBrandService.DeleteAsync(_testBrandDto);

            // Assert
            result.Should().BeFalse();
        }
    }
}