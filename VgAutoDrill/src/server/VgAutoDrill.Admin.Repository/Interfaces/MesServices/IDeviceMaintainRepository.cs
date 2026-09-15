using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceMain;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    public interface IDeviceMaintainRepository : IBaseRepository<DeviceMaintain>
    {
        Task<List<DeviceMaintainToExcelDto>> GetToExcelList();
    }
}
