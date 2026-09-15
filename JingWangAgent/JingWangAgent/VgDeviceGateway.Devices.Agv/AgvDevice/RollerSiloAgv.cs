using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgDeviceGateway.Devices.Common.Agv;

namespace VgDeviceGateway.Devices.Agv.AgvDevice
{
    public class RollerSiloAgv : DeviceShare<DefaultAgv>, IAgvDevice
    {
        private readonly ILogger<RollerSiloAgv> logger;

        public RollerSiloAgv(ILogger<RollerSiloAgv> logger, IServiceProvider serviceProvider, DefaultAgv device) : base(serviceProvider, device)
        {
            this.logger = logger;
        }

        public Tuple<bool, string> AgvToReadyLocal()
        {
            throw new NotImplementedException();
        }

        public Task<DeviceServiceInvokeResponse> ChargeLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            throw new NotImplementedException();
        }

        public Tuple<bool, string> CheckCanMoveLocal()
        {
            throw new NotImplementedException();
        }

        public string ClearExternalMaterialLocal()
        {
            throw new NotImplementedException();
        }

        public Task InitializeLocal()
        {
            throw new NotImplementedException();
        }

        public void InitNoSiloLocal()
        {
            throw new NotImplementedException();
        }

        public List<Panel> InitPayloadPanels()
        {
            throw new NotImplementedException();
        }

        public void InitSiloNoMaterialLocal()
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<bool, string>> LoadExternalMaterialLocal(AgvPageEntity agvPageEntity)
        {
            throw new NotImplementedException();
        }

        public Task<DeviceServiceInvokeResponse> MoveLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            throw new NotImplementedException();
        }

        public Task<DeviceServiceInvokeResponse> MoveOperationLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, int operation)
        {
            throw new NotImplementedException();
        }

        public Task<DeviceServiceInvokeResponse> PlcOpertaionLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest) => throw new NotImplementedException();

        public string QueryExternalMaterialLocal()
        {
            throw new NotImplementedException();
        }

        public Tuple<bool, string> ReadMaterialCodeLocal()
        {
            throw new NotImplementedException();
        }

        public Tuple<bool, string> ReadMaterialCodeLocal(int startIndex) => throw new NotImplementedException();
        public string ScannigConvertL(string Scannignumber) => throw new NotImplementedException();
        public void SetPlcHeartLocal()
        {
            throw new NotImplementedException();
        }

        public Task<DeviceServiceInvokeResponse> ShutdownLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            throw new NotImplementedException();
        }

        public Task<DeviceServiceInvokeResponse> StandbyLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            throw new NotImplementedException();
        }

        public Task<DeviceServiceInvokeResponse> WorkLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            throw new NotImplementedException();
        }
    }
}
