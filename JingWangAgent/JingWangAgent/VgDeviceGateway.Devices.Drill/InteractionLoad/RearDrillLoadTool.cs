using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgDeviceGateway.Devices.Drill.InteractionLoad
{
    public class RearDrillLoadTool : DeviceShare<DefaultDrill>, IDrillLoadInteraction
    {
        private readonly ILogger<RearDrillLoadTool> logger;

        public RearDrillLoadTool(ILogger<RearDrillLoadTool> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
        {
            this.logger = logger;
        }

        public async Task<DeviceServiceInvokeResponse> CompleteLoadMaterialLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, int position)
        {
            return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, string.Empty);
        }

        public async Task<DeviceServiceInvokeResponse> InvokeLoadMaterialLocal(int position)
        {
            return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, string.Empty);
        }

        public async Task<DeviceServiceInvokeResponse> PrepareLoadMaterialLocal(DeviceServiceInvokeRequest request, int position)
        {
            return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, string.Empty);
        }

        public async Task<DeviceServiceInvokeResponse> CanExecuteDeviceServiceInvokeLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest,
                                                                                 DefaultDrill InteractingDevice,
                                                                                 string eventId = "",
                                                                                 string eventName = "",
                                                                                 string eventMessage = "",
                                                                                 string methodName = "")
        {
            return await this.InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, string.Empty);
        }
    }
}
