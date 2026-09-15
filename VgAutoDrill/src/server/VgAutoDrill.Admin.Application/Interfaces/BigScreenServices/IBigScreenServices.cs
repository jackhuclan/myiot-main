using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;
using VgAutoDrill.Admin.Model.ViewModels.Mes.FeedBack;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder;

namespace VgAutoDrill.Admin.Application.Interfaces.BigScreenServices
{
    /// <summary>
    /// 大屏接口
    /// </summary>
    public interface IBigScreenServices
    {
        /// <summary>
        /// 获取设备统计数据
        ///（设备类型、总数、在线比例）
        /// </summary>
        /// <returns></returns>
        Task<List<DeviceStatsDto>> GetDeviceStats();

        /// <summary>
        /// 获取设备告警统计数据
        ///（设备名称、告警日期、告警名称、告警级别）
        /// </summary>
        /// <returns></returns>
        Task<List<List<string>>> GetDeviceAlarmStats(int deviceId);

        /// <summary>
        /// 获取板料追踪统计数据
        ///（板料料号，物料代码，对应设备）
        /// </summary>
        /// <returns></returns>
        Task<List<List<string>>> GetPanelStats(int deviceId);

        /// <summary>
        /// 获取工单进度统计数据
        ///（工单号，产品名，工单进度）
        /// </summary>
        /// <returns></returns>
        Task<List<List<string>>> GetWorkOrderStats();

        /// <summary>
        /// 获取近一周产量统计
        ///（工序、近一周每天产量）
        /// </summary>
        /// <returns></returns>
        Task<FeedBackStatsByTimeDto> GetFeedBackStats();

        /// <summary>
        /// 获取生产量统计
        /// （生产目标、实际产量、日进度）
        /// </summary>
        /// <returns></returns>
        Task<TaskStatsByTime> GetTaskStats();

        /// <summary>
        /// 获取工单统计
        ///（生产增率、工单增率）
        /// </summary>
        /// <returns></returns>
        Task<WorkOrderRateStatsDto> GetWorkOrderRateStats();

        /// <summary>
        /// 获取设备运行情况统计
        ///（运行中、故障中、停用、待机）
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        Task<DeviceStatusStatsDto> GetDeviceStatusStats(int deviceId);

        /// <summary>
        /// 获取设备开机率统计
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        Task<List<List<string>>> GetDeviceUptimeStats(int deviceId);

        /// <summary>
        /// 获取设备稼动率统计
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        Task<List<List<string>>> GetDeviceMovementStats(int deviceId);

        /// <summary>
        /// 获取设备加工时间统计
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        Task<List<List<string>>> GetDeviceProcessingStats(int deviceId);

        Task<List<DeviceDataToScreen>> GetDeviceDatas();
        /// <summary>
        /// 获取指定设备的稼动率大屏数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<List<DeviceDataToScreen>> GetSpecificDeviceDatas(GetDeviceListReq request);
    }
}
