using VgEAPClient.Socket;
using VgEAPClient.Socket.Models;

namespace UnitTest.VgEAPClient.Socket;

public class XmlHelperTest
{
    [Fact]
    public void ExtractMessageNameFromXmlShouldWork()
    {
        string message = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<message>\r\n<header>\r\n<messagename>ControlModeCommand</messagename>\r\n<transactionid>20181223121212789</transactionid>\r\n</header>\r\n<body>\r\n<eqp_id></eqp_id>\r\n<control_mode></control_mode>\r\n</body>\r\n<return>\r\n<returncode></returncode>\r\n<returnmessage></returnmessage>\r\n</return>\r\n</message>";
        var result = EapMessageXmlHelper.ExtractMessageNameFromXml(message);
        Assert.Equal("ControlModeCommand", result);
    }

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

        var result = EapMessageXmlHelper.Serialize(message);
        Assert.NotNull(result);
        var obj4 = EapMessageXmlHelper.Deserialize<EapMessage>(result);
        Assert.NotNull(obj4);
        Assert.Equal(typeof(AreYouThereRequest), obj4.Body.GetType());
    }
}
