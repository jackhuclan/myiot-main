using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Fundation.Pipe;

public interface IPipeClient
{
    bool IsConnected { get; }

    Task ConnectAsync(CancellationToken cancellationToken = default);

    Task DisconnectAsync();
    Task Post(DeviceServiceInvokeRequest deviceServiceInvokeRequest);
    Task<DeviceServiceInvokeResponse> Request(DeviceServiceInvokeRequest deviceServiceInvokeRequest, CancellationToken cancellationToken = default);
}
