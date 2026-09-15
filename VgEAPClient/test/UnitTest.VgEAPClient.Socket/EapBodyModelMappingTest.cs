using VgEAPClient.Socket;
using VgEAPClient.Socket.Models;

namespace UnitTest.VgEAPClient.Socket;

public class EapBodyModelMappingTest
{
    [Fact]
    public void CheckReplyTypesWorks()
    {
        var requestType = typeof(AreYouThereRequest);
        var expectedReplyType = typeof(AreYouThereRequestReply);
        var replyType = EapBodyModelMapping.ReplyTypes.GetValueOrDefault(requestType);
        Assert.Equal(expectedReplyType, replyType);
    }
}
