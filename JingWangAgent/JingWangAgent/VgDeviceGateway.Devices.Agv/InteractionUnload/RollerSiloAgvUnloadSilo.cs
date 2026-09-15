using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Agv.InteractionUnload
{
    public class RollerSiloAgvUnloadSilo : DeviceShare<DefaultAgv>, IAgvUnloadInteraction
    {
        private readonly ILogger<RollerSiloAgvUnloadSilo> logger;

        public RollerSiloAgvUnloadSilo(ILogger<RollerSiloAgvUnloadSilo> logger, IServiceProvider serviceProvider, DefaultAgv device) : base(serviceProvider, device)
        {
            this.logger = logger;
        }

        public Task<DeviceServiceInvokeResponse> AgvUnloadMaterialSelfLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation)
        {
            throw new NotImplementedException();
        }

        public Task<DeviceServiceInvokeResponse> CheckDeviceStatusLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation)
        {
            throw new NotImplementedException();
        }

        public void ResetSingleFinished(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            throw new NotImplementedException();
        }

        public void ResetSingleStart(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            throw new NotImplementedException();
        }

        public Task<DeviceServiceInvokeResponse> TargetDeviceOperationLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation)
        {
            throw new NotImplementedException();
        }

        public void UpdateSiloInfoForUnload(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            throw new NotImplementedException();
        }
    }
}
