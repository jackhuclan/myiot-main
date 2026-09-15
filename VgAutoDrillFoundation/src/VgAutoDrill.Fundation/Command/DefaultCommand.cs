using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Fundation.Command;

public class DefaultCommand<TDevice> : SimpleCommand<TDevice>
    where TDevice : Device
{
    public DefaultCommand(IServiceProvider serviceProvider,
       TDevice device,
        CommandDescriptor commandDescriptor)
       : base(serviceProvider, device, commandDescriptor)
    {
    }

    public override Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return Task.FromResult(DeviceServiceInvokeResponse.SUCCESS);
    }
}
