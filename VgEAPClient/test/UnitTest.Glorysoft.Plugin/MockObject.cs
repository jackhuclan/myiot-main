using Moq;

namespace UnitTest.Glorysoft.Plugin;

internal class MockObject<T> : Mock<T>
    where T : class
{
    private readonly Mock<T> _mock;

    public MockObject()
    {
        _mock = new Mock<T>();
    }

    public Mock<T> Mock()
    {
        return _mock;
    }
}
