using VgAutoDrill.Fundation.Iot.Models;

namespace VgDeviceGateway.Devices.Drill.InteractionLoad
{
    public interface IDrillLoadInteraction
    {
        public Task<DeviceServiceInvokeResponse> CompleteLoadMaterialLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, int position);

        public Task<DeviceServiceInvokeResponse> InvokeLoadMaterialLocal(int position);

        public Task<DeviceServiceInvokeResponse> PrepareLoadMaterialLocal(DeviceServiceInvokeRequest request, int position);

        public Task<DeviceServiceInvokeResponse> CanExecuteDeviceServiceInvokeLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest,
                                                                                 DefaultDrill interactingDevice,
                                                                                 string eventId = "",
                                                                                 string eventName = "",
                                                                                 string eventMessage = "",
                                                                                 string methodName = "");
    }
}
