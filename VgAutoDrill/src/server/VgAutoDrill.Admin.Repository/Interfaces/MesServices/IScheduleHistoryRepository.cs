using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedule;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    public interface IScheduleHistoryRepository : IBaseRepository<ScheduleHistory>
    {
        Task<bool> ExsistSchedule(GetScheduleListReq req);
        Task<IPageList<ScheduleDto>> GetList(GetScheduleListReq req);
        Task<IPageList<ScheduleInfo>> GetFullDataList(GetScheduleListReq req);
        Task<IPageList<ScheduleDto>> GetScheduleWithRequestList(GetScheduleListReq req);
    }
}
