using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SchedulementDetail;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    public interface IScheduleLogRepository : IBaseRepository<ScheduleLog>
    {
        Task<IPageList<ScheduleLog>> GetList(GetScheduleLogListReq req);
    }
}
