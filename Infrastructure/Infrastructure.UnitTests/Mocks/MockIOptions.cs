using Microsoft.Extensions.Options;

namespace Infrastructure.UnitTests.Mocks
{
    public class MockIOptions<T>
        : IOptions<T>
        where T : class
    {
        public T Value { get; set; }
    }
}
