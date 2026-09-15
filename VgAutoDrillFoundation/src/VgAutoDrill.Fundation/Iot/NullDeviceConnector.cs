using VgAutoDrill.Fundation.Iot.Configuration;

namespace VgAutoDrill.Fundation.Iot;

internal class NullDeviceConnector : IDeviceConnector
{
    private volatile bool _isConnected = true;
    public Func<DeviceDescriptor, Task<bool>> ConnectFunc { get; set; } = (d) => Task.FromResult(false);
    public Func<Task> HeartBeatFunc { get; set; } = () => Task.CompletedTask;
    public bool IsConnected { get => _isConnected; set => _isConnected = value; }
    public event EventHandler<DevcieConnectedEventArgs> OnConnected;
    public Task Connect(CancellationToken cancellationToken = default) => Task.CompletedTask;
    public void Dispose() { }
}
