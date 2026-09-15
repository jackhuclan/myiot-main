using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedule;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ScheduleHistory;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;

namespace VgAutoDrill.Admin.Application.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IScheduleService
    {
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<ScheduleDto>>> GetList(GetScheduleListReq req);
        Task<ResponseDto<PageDto<ScheduleInfo>>> GetFullDataList(GetScheduleListReq req);
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<List<ScheduleDto>>> GetScheduleTasks(GetScheduleListReq req);
        Task<ScheduleDto> FindScheduleByTraceId(string traceId);
        Task<ScheduleDto> FindScheduleByRoutingKey(string routingKey);
        /// <summary>
        /// 是否存在数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<bool> ExsistSchedule(GetScheduleListReq req);
        /// <summary>
        /// 更新调度from中控
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> UpdateCentralTask(AddOrUpdateScheduleReq req);

        /// <summary>
        /// 清除过期无效的任务: 上报已超时5分钟、或者分配后5分钟未继续执行的
        /// 清除redis中的无效锁定标识：设备在线 & 设备没有在途的调度任务 & 创建时间超过5分钟
        /// </summary>
        /// <returns></returns>
        Task<int> ClearTimeoutSchedule();

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<ScheduleDto>> QueryByID(long id);

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> AddData(AddOrUpdateScheduleReq req);
        //Task<ResponseDto<ScheduleDto>> AddWithReturn(AddOrUpdateScheduleReq req);
        Task<bool> SetMatchSchedule(string traceId, string matchAgv, string masterCode, string message = "");
        /// <summary>
        /// 修改信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> UpdateData(AddOrUpdateScheduleReq req);

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Delete(long id);

        /// <summary>
        /// 删除信息集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> DeleteList(object[] idList);

        /// <summary>
        /// 批量取消调度记录
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> BulkCanceled(List<long> idList, string reason = "");
        Task<string> CancelSingle(string traceId, bool isForced = false, string reason = "");
        Task<string> CancelDeviceSingle(string traceId, string reason = "");

        Task<string> FindSingleBySubDeviceCode(string subDeviceCode);

        /// <summary>
        /// 根据调度ID获取详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<ScheduleLogsDto>> QueryLogsByID(long id);

        Task<string> FindSingleByDrillDeviceCode(string sourceDeviceId);

        /// <summary>
        /// 取消已分配的、正在执行的调度记录
        /// </summary>
        /// <returns></returns>
        Task<int> CancelExpireSchedule();
        Task<ResponseDto<string>> SetScheduleUrgent(long id);
        Task<List<ScheduleToExcelDto>> GetToExcelList(GetScheduleListReq req);
        Task<ResponseDto<string>> SetBarcodeCheckResult(long id, bool isBarcodeOk);

        /// <summary>
        /// 转移调度历史数据
        /// </summary>
        /// <returns></returns>
        Task<bool> TransferScheduleHistoryData(TransferScheduleHistoryDataReq req);
        Task RegularDeleteHisData();
        Task<ResponseDto<ScheduleDto>> QueryHistoryByID(long id);

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<ScheduleDto>>> GetHistoryList(GetScheduleListReq req);
        /// <summary>
        /// 根据调度ID获取详细数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<ScheduleLogsDto>> QueryHistoryLogsByID(long id);
        Task<List<ScheduleToExcelDto>> GetHisToExcelList(GetScheduleListReq req);




        /// <summary>
        /// 根据scheduleId更新库位panel信息
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <param name="scheduleLocationPanels"></param>
        /// <returns></returns>
        Task<bool> UpdateLocationPanels(long scheduleId, List<ScheduleLocationPanel> scheduleLocationPanels);

        Task<List<string?>> FindSingleByDrillDeviceCodes(List<string> drillDeviceCodes);
    }
}
