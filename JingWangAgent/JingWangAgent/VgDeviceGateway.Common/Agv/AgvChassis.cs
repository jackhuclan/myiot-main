using VgAutoDrill.Fundation.Iot.Models;

namespace VgDeviceGateway.Devices.Common.Agv;

public abstract class AgvChassis
{
    public abstract Task<DeviceServiceInvokeResponse> GetAgvInfo(DeviceServiceInvokeRequest deviceServiceInvokeRequest);

    public abstract Task<DeviceServiceInvokeResponse> Move(DeviceServiceInvokeRequest deviceServiceInvokeRequest);

    public abstract Task<DeviceServiceInvokeResponse> Charge(DeviceServiceInvokeRequest deviceServiceInvokeRequest);
}
