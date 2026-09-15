using VgAutoDrill.Fundation.Iot.Configuration;

namespace VgAutoDrill.Fundation.Iot;

public interface IDeviceConnectorProvider
{
    /// <summary>
    /// 根据设备描述获取设备连接器<see cref="IDeviceConnector"/>
    /// </summary>
    /// <param name="deviceDescriptor"></param>
    /// <returns></returns>
    IDeviceConnector GetDeviceConnector(DeviceDescriptor deviceDescriptor);
}
