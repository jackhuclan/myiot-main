using VgAutoDrill.Fundation.Utils;

namespace UnitTest.VgAutoDrill.Fundation.Utils;

public class FuncExtensionsTest
{
    [Fact]
    public async Task FuncRetryShouldWork()
    {
        int successCount = 0, failCount = 0, otherCount = 0;

        Func<bool> successFunc = () =>
        {
            Thread.Sleep(100);
            Interlocked.Increment(ref successCount);
            return true;
        };

        Func<bool> failFunc = () =>
        {
            Thread.Sleep(100);
            Interlocked.Increment(ref failCount);
            return false;
        };

        Func<bool> otherFunc = () =>
        {
            Thread.Sleep(100);
            Interlocked.Increment(ref otherCount);
            return otherCount == 2;
        };

        await successFunc.Retry(0);
        Assert.Equal(1, successCount);
        Interlocked.Exchange(ref successCount, 0);

        await successFunc.Retry(1);
        Assert.Equal(1, successCount);
        Interlocked.Exchange(ref successCount, 0);

        await successFunc.Retry(3, TimeSpan.FromSeconds(1));
        await failFunc.Retry(3, TimeSpan.FromSeconds(0.2));
        await otherFunc.Retry(3);
        Assert.Equal(1, successCount);
        Assert.Equal(3, failCount);
        Assert.Equal(2, otherCount);
    }
}
