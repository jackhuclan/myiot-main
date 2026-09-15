using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SchedulementDetail;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IScheduleLogDomainService : IBaseDomainService<ScheduleLog>
    {
        Task<PageDto<ScheduleLogDto>> GetList(GetScheduleLogListReq req);
    }
}
