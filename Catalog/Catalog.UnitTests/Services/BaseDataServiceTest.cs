using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Services
{
    public class BaseDataServiceTest
    {
        private readonly Mock<Host.Services.Interfaces.IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<BaseDataService<ApplicationDbContext>>> _logger;
        private readonly TestDataService _dataService;
        private readonly Mock<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction> _dbContextTransactionMock;

        public BaseDataServiceTest()
        {
            _dbContextTransactionMock = new Mock<IDbContextTransaction>();
            _dbContextWrapper = new Mock<Host.Services.Interfaces.IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<BaseDataService<ApplicationDbContext>>>();
            _dataService = new TestDataService(_dbContextWrapper.Object, _logger.Object);
        }

        [Fact]
        public async Task ExecuteSafeAsync_ExecutesActionAndCommitsTransaction()
        {
            // Arrange
            var executed = false;
            var cancellationToken = CancellationToken.None;

            Task CheckAction()
            {
                executed = true;
                return Task.CompletedTask;
            }

            _dbContextWrapper
                .Setup(s => s.BeginTransactionAsync(cancellationToken))
                .ReturnsAsync(_dbContextTransactionMock.Object);

            // Act
            await _dataService.ExecuteSafeAsync(CheckAction, cancellationToken);

            // Assert
            Assert.True(executed);
            _dbContextTransactionMock.Verify(t => t.CommitAsync(cancellationToken), Times.Once);
            _dbContextWrapper.Verify(s => s.BeginTransactionAsync(cancellationToken), Times.Once);
            _dbContextTransactionMock.Verify(s => s.RollbackAsync(cancellationToken), Times.Never);
        }

        [Fact]
        public async Task ExecuteSafeAsync_CatchesExceptionAndRollsBackTransaction()
        {
            // Arrange
            var exceptionMessage = "Test exception";
            var cancellationToken = CancellationToken.None;

            Task CheckAction()
            {
                throw new Exception(exceptionMessage);
            }

            _dbContextWrapper
                .Setup(s => s.BeginTransactionAsync(cancellationToken))
                .ReturnsAsync(_dbContextTransactionMock.Object);

            // Act
            await _dataService.ExecuteSafeAsync(CheckAction, cancellationToken);

            // Assert
            _dbContextTransactionMock.Verify(t => t.RollbackAsync(cancellationToken), Times.Once);
            _dbContextWrapper.Verify(s => s.BeginTransactionAsync(cancellationToken), Times.Once);
            _dbContextTransactionMock.Verify(s => s.CommitAsync(cancellationToken), Times.Never);
        }

        private class TestDataService : BaseDataService<ApplicationDbContext>
        {
            public TestDataService(
                Host.Services.Interfaces.IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
                ILogger<BaseDataService<ApplicationDbContext>> logger)
                : base(dbContextWrapper, logger)
            {
            }

            public new Task ExecuteSafeAsync(Func<Task> action, CancellationToken cancellationToken = default)
            {
                return base.ExecuteSafeAsync(action, cancellationToken);
            }

            public new Task<TResult> ExecuteSafeAsync<TResult>(Func<Task<TResult>> action, CancellationToken cancellationToken = default)
            {
                return base.ExecuteSafeAsync(action, cancellationToken);
            }
        }
    }
}