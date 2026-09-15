using VgAutoDrill.Fundation.Iot.Models;
using VgDeviceGateway.Devices.Common.Agv;

namespace VgDeviceGateway.Devices.Agv.AgvDevice
{
    public interface IAgvDevice
    {
        public Task InitializeLocal();

        public Task<DeviceServiceInvokeResponse> WorkLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest);

        public Task<DeviceServiceInvokeResponse> StandbyLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest);

        public Task<DeviceServiceInvokeResponse> ShutdownLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest);

        public Task<DeviceServiceInvokeResponse> ChargeLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest);

        public Task<DeviceServiceInvokeResponse> MoveLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest);

        public Task<DeviceServiceInvokeResponse> MoveOperationLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, int operation);

        public Tuple<bool, string> ReadMaterialCodeLocal(int startIndex);
        public string ScannigConvertL(string ScannigData);
        public void SetPlcHeartLocal();

        public Tuple<bool, string> CheckCanMoveLocal();

        public Task<Tuple<bool, string>> LoadExternalMaterialLocal(AgvPageEntity agvPageEntity);

        public string QueryExternalMaterialLocal();

        public string ClearExternalMaterialLocal();

        public void InitSiloNoMaterialLocal();

        public void InitNoSiloLocal();

        public Tuple<bool, string> AgvToReadyLocal();

        public List<Panel> InitPayloadPanels();

        public Task<DeviceServiceInvokeResponse> PlcOpertaionLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest);
    }
}
