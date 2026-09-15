using VgAutoDrill.Fundation.Command;
using VgAutoDrill.Fundation.Iot.Models;
using VgDeviceGateway.Devices.Drill;

namespace VegaIot.External.XianjinIot.Commands;

internal class OTAUpgradeCallback : SimpleCommand<DefaultDrill>
{
    public OTAUpgradeCallback(IServiceProvider serviceProvider, DefaultDrill device,
        CommandDescriptor commandDescriptor)
        : base(serviceProvider, device, commandDescriptor)
    {
    }

    public override Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        => throw new NotImplementedException();
}
