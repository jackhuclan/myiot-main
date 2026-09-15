using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IScheduleHistoryDomainService : IBaseDomainService<ScheduleHistory>
    {
        Task<PageDto<ScheduleDto>> GetList(GetScheduleListReq req);
    }
}
