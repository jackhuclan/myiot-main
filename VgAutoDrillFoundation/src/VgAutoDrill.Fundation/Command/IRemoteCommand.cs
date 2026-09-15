using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Fundation.Command;

public interface IRemoteCommand
{
    CommandDescriptor Descriptor { get; }

    Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest);
}
