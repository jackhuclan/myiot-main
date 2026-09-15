using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceRecordsSummary;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IDeviceRecordsSummaryDomainService : IBaseDomainService<DeviceRecordsSummary>
    {
        Task<PageDto<DeviceRecordsSummaryDto>> GetList(GetDeviceRecordsSummaryListReq req);
        Task<DateTime?> GetMaxDate();

    }
}
