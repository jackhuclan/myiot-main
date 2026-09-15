using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Fundation.Iot;

public interface IVehicle
{
    Task<DeviceServiceInvokeResponse> Move(DeviceServiceInvokeRequest deviceServiceInvokeRequest);
    Task<DeviceServiceInvokeResponse> Charge(DeviceServiceInvokeRequest deviceServiceInvokeRequest);
}
