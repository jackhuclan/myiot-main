using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Mock;

public class SimpleDevice : Device
{
    private readonly ILogger<SimpleDevice> _logger;

    public SimpleDevice(DeviceDescriptor deviceDescriptor, IDeviceEngine deviceEngine, IServiceProvider serviceProvider)
        : base(deviceDescriptor, deviceEngine, serviceProvider)
    {
        _logger = LoggerFactory.CreateLogger<SimpleDevice>();
    }

    protected override void ConfigureServiceCapabilities()
    {
        base.ConfigureServiceCapabilities();

        AddCommand("SetLoadAptFileCommand", LoadAtpFile);
    }

    private async Task<DeviceServiceInvokeResponse> LoadAtpFile(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        _logger.LogInformation(deviceServiceInvokeRequest.ToJson());
        _logger.LogInformation($"LoadAptFile 开始了");
        await Task.Delay(5000);
        _logger.LogInformation($"LoadAptFile 返回结束");
        return DeviceServiceInvokeResponse.SUCCESS;
    }
}
