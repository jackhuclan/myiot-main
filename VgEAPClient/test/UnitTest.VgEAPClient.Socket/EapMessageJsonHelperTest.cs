using VgEAPClient.Socket;
using VgEAPClient.Socket.Models;

namespace UnitTest.VgEAPClient.Socket;

/// <summary>
/// todo: need to verify
/// </summary>
public class EapMessageJsonHelperTest
{
    [Fact]
    public void SerializeAndDeserializeShouldWork()
    {
        var maker = new TransactionIdMaker();
        var message = new EapMessage
        {
            Body = new AreYouThereRequest()
            {
                EqpId = "foo",
                ServerIp = "bar"
            }
        };
        message.Header.MessageName = nameof(AreYouThereRequest);
        message.Header.TransactionId = maker.NextId();

        var result = EapMessageJsonHelper.Serialize(message);
        Assert.NotNull(result);
        var obj4 = EapMessageXmlHelper.Deserialize<EapMessage>(result);
        Assert.NotNull(obj4);
        Assert.Equal(typeof(AreYouThereRequest), obj4.Body.GetType());
    }
}
