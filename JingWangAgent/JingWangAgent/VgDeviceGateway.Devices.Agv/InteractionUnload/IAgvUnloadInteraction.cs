using VgAutoDrill.Fundation.Iot.Models;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Agv.InteractionUnload
{
    public interface IAgvUnloadInteraction
    {
        public Task<DeviceServiceInvokeResponse> CheckDeviceStatusLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation);

        public Task<DeviceServiceInvokeResponse> TargetDeviceOperationLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation);

        public Task<DeviceServiceInvokeResponse> AgvUnloadMaterialSelfLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation);

        public void ResetSingleStart(DeviceServiceInvokeRequest deviceServiceInvokeRequest);

        public void ResetSingleFinished(DeviceServiceInvokeRequest deviceServiceInvokeRequest);

        public void UpdateSiloInfoForUnload(DeviceServiceInvokeRequest deviceServiceInvokeRequest);
    }
}
