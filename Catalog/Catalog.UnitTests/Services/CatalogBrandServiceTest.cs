using System.Threading;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using FluentAssertions;

namespace Catalog.Host.UnitTests.Services
{
    public class CatalogBrandServiceTest
    {
        private readonly CatalogBrand _testBrand1;
        private readonly CatalogBrand _testBrand2;
        private readonly CatalogBrandDto _testBrand1Dto;
        private readonly CatalogBrandDto _testBrand2Dto;
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
            _testBrand1 = new ()
            {
                Id = 1,
                Brand = "Test",
            };
            _testBrand2 = new ()
            {
                Id = 2,
                Brand = "Test2",
            };
            _testBrand1Dto = new ()
            {
                Id = 1,
                Brand = "Test",
            };
            _testBrand2Dto = new ()
            {
                Id = 2,
                Brand = "Test2",
            };
        }

        [Fact]
        public async Task UpdateAsync_Success()
        {
            // arrange
            _catalogBrandRepository.Setup(s => s.UpdateAsync(
                1,
                _testBrand1)).ReturnsAsync(true);

            _mapper.Setup(s => s.Map<CatalogBrandDto>(
               It.Is<CatalogBrand>(i => i.Equals(_testBrand1)))).Returns(_testBrand1Dto);

            // act
            var result = await _catalogBrandService.UpdateAsync(_testBrand1.Id, _testBrand1Dto);

            // assert
            result.Should().Be(true);
        }

        [Fact]
        public async Task UpdateAsync_Failed()
        {
            // arrange
            _catalogBrandRepository.Setup(s => s.UpdateAsync(
                1,
                _testBrand1)).ReturnsAsync(false);

            _mapper.Setup(s => s.Map<CatalogBrandDto>(
               It.Is<CatalogBrand>(i => i.Equals(_testBrand1)))).Returns(_testBrand1Dto);

            // act
            var result = await _catalogBrandService.UpdateAsync(_testBrand1.Id, _testBrand1Dto);

            // assert
            result.Should().Be(false);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            // arrange
            _catalogBrandRepository.Setup(s => s.AddAsync(
                _testBrand1)).ReturnsAsync(_testBrand1.Id);

            _mapper.Setup(s => s.Map<CatalogBrandDto>(
               It.Is<CatalogBrand>(i => i.Equals(_testBrand1)))).Returns(_testBrand1Dto);

            // act
            var result = await _catalogBrandService.AddAsync(_testBrand1Dto);

            // assert
            result.Should().Be(_testBrand1.Id);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            // arrange
            _catalogBrandRepository.Setup(s => s.DeleteAsync(
                _testBrand1)).ReturnsAsync(true);

            _mapper.Setup(s => s.Map<CatalogBrandDto>(
               It.Is<CatalogBrand>(i => i.Equals(_testBrand1)))).Returns(_testBrand1Dto);

            // act
            var result = await _catalogBrandService.DeleteAsync(_testBrand1Dto);

            // assert
            result.Should().Be(true);
        }

        [Fact]
        public async Task DeleteAsync_Failed()
        {
            // arrange
            _catalogBrandRepository.Setup(s => s.DeleteAsync(
                _testBrand1)).ReturnsAsync(false);

            _mapper.Setup(s => s.Map<CatalogBrandDto>(
               It.Is<CatalogBrand>(i => i.Equals(_testBrand1)))).Returns(_testBrand1Dto);

            // act
            var result = await _catalogBrandService.DeleteAsync(_testBrand1Dto);

            // assert
            result.Should().Be(false);
        }
    }
}
