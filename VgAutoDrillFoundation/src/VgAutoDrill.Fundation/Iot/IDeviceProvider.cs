namespace VgAutoDrill.Fundation.Iot;

public interface IDeviceProvider
{
    IReadOnlyList<Device> Devices { get; }
    Device GetDevice(string? deviceId);
}
