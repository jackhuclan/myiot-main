using Microsoft.Extensions.Configuration;
using VgAutoDrill.Fundation.Iot.Configuration;

namespace UnitTest.VgAutoDrill.Fundation.Iot;

public class DeviceGatewayOptionsTest
{
    [Fact]
    public void TestLoadConfig()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json")
              .Build();

        var deviceGatewayOptions = configuration.GetSection(nameof(DeviceGatewayOptions))
                                                .Get<DeviceGatewayOptions>();

        Assert.NotNull(deviceGatewayOptions);
        Assert.Equal(5, deviceGatewayOptions.NoticeCentralHeartbeat.Minutes);
    }
}
