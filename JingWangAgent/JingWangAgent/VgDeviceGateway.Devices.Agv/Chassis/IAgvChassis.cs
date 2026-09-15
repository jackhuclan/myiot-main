using VgAutoDrill.Fundation.Iot.Models;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Agv.Chassis
{
    public interface IAgvChassis
    {
        public Task<DeviceServiceInvokeResponse> GetAgvInfoLocal(DefaultAgv InteractingDevice);

        public Task<DeviceServiceInvokeResponse> MoveLocal(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest);

        public Task<DeviceServiceInvokeResponse> ChargeLocal(DefaultAgv InteractingDevice);

        public Task<DeviceServiceInvokeResponse> MoveCheckLocal(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest);

        public Task<bool> IsLowBatteryLocal(DefaultAgv InteractingDevice);

        public Task<bool> CanDispatchLocal(DefaultAgv InteractingDevice);

        public Task<bool> CheckIsArrivedLocal(DefaultAgv InteractingDevice);

        public Task<DefaultCallBackEntity> ArrivedInfoLocal(DefaultAgv InteractingDevice);

        public Task<DeviceServiceInvokeResponse> ArrivedLocal(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest, string moveid = "");

        public Task<DeviceServiceInvokeResponse> MoveOperationLocal(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest, int operationType);

        public Task<DeviceServiceInvokeResponse> CancelLocal(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest);
        public Task<DeviceServiceInvokeResponse> ReleaseLocal(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest);

    }
}
