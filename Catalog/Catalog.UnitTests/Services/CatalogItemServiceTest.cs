namespace Catalog.Host.UnitTests.Services
{
    public class CatalogItemServiceTest
    {
        private readonly CatalogItem _testItem1;
        private readonly CatalogItemDto _testItem1Dto;
        private readonly ICatalogItemService _catalogItemService;
        private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<Host.Services.Interfaces.IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<CatalogItemService>> _logger;

        public CatalogItemServiceTest()
        {
            _catalogItemRepository = new Mock<ICatalogItemRepository>();
            _mapper = new Mock<IMapper>();
            _dbContextWrapper = new Mock<Host.Services.Interfaces.IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<CatalogItemService>>();
            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(dbContextTransaction.Object);
            _catalogItemService = new CatalogItemService(_dbContextWrapper.Object, _logger.Object, _catalogItemRepository.Object, _mapper.Object);
            _testItem1 = new ()
            {
                Id = 1,
                Name = "A",
                Description = "dA",
                AvailableStock = 10,
                PictureFileName = "dsf",
                CatalogBrandId = 1,
                CatalogTypeId = 1,
                Price = 10,
            };
            _testItem1Dto = new ()
            {
                Id = 1,
                Name = "A",
                Description = "dA",
                AvailableStock = 10,
                PictureUrl = "dsf",
                CatalogBrandId = 1,
                CatalogTypeId = 1,
                Price = 10,
            };
        }

        [Fact]
        public async Task UpdateAsync_Success()
        {
            // arrange
            _catalogItemRepository.Setup(s => s.UpdateAsync(
                1,
                _testItem1)).ReturnsAsync(true);

            _mapper.Setup(s => s.Map<CatalogItem>(
               It.Is<CatalogItemDto>(i => i.Equals(_testItem1Dto)))).Returns(_testItem1);

            // act
            var result = await _catalogItemService.UpdateAsync(_testItem1.Id, _testItem1Dto);

            // assert
            result.Should().Be(true);
        }

        [Fact]
        public async Task UpdateAsync_Failed()
        {
            // arrange
            _catalogItemRepository.Setup(s => s.UpdateAsync(
                1,
                _testItem1)).ReturnsAsync(false);

            _mapper.Setup(s => s.Map<CatalogItem>(
               It.Is<CatalogItemDto>(i => i.Equals(_testItem1Dto)))).Returns(_testItem1);

            // act
            var result = await _catalogItemService.UpdateAsync(_testItem1.Id, _testItem1Dto);

            // assert
            result.Should().Be(false);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            // arrange
            _catalogItemRepository.Setup(s => s.AddAsync(
                _testItem1)).ReturnsAsync(_testItem1.Id);

            _mapper.Setup(s => s.Map<CatalogItem>(
               It.Is<CatalogItemDto>(i => i.Equals(_testItem1Dto)))).Returns(_testItem1);

            // act
            var result = await _catalogItemService.AddAsync(_testItem1Dto);

            // assert
            result.Should().Be(_testItem1.Id);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            // arrange
            _catalogItemRepository.Setup(s => s.DeleteAsync(
                _testItem1)).ReturnsAsync(true);

            _mapper.Setup(s => s.Map<CatalogItem>(
               It.Is<CatalogItemDto>(i => i.Equals(_testItem1Dto)))).Returns(_testItem1);

            // act
            var result = await _catalogItemService.DeleteAsync(_testItem1Dto);

            // assert
            result.Should().Be(true);
        }

        [Fact]
        public async Task DeleteAsync_Failed()
        {
            // arrange
            _catalogItemRepository.Setup(s => s.DeleteAsync(
                _testItem1)).ReturnsAsync(false);

            _mapper.Setup(s => s.Map<CatalogItem>(
               It.Is<CatalogItemDto>(i => i.Equals(_testItem1Dto)))).Returns(_testItem1);

            // act
            var result = await _catalogItemService.DeleteAsync(_testItem1Dto);

            // assert
            result.Should().Be(false);
        }
    }
}
