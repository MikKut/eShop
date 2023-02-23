namespace Catalog.Host.UnitTests.Services
{
    public class CatalogTypeServiceTest
    {
        private readonly CatalogType _testType1;
        private readonly CatalogType _testType2;
        private readonly CatalogTypeDto _testType1Dto;
        private readonly CatalogTypeDto _testType2Dto;
        private readonly ICatalogTypeService _catalogTypeService;
        private readonly Mock<ICatalogTypeRepository> _catalogTypeRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<Host.Services.Interfaces.IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<CatalogTypeService>> _logger;

        public CatalogTypeServiceTest()
        {
            _catalogTypeRepository = new Mock<ICatalogTypeRepository>();
            _mapper = new Mock<IMapper>();
            _dbContextWrapper = new Mock<Host.Services.Interfaces.IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<CatalogTypeService>>();
            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(dbContextTransaction.Object);
            _catalogTypeService = new CatalogTypeService(_dbContextWrapper.Object, _logger.Object, _catalogTypeRepository.Object, _mapper.Object);
            _testType1 = new ()
            {
                Id = 1,
                Type = "Test",
            };
            _testType2 = new ()
            {
                Id = 2,
                Type = "Test2",
            };
            _testType1Dto = new ()
            {
                Id = 1,
                Type = "Test",
            };
            _testType2Dto = new ()
            {
                Id = 2,
                Type = "Test2",
            };
        }

        [Fact]
        public async Task UpdateAsync_Success()
        {
            // arrange
            _catalogTypeRepository.Setup(s => s.UpdateAsync(
                1,
                _testType1)).ReturnsAsync(true);

            _mapper.Setup(s => s.Map<CatalogType>(
               It.Is<CatalogTypeDto>(i => i.Equals(_testType1Dto)))).Returns(_testType1);

            // act
            var result = await _catalogTypeService.UpdateAsync(_testType1.Id, _testType1Dto);

            // assert
            result.Should().Be(true);
        }

        [Fact]
        public async Task UpdateAsync_Failed()
        {
            // arrange
            _catalogTypeRepository.Setup(s => s.UpdateAsync(
                1,
                _testType1)).ReturnsAsync(false);

            _mapper.Setup(s => s.Map<CatalogType>(
               It.Is<CatalogTypeDto>(i => i.Equals(_testType1Dto)))).Returns(_testType1);

            // act
            var result = await _catalogTypeService.UpdateAsync(_testType1.Id, _testType1Dto);

            // assert
            result.Should().Be(false);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            // arrange
            _catalogTypeRepository.Setup(s => s.AddAsync(
                _testType1)).ReturnsAsync(_testType1.Id);

            _mapper.Setup(s => s.Map<CatalogType>(
               It.Is<CatalogTypeDto>(i => i.Equals(_testType1Dto)))).Returns(_testType1);

            // act
            var result = await _catalogTypeService.AddAsync(_testType1Dto);

            // assert
            result.Should().Be(_testType1.Id);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            // arrange
            _catalogTypeRepository.Setup(s => s.DeleteAsync(
                _testType1)).ReturnsAsync(true);

            _mapper.Setup(s => s.Map<CatalogType>(
               It.Is<CatalogTypeDto>(i => i.Equals(_testType1Dto)))).Returns(_testType1);

            // act
            var result = await _catalogTypeService.DeleteAsync(_testType1Dto);

            // assert
            result.Should().Be(true);
        }

        [Fact]
        public async Task DeleteAsync_Failed()
        {
            // arrange
            _catalogTypeRepository.Setup(s => s.DeleteAsync(
                _testType1)).ReturnsAsync(false);

            _mapper.Setup(s => s.Map<CatalogType>(
               It.Is<CatalogTypeDto>(i => i.Equals(_testType1Dto)))).Returns(_testType1);

            // act
            var result = await _catalogTypeService.DeleteAsync(_testType1Dto);

            // assert
            result.Should().Be(false);
        }
    }
}
