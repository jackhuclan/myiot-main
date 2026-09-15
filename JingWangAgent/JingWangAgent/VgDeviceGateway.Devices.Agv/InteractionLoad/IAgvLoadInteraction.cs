using VgAutoDrill.Fundation.Iot.Models;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Agv.InteractionLoad
{
    public interface IAgvLoadInteraction
    {
        public Task<DeviceServiceInvokeResponse> CheckDeviceStatusLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation);

        public Task<DeviceServiceInvokeResponse> TargetDeviceOperationLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation);

        public Task<DeviceServiceInvokeResponse> AgvLoadMaterialSelfLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation);

        public void ResetSingleStart(DeviceServiceInvokeRequest deviceServiceInvokeRequest);

        public void ResetSingleFinished(DeviceServiceInvokeRequest deviceServiceInvokeRequest);

        public void UpdateSiloInfoForLoad(DeviceServiceInvokeRequest deviceServiceInvokeRequest);
    }
}
