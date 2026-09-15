using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceMain;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IDeviceMaintainDomainService : IBaseDomainService<DeviceMaintain>
    {
        Task<List<DeviceMaintainToExcelDto>> GetToExcelList();
    }
}
