using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Agv.InteractionLoad
{
    public class FrontPanelAgvLoadSilo : DeviceShare<DefaultAgv>, IAgvLoadInteraction
    {
        public FrontPanelAgvLoadSilo(IServiceProvider serviceProvider, DefaultAgv device) : base(serviceProvider, device)
        {
        }

        public Task<DeviceServiceInvokeResponse> AgvLoadMaterialSelfLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation)
        {
            throw new NotImplementedException();
        }

        public Task<DeviceServiceInvokeResponse> CheckDeviceStatusLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation)
        {
            throw new NotImplementedException();
        }

        public void UpdateSiloInfoForLoad(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
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
    }
}
