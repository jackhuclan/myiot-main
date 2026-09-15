using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Fundation.Command;

public class DelegateCommand<TDevice> : DeviceShare<TDevice>, IRemoteCommand
    where TDevice : Device
{
    private readonly CommandFunction _commandFunction;

    public DelegateCommand(IServiceProvider serviceProvider,
        TDevice device,
        CommandDescriptor commandDescriptor,
        CommandFunction commandFunction)
        : base(serviceProvider, device)
    {
        Descriptor = commandDescriptor;
        _commandFunction = commandFunction;
    }

    public CommandDescriptor Descriptor { get; }

    public Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        => _commandFunction.Invoke(deviceServiceInvokeRequest);
}
