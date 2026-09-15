using VgAutoDrill.Fundation.Iot.Configuration;

namespace VgAutoDrill.Central.Core;

public interface IDeviceProxyFactory
{
    DeviceProxy Create();

    DeviceProxy Create(DeviceDescriptor deviceDescriptor);
}
