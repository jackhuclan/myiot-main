using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedule;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IScheduleDomainService : IBaseDomainService<Schedule>
    {
        Task<bool> ExsistSchedule(GetScheduleListReq req);
        Task<PageDto<ScheduleDto>> GetList(GetScheduleListReq req);
        Task<PageDto<ScheduleInfo>> GetFullDataList(GetScheduleListReq req);
        Task<PageDto<ScheduleDto>> GetScheduleWithRequestList(GetScheduleListReq req);

        /// <summary>
        /// 根据scheduleId更新库位panel信息
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <param name="scheduleLocationPanels"></param>
        /// <returns></returns>
        Task<bool> UpdateLocationPanels(long scheduleId, List<ScheduleLocationPanel> scheduleLocationPanels);
    }
}
