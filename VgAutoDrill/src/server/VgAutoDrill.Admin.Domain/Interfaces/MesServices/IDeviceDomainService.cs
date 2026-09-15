using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IDeviceDomainService : IBaseDomainService<Device>
    {
        public Task<PageDto<DeviceDto>> PageList(GetDeviceListReq req);

        Task<List<RouteAndProcessListByDevice>> GetRouteAndProcessList(string agvDeviceCode, string anyDeviceCode);

        /// <summary>
        /// 获取在线设备绑定的工艺路线
        /// </summary>
        /// <param name="devices"></param>
        /// <returns></returns>
        Task<List<CentralOnlineDeviceDto>> GetOnlineDeviceInfo(List<CentralOnlineDeviceDto> devices);

        /// <summary>
        /// 获取钻机下Commit任务
        /// </summary>
        /// <param name="devices"></param>
        /// <returns></returns>
        Task<List<DrillDeviceTaskDto>> GetDrillDeviceTask(List<CentralOnlineDeviceDto> devices);

        /// <summary>
        /// 获取近7天调度记录
        /// </summary>
        /// <returns></returns>
        Task<List<Schedule>> GetSchedules();

        /// <summary>
        /// 根据调度状态，统计近七天设备调度数量
        /// </summary>
        /// <returns></returns>
        Task<List<Schedule>> GetSchedulesByStatus(ScheduledTaskStatus scheduledTaskStatus);

        /// <summary>
        /// 获取AGV最近调度记录
        /// </summary>
        /// <param name="devices"></param>
        /// <param name="queryCount"></param>
        /// <returns></returns>
        Task<List<ScheduleDto>> GetLatestSchedule(List<CentralOnlineDeviceDto> devices, int queryCount);

        /// <summary>
        /// 根据设备Code获取绑定的工艺路线和工序（非AGV）
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        Task<List<RouteAndProcessListByDevice>> GetRPListByDeviceCode(string deviceCode);
        Task<IPageList<Device>> GetDrills(GetDeviceListReq req);
    }
}
