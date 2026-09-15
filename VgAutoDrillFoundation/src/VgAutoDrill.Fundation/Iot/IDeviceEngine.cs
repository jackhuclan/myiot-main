namespace VgAutoDrill.Fundation.Iot;

public interface IDeviceEngine : IDisposable
{
    IDeviceConnector DeviceConnector { get; }
    Task Fire(Device device, CancellationToken cancellationToken);
    Task Shutdown(CancellationToken cancellationToken);
}
