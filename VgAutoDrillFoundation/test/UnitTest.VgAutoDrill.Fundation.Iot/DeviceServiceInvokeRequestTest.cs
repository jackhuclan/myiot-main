using System.Text.Json;
using VgAutoDrill.Fundation.Iot.Models;

namespace UnitTest.VgAutoDrill.Fundation.Iot;

public class DeviceServiceInvokeRequestTest
{
    [Fact]
    public void RequestTopicShouldNotEmptyAfterDeserialized()
    {
        var request = new DeviceServiceInvokeRequest
        {
            ServiceId = "RemoteCommand",
            TargetProductId = "MockProductId",
            TargetDeviceId = "MockDeviceId",
            TargetClientId = "MockClientId"
        };

        var json = JsonSerializer.Serialize(request);
        var requestFromDeserialized = JsonSerializer.Deserialize<DeviceServiceInvokeRequest>(json);
        Assert.NotNull(requestFromDeserialized);
        Assert.NotEmpty(requestFromDeserialized.RequestTopic);
        Assert.NotEmpty(requestFromDeserialized.ReplyTopic);
    }
}
