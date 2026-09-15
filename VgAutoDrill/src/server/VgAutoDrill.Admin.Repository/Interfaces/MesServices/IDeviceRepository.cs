using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    public interface IDeviceRepository : IBaseRepository<Device>
    {
        Task<IPageList<Device>> GetList(GetDeviceListReq req);

        Task<List<RouteAndProcessListByDevice>> GetRouteAndProcessList(string agvDeviceCode, string anyDeviceCode);

        Task<List<CentralOnlineDeviceDto>> GetOnlineDeviceInfo(List<CentralOnlineDeviceDto> devices);

        Task<List<DrillDeviceTaskDto>> GetDrillDeviceTask(List<CentralOnlineDeviceDto> devices);

        Task<List<Schedule>> GetSchedules();

        Task<List<Schedule>> GetSchedulesByStatus(ScheduledTaskStatus scheduledTaskStatus);

        Task<List<Schedule>> GetLatestSchedule(List<CentralOnlineDeviceDto> devices, int queryCount);

        Task<List<RouteAndProcessListByDevice>> GetRPListByDeviceCode(string deviceCode);
        Task<IPageList<Device>> GetDrills(GetDeviceListReq req);
    }
}
