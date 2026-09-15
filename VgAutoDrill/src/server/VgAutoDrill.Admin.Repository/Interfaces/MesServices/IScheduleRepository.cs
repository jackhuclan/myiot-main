using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedule;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    public interface IScheduleRepository : IBaseRepository<Schedule>
    {
        Task<bool> ExsistSchedule(GetScheduleListReq req);
        Task<IPageList<ScheduleDto>> GetList(GetScheduleListReq req);
        Task<IPageList<ScheduleInfo>> GetFullDataList(GetScheduleListReq req);
        Task<IPageList<ScheduleDto>> GetScheduleWithRequestList(GetScheduleListReq req);

        /// <summary>
        /// 根据scheduleId更新库位panel信息
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <param name="scheduleLocationPanels"></param>
        /// <returns></returns>
        Task<bool> UpdateLocationPanels(long scheduleId, List<ScheduleLocationPanel> scheduleLocationPanels);
    }
}