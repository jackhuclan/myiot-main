using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Fundation.Command;

/// <summary>
///简单服务调用指令
/// </summary>
/// <typeparam name="TDevice"></typeparam>
public abstract class SimpleCommand<TDevice> : DeviceShare<TDevice>, IRemoteCommand
    where TDevice : Device
{
    public SimpleCommand(IServiceProvider serviceProvider,
        TDevice device,
        CommandDescriptor commandDescriptor)
        : base(serviceProvider, device)
    {
        Descriptor = commandDescriptor;
    }

    public CommandDescriptor Descriptor { get; }

    public abstract Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest);
}
