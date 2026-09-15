using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceTemporaryMaintenanceRecords;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    public interface IDeviceTemporaryMaintenanceRecordsRepository : IBaseRepository<DeviceTemporaryMaintenanceRecords>
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<PageList<DeviceTemporaryMaintenanceRecords>> GetList(GetDeviceTemporaryMaintenanceRecordsListReq req);

    }
}
