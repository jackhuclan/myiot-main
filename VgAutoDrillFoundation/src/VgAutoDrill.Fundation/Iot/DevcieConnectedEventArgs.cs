using VgAutoDrill.Fundation.Iot.Configuration;

namespace VgAutoDrill.Fundation.Iot;

public class DevcieConnectedEventArgs : EventArgs
{
    public DevcieConnectedEventArgs(DeviceDescriptor deviceDescriptor, bool isConnected)
    {
        DeviceDescriptor = deviceDescriptor;
        IsConnected = isConnected;
    }

    public DeviceDescriptor DeviceDescriptor { get; }
    public bool IsConnected { get; }
}