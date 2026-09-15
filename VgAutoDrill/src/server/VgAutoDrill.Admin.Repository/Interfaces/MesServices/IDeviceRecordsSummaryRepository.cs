using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceRecordsSummary;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    public interface IDeviceRecordsSummaryRepository : IBaseRepository<DeviceRecordsSummary>
    {
        Task<PageList<DeviceRecordsSummaryDto>> GetList(GetDeviceRecordsSummaryListReq req);
        Task<DateTime?> GetMaxDate();
    }
}
