using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    public interface ICentralOnlineDevice
    {
        /// <summary>
        /// 获取在线设备详细信息
        /// </summary>
        /// <returns></returns>
        Task<List<CentralOnlineDeviceDto>> GetOnlineDevicesAndRoute();
        Task<List<CentralOnlineDeviceDto>> GetOnlineDevices();

        Task<string> AllotsDeviceCommand(string urlAddress, DeviceCommandCentralRequest commandRequest);
    }
}
