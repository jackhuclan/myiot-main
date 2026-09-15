using VgEAPClient.Socket;

namespace UnitTest.VgEAPClient.Socket;

public class TransactionIdMakerTest
{
    [Fact]
    public void NextIdShouldWork()
    {
        var maker = new TransactionIdMaker();
        var result = maker.NextId();
        Assert.NotNull(result);
    }
}
