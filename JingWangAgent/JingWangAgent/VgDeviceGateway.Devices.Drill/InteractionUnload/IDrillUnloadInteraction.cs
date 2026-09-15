using VgAutoDrill.Fundation.Iot.Models;

namespace VgDeviceGateway.Devices.Drill.InteractionUnload
{
    public interface IDrillUnloadInteraction
    {
        public Task<DeviceServiceInvokeResponse> CompleteUnloadMaterialLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, int position);

        public Task<DeviceServiceInvokeResponse> InvokeUnloadMaterialLocal(int position);

        public Task<DeviceServiceInvokeResponse> PrepareUnloadMaterialLocal(DeviceServiceInvokeRequest request, int position);

        public Task<DeviceServiceInvokeResponse> CanExecuteDeviceServiceInvokeLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest,
                                                                                 DefaultDrill interactingDevice,
                                                                                 string eventId = "",
                                                                                 string eventName = "",
                                                                                 string eventMessage = "",
                                                                                 string methodName = "");
    }
}
