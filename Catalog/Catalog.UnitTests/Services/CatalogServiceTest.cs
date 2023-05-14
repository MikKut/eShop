using System.Threading;
using Catalog.Host.Data.Entities;
using Catalog.Host.Extensions;
using Catalog.Host.Models.Dtos;
using FluentAssertions;

namespace Catalog.UnitTests.Services;

public class CatalogServiceTest
{
    private readonly ICatalogService _catalogService;

    private readonly Mock<ICatalogRepository> _catalogRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<Host.Services.Interfaces.IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogService>> _logger;
    private readonly CatalogItem _testExampleCatalogItem;
    private readonly CatalogItemDto _testExampleCatalogItemDto;

    public CatalogServiceTest()
    {
        _catalogRepository = new Mock<ICatalogRepository>();
        _mapper = new Mock<IMapper>();
        _dbContextWrapper = new Mock<Host.Services.Interfaces.IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogService>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(dbContextTransaction.Object);

        _catalogService = new CatalogService(_dbContextWrapper.Object, _logger.Object, _catalogRepository.Object, _mapper.Object);
        _testExampleCatalogItem = new CatalogItem() { Name = "TestName", Description = "TestDescription", AvailableStock = 10, PictureFileName = "TestName", Price = 10, Id = 1, CatalogBrandId = 1, CatalogTypeId = 1 };
        _testExampleCatalogItemDto = new CatalogItemDto() { Name = "TestName", Description = "TestDescription", AvailableStock = 10, PictureUrl = "TestName", Price = 10, Id = 1, CatalogBrandId = 1, CatalogTypeId = 1 };
    }

    [Fact]
    public async Task GetByIdAsync_Success()
    {
        // arrange
        _catalogRepository.Setup(c => c.GetByIDAsync(It.IsAny<int>())).ReturnsAsync(_testExampleCatalogItem);
        _mapper.Setup(s => s.Map<CatalogItemDto>(
              It.Is<CatalogItem>(i => i.Equals(_testExampleCatalogItem)))).Returns(_testExampleCatalogItemDto);

        // act
        var result = await _catalogService.GetByIDAsync(_testExampleCatalogItemDto.Id);

        // assert
        result.Should().NotBeNull();
        result.Should().Be(_testExampleCatalogItemDto);
    }

    [Fact]
    public async Task GetByIdAsync_WrondIdReturnsNull()
    {
        // arrange
        _catalogRepository.Setup(c => c.GetByIDAsync(It.IsAny<int>())).ReturnsAsync(default(CatalogItem));
        _mapper.Setup(s => s.Map<CatalogItemDto?>(
              It.Is<CatalogItem>(i => i.Equals(_testExampleCatalogItem)))).Returns(default(CatalogItemDto));

        // act
        var result = await _catalogService.GetByIDAsync(_testExampleCatalogItemDto.Id);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Success()
    {
        // arrange
        var testPageIndex = 0;
        var testPageSize = 4;
        var testTotalCount = 12;

        var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                new CatalogItem()
                {
                    Name = "TestName",
                },
            },
            TotalCount = testTotalCount,
        };

        var catalogItemSuccess = new CatalogItem()
        {
            Name = "TestName"
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Name = "TestName"
        };

        _catalogRepository.Setup(s => s.GetByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize),
            null))
            .ReturnsAsync(pagingPaginatedItemsSuccess);

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess))))
            .Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex, null);

        // assert
        Assert.NotNull(result);
        Assert.NotNull(result?.Data);
        Assert.Equal(result?.Count, testTotalCount);
        Assert.Equal(result?.PageSize, testPageSize);
        Assert.Equal(result?.PageIndex, testPageIndex);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;
        PaginatedItems<CatalogItem> item = null!;

        _catalogRepository.Setup(s => s.GetByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize),
            null))
            .ReturnsAsync(item);

        // act
        var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex, null);

        // assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByBrandAsync_WrondIdReturnsNull()
    {
        // arrange
        _catalogRepository.Setup(c => c.GetByBrandAsync(It.IsAny<int>()))
            .ReturnsAsync(default(CatalogItem));
        _mapper.Setup(s => s.Map<CatalogItemDto?>(
              It.Is<CatalogItem>(i => i.Equals(_testExampleCatalogItem))))
            .Returns(default(CatalogItemDto));

        // act
        var result = await _catalogService.GetByBrandAsync(_testExampleCatalogItemDto.Id);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByBrandAsync_Success()
    {
        // arrange
        _catalogRepository.Setup(c => c.GetByBrandAsync(It.IsAny<int>()))
            .ReturnsAsync(_testExampleCatalogItem);
        _mapper.Setup(s => s.Map<CatalogItemDto>(
              It.Is<CatalogItem>(i => i.Equals(_testExampleCatalogItem))))
            .Returns(_testExampleCatalogItemDto);

        // act
        var result = await _catalogService.GetByBrandAsync(_testExampleCatalogItemDto.Id);

        // assert
        result.Should().NotBeNull();
        result.Should().Be(_testExampleCatalogItemDto);
    }

    [Fact]
    public async Task GetByTypeAsync_WrondIdReturnsNull()
    {
        // arrange
        _catalogRepository.Setup(c => c.GetByTypeAsync(It.IsAny<int>()))
            .ReturnsAsync(default(CatalogItem));
        _mapper.Setup(s => s.Map<CatalogItemDto?>(
              It.Is<CatalogItem>(i => i.Equals(_testExampleCatalogItem))))
            .Returns(default(CatalogItemDto));

        // act
        var result = await _catalogService.GetByTypeAsync(_testExampleCatalogItemDto.Id);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTypesAsync_Success()
    {
        // arrange
        var catalogType1 = new CatalogType() { Id = 1, Type = "dsf" };
        var catalogType1Dto = new CatalogTypeDto() { Id = 1, Type = "dsf" };
        var catalogType2 = new CatalogType() { Id = 2, Type = "fdsf" };
        var catalogType2Dto = new CatalogTypeDto() { Id = 2, Type = "fdsf" };
        var resListRepos = new List<CatalogType>() { catalogType1, catalogType2 };
        var resListTotalMethod = new List<CatalogTypeDto?>() { catalogType1Dto, catalogType2Dto };
        _catalogRepository.Setup(c => c.GetTypesAsync())
            .ReturnsAsync(resListRepos);
        _mapper.Setup(s => s.Map<CatalogTypeDto>(
              It.Is<CatalogType>(i => i.Equal(catalogType2))))
            .Returns(catalogType2Dto);
        _mapper.Setup(s => s.Map<CatalogTypeDto>(
              It.Is<CatalogType>(i => i.Equal(catalogType1))))
            .Returns(catalogType1Dto);

        // act
        var result = await _catalogService.GetTypesAsync();

        // assert
        result.Should().NotBeNull();
        result.Count.Should().Be(resListRepos.Count);
        result.Count.Should().Be(resListTotalMethod.Count);
        result.Should().BeEquivalentTo(resListTotalMethod);
    }

    [Fact]
    public async Task GetTypesAsync_WrondIdReturnsEmptyList()
    {
        // arrange
        var emptyList = new List<CatalogType>();
        _catalogRepository.Setup(c => c.GetTypesAsync()).ReturnsAsync(emptyList);
        _mapper.Setup(s => s.Map<CatalogTypeDto?>(
              It.Is<CatalogType>(i => i.Equals(default(CatalogType))))).Returns(default(CatalogTypeDto));

        // act
        var result = await _catalogService.GetTypesAsync();

        // assert
        result.Should().NotBeNull();
        result.Count.Should().Be(0);
        result.Should().BeEquivalentTo(emptyList);
    }

    [Fact]
    public async Task GetBrandsAsync_Success()
    {
        // arrange
        var catalogBrand1 = new CatalogBrand() { Id = 1, Brand = "dsf" };
        var catalogBrand1Dto = new CatalogBrandDto() { Id = 1, Brand = "dsf" };
        var catalogBrand2 = new CatalogBrand() { Id = 2, Brand = "fdsf" };
        var catalogBrand2Dto = new CatalogBrandDto() { Id = 2, Brand = "fdsf" };
        var resListRepos = new List<CatalogBrand>() { catalogBrand1, catalogBrand2 };
        var resListTotalMethod = new List<CatalogBrandDto?>() { catalogBrand1Dto, catalogBrand2Dto };
        _catalogRepository.Setup(c => c.GetBrandsAsync())
            .ReturnsAsync(resListRepos);
        _mapper.Setup(s => s.Map<CatalogBrandDto>(
              It.Is<CatalogBrand>(i => i.Equal(catalogBrand2))))
            .Returns(catalogBrand2Dto);
        _mapper.Setup(s => s.Map<CatalogBrandDto>(
              It.Is<CatalogBrand>(i => i.Equal(catalogBrand1))))
            .Returns(catalogBrand1Dto);

        // act
        var result = await _catalogService.GetBrandsAsync();

        // assert
        result.Should().NotBeNull();
        result.Count.Should().Be(resListRepos.Count);
        result.Count.Should().Be(resListTotalMethod.Count);
        result.Should().BeEquivalentTo(resListTotalMethod);
    }

    [Fact]
    public async Task GetBrandsAsync_WrondIdReturnsEmptyList()
    {
        // arrange
        var emptyList = new List<CatalogBrand>();
        _catalogRepository.Setup(c => c.GetBrandsAsync()).ReturnsAsync(emptyList);
        _mapper.Setup(s => s.Map<CatalogBrandDto?>(
              It.Is<CatalogBrand>(i => i.Equals(default(CatalogBrand))))).Returns(default(CatalogBrandDto));

        // act
        var result = await _catalogService.GetBrandsAsync();

        // assert
        result.Should().NotBeNull();
        result.Count.Should().Be(0);
        result.Should().BeEquivalentTo(emptyList);
    }
}