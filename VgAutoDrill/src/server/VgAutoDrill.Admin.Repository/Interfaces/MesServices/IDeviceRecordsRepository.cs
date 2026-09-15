using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceRecords;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    public interface IDeviceRecordsRepository : IBaseRepository<DeviceRecords>
    {
        Task<PageList<DeviceRecordsDto>> GetList(GetDeviceRecordsListReq req);
        Task<DateTime?> GetMinDate();

        Task<List<DeviceRecords>> GetListByDate(DateTime startDate, DateTime endDate);
    }
}
