using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Admin.Application.Interfaces
{
    /// <summary>
    /// 设备
    /// </summary>
    public interface IDeviceService
    {
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<DeviceDto>>> GetEquipmentList(GetDeviceListReq req);

        /// <summary>
        /// 获取树形结构数据
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<List<DeviceTreeDto>>> GetEquipmentTreeList();

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<DeviceFullDataDto>> QueryFullDataByID(long id);

        /// <summary>
        /// 根据deviceid获取设备属性
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        Task<DeviceDto> FindSingle(string deviceCode);

        Task<bool> IsExistByDeviceId(string deviceId);

        /// <summary>
        /// 更新设备状态
        /// </summary>
        /// <param name="deviceDescriptor"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        Task<ResponseDto<DeviceInfoDto>> UpdateStatus(DeviceDescriptor deviceDescriptor, DeviceStatus newStatus);

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Add(AddOrUpdateDeviceReq req);
        /// <summary>
        /// 添加信息（校验设备状态）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> AddData(AddOrUpdateDeviceReq req);
        /// <summary>
        /// 修改信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<DeviceInfoDto>> UpdateData(AddOrUpdateDeviceReq req);

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> DeleteData(long id);
        /// <summary>
        /// 删除信息集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> DeleteDataList(List<long> idList);

        /// <summary>
        /// 获取首页设备数据
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<DeviceStatusForHomeDto>> GetHomeData();

        /// <summary>
        /// 根据设备获取关联的生产线
        /// </summary>
        /// <param name="agvDeviceCode"></param>
        /// <param name="anyDeviceCode"></param>
        /// <returns></returns>
        Task<ResponseDto<RouteAndProcessInfoByDeviceDto>> GetRouteAndProcessInfo(string agvDeviceCode, string anyDeviceCode);

        /// <summary>
        /// 批量刷新设备状态
        /// </summary>
        /// <param name="deviceCodes"></param>
        /// <returns></returns>
        Task<int> RefreshDeviceStatus(List<AddOrUpdateDeviceReq> deviceCodes);

        /// <summary>
        /// 获取在线设备详细信息
        /// 绑定的工艺路线
        /// </summary>
        /// <param name="devices"></param>
        /// <returns></returns>
        Task<List<CentralOnlineDeviceDto>> GetOnlineDeviceInfo(List<CentralOnlineDeviceDto> devices);

        /// <summary>
        /// 获取中控系统在线设备列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<CentralOnlineDeviceDto>>> GetCentralOnlineDevice(CentralOnlineDeviceReq req);

        /// <summary>
        /// 获取在线钻机待做任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<DrillDeviceTaskDto>>> GetDrillDeviceTask(GetDrillOrAgvDeviceInfoReq req);

        /// <summary>
        /// 根据设备编码获取在线设备详细信息
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        Task<ResponseDto<CentralOnlineDeviceDto>> GetOnlineDeviceInfo(string deviceId);

        /// <summary>
        /// 获取在线AGV板料信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<AGVDeviceSiloInfo>>> GetAGVDeviceSiloInfo(GetDrillOrAgvDeviceInfoReq req);

        /// <summary>
        /// 统计近七天AGV设备调度数量
        /// </summary>
        /// <returns></returns>
        Task<ScheduleDeviceByTimesStatsDto> GetScheduleDeviceStats();

        /// <summary>
        /// 统计近七天设备调度数量(完成/异常分开统计)
        /// </summary>
        /// <returns></returns>
        Task<ScheduleDeviceByTimesStatsDto> GetScheduleDeviceStatusStats();

        /// <summary>
        /// 根据调度状态，统计近七天设备调度数量
        /// </summary>
        /// <param name="scheduledTaskStatus"></param>
        /// <returns></returns>
        Task<ScheduleDeviceByTimesStatsDto> GetSchedulementDeviceStatsByStatus(ScheduledTaskStatus scheduledTaskStatus);

        Task<ResponseDto<string>> SetDeviceStatus(string deviceCode, int status);

        Task<ResponseDto<string>> BulkInsert(List<DeviceToExcelDto> list);
        Task<ResponseDto<string>> AllotsDeviceCommand(DeviceCommandRequest commandRequest);
    }
}
