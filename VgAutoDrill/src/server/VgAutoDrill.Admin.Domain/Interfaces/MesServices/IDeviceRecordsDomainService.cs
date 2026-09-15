using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceRecords;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IDeviceRecordsDomainService : IBaseDomainService<DeviceRecords>
    {
        Task<PageDto<DeviceRecordsDto>> GetList(GetDeviceRecordsListReq req);
        Task<DateTime?> GetMinDate();
        Task<List<DeviceRecords>> GetListByDate(DateTime startDate, DateTime endDate);
    }
}
