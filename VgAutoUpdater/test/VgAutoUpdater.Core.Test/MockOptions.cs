using Microsoft.Extensions.Options;
using Moq;

namespace VgAutoUpdater.Core.Test
{
    internal class MockOptions<TOptions> : Mock<IOptions<TOptions>>
        where TOptions : class, new()
    {
        private Mock<IOptions<TOptions>> _mock;

        public MockOptions()
        {
            _mock = new Mock<IOptions<TOptions>>();
        }

        public Mock<IOptions<TOptions>> Use(TOptions options)
        {
            _mock.SetupGet(x => x.Value).Returns(options);
            return _mock;
        }
    }
}
