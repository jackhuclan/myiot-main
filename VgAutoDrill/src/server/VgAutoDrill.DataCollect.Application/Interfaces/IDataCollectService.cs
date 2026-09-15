using VgAutoDrill.DataCollect.Application.Models;
using VgAutoDrill.DataCollect.Application.Models.DeviceEvent;
using VgAutoDrill.DataCollect.Application.Models.DeviceProperty;
using VgAutoDrill.DataCollect.Application.Models.DeviceService;
using VgAutoDrill.DataCollect.Application.Models.DeviceStatus;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.DataCollect.Application.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDataCollectService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        DevicePropertiesReportResponse DevicePropertiesReport(DevicePropertiesReportRequest request);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        void HandleDeviceEventReport(DeviceEventReportRequest request, DeviceEventReportResponse response);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        void HandleDeviceService(DeviceServiceInvokeRequest request, DeviceServiceInvokeResponse response);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        void HandleDeviceStatusReport(DeviceStatusReportRequest request);

        /// <summary>
        /// 获取设备状态日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<CollectResponseDto<List<DeviceStatusDto>>> GetDeviceStatusList(GetDeviceStatusList input);
        /// <summary>
        /// 获取设备服务调度日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<CollectResponseDto<List<DeviceServiceDto>>> GetDeviceServiceList(GetDeviceServiceList input);
        /// <summary>
        /// 获取设备属性日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<CollectResponseDto<List<DevicePropertyDto>>> GetDevicePropertyList(GetDevicePropertyList input);
        /// <summary>
        /// 获取设备事件日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<CollectResponseDto<List<DeviceEventDto>>> GetDeviceEventList(GetDeviceEventList input);

        /// <summary>
        /// 获取设备调度日志
        /// </summary>
        /// <returns></returns>
        Task<List<List<string>>> GetDeviceServiceStatsList();
    }
}
