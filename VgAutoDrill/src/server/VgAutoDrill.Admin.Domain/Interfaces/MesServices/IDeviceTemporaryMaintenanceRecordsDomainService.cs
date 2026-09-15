using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceTemporaryMaintenanceRecords;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IDeviceTemporaryMaintenanceRecordsDomainService : IBaseDomainService<DeviceTemporaryMaintenanceRecords>
    {


        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<PageDto<DeviceTemporaryMaintenanceRecordsDto>> GetList(GetDeviceTemporaryMaintenanceRecordsListReq req);



    }
}
