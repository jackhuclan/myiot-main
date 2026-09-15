using VgAutoDrill.Admin.Model.Entites.Mes;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IDevicePanelDomainService : IBaseDomainService<DevicePanel>
    {
        /// <summary>
        /// 清空指定设备的负载panel
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        Task<int> Clear(string deviceCode);
    }
}
