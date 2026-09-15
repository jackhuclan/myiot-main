using VgAutoDrill.Fundation.Iot.Configuration;

namespace VgAutoDrill.Fundation.Iot;

public interface IDeviceConnector : IDisposable
{
    /// <summary>
    /// connect to plc
    /// </summary>
    Func<DeviceDescriptor, Task<bool>> ConnectFunc { get; set; }

    /// <summary>
    /// heartbeat with plc
    /// </summary>
    Func<Task> HeartBeatFunc { get; set; }

    event EventHandler<DevcieConnectedEventArgs> OnConnected;
    bool IsConnected { get; set; }
    Task Connect(CancellationToken cancellationToken = default);
}
