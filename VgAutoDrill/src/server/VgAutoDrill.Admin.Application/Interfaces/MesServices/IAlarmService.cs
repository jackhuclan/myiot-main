using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DvAlarmRecord;

namespace VgAutoDrill.Admin.Application.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAlarmService
    {
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<AlarmDto>>> GetList(GetAlarmListReq req);


        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<AlarmDto>> QueryByID(long id);

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> AddData(AddOrUpdateAlarmReq req);

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> UpdateData(AddOrUpdateAlarmReq req);

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
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<AlarmToExcelDto>> GetToExcelList(GetAlarmListReq req);
        Task<bool> TryHandleBySyncId(long syncId);
        Task<AlarmDto> FindSingleBySyncId(long syncId);
        Task RegularDeleteData();

        /// <summary>
        /// 检查S开头的外部工单，如果没有生产任务时产生告警记录
        /// </summary>
        /// <returns>新增的告警记录数量</returns>
        Task<int> CheckSampleOrderAlarms();

        /// <summary>
        /// 检查是否存在未处理的相同告警代码
        /// </summary>
        /// <param name="alarmCode">告警代码</param>
        /// <returns></returns>
        Task<bool> HasUnhandledAlarm(string alarmCode);
    }
}
