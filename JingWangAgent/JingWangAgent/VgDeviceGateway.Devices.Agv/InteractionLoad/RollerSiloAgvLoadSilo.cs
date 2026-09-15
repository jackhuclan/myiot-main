using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Agv.InteractionLoad
{
    public class RollerSiloAgvLoadSilo : DeviceShare<DefaultAgv>, IAgvLoadInteraction
    {
        private readonly ILogger<RollerSiloAgvLoadSilo> logger;

        public RollerSiloAgvLoadSilo(ILogger<RollerSiloAgvLoadSilo> logger, IServiceProvider serviceProvider, DefaultAgv device) : base(serviceProvider, device)
        {
            this.logger = logger;
        }

        public Task<DeviceServiceInvokeResponse> AgvLoadMaterialSelfLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation)
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

        public void UpdateSiloInfoForLoad(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            throw new NotImplementedException();
        }
    }
}
