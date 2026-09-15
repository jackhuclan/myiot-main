using Microsoft.Extensions.Options;
using VgAutoDrill.Fundation.Command;
using VgAutoDrill.Fundation.Iot.Models;
using VgDeviceGateway.Devices.Drill;

namespace VegaIot.External.XianjinIot.Commands;

internal class PublicKeyAckCallback : SimpleCommand<DefaultDrill>
{
    private readonly XianJinIotOptions _options;

    public PublicKeyAckCallback(IServiceProvider serviceProvider,
        IOptions<XianJinIotOptions> options,
        DefaultDrill device,
        CommandDescriptor commandDescriptor)
        : base(serviceProvider, device, commandDescriptor)
    {
        _options = options.Value;
    }

    public override Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        _options.Signature = "";
        _options.SignCode = 0;

        return Task.FromResult(new DeviceServiceInvokeResponse { });
    }
}
