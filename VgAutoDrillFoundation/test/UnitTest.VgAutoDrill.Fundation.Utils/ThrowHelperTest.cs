using VgAutoDrill.Fundation.Utils;

namespace UnitTest.VgAutoDrill.Fundation.Utils;

public class ThrowHelperTest
{
    [Fact]
    public void TestThrowArgumentNullException()
    {
        string? strOfNull = null;
        Assert.Throws<ArgumentNullException>(() => ThrowHelper.ThrowArgumentNullException(strOfNull));
    }

    [Fact]
    public void TestThrowArgumentNullException2()
    {
        string? strOfNull = null;
        Assert.Throws<ArgumentNullException>(() => ThrowHelper.ThrowArgumentNullException(strOfNull, "should throw ArgumentNullException"));
    }

    [Fact]
    public void TestThrowArgumentNullException3()
    {
        object? strOfNull = null;
        Assert.Throws<ArgumentNullException>(() => ThrowHelper.ThrowArgumentNullException(strOfNull));
    }

}
