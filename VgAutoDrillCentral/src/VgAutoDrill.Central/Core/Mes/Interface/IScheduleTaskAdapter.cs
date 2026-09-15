using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;
using VgAutoDrill.Central.Core.Mes.Model;

namespace VgAutoDrill.Central.Core.Mes;

public interface IScheduleTaskAdapter
{
    Task AddScheduleLog(long masterId, string message);
    //Task<int> RefreshTimeoutSchedule();

    /// <summary>
    /// 是否存在未开始或者正在执行的调度任务
    /// </summary>
    /// <param name="routingKey"></param>
    /// <returns></returns>
    Task<bool> HasUnstartedOrDoingScheduleTaskByRoutingKey(string routingKey);

    Task<bool> HasCompletedScheduleWithNotStartedWorkTask(string taskId);

    Task<ScheduleTaskWithRequest> FindScheduleByTraceId(string traceId);

    Task<ScheduleTask> FindMasterSchedule(ScheduleTask inputSchedule);

    Task<ScheduleTaskWithRequest> FindTodoScheduleByRoutingKey(string routingKey);

    //
    // 摘要:
    //     获取数据列表
    //
    // 参数:
    //   req:
    Task<List<T>> GetSchedule<T>(QueryScheduleRequest request)
        where T : ScheduleTaskWithRequest;

    Task<ScheduleTaskWithRequest> GetSchedule(long scheduleId);

    Task<string> CancelSingleSchedule(string traceId, bool isForced = false, string reason = "");

    Task<string> FindSingleBySubDeviceCode(string subDeviceCode);

    //Task BatchCancelSchedule(List<long> scheduleIds);

    /// <summary>
    /// 指定下一个接着要做的任务
    /// </summary>
    /// <param name="currentScheduleCode"></param>
    /// <param name="macthedAgv"></param>
    /// <param name="relateCode"></param>
    /// <returns></returns>
    Task SpecifyFollowedSchedule(string currentScheduleCode, string macthedAgv, string relateCode, string schedulePath);

    Task UpdateScheduleForChangingSilo(string eventTraceId, DeviceProxy agvDevice, string schedulePath);

    /// <summary>
    /// 获取未完成的钻机调度请求
    /// </summary>
    /// <returns></returns>
    Task<List<DrillScheduleTask>> GetNotStartedDrillSchedule();

    /// <summary>
    /// 获取未完成的钻机调度请求
    /// </summary>
    /// <param name="deviceId"></param>
    /// <returns></returns>
    Task<List<DrillScheduleTask>> GetNotStartedDrillSchedule(string deviceId);

    /// <summary>
    /// 获取未完成的pin调度请求
    /// </summary>
    /// <returns></returns>
    Task<List<PinScheduleTask>> GetNotStartedPinSchedule();

    /// <summary>
    /// 获取未完成的unpin调度请求
    /// </summary>
    /// <returns></returns>
    Task<List<UnpinScheduleTask>> GetNotStartedUnpinSchedule();

    /// <summary>
    /// 获取未完成的插齿和料架调度请求
    /// </summary>
    /// <returns></returns>
    Task<List<ShelfScheduleTask>> GetNotStartedForkOrShelfSchedule();

    /// <summary>
    /// 获取待执行的配对调度中的后一个调度请求
    /// </summary>
    /// <returns></returns>
    Task<List<ScheduleTaskWithRequest>> GetNotStartedFollowedSchedule();

    /// <summary>
    /// 获取待执行的主叫的调度请求
    /// </summary>
    /// <returns></returns>
    Task<List<ScheduleTaskWithRequest>> GetNotStartedSpecifiedAgvSchedule();

    Task<bool> HasUnstartedOrDoingScheduleTaskByAllocatedAgv(string allocatedAgv);

    /// <summary>
    /// 更新数据库ScheduleTask表
    /// </summary>
    /// <param name="updateDto"></param>
    /// <returns></returns>
    Task<string> UpdateDbScheduleTaskTable(AddOrUpdateScheduleReq updateDto);

    Task<string> AddData(AddOrUpdateScheduleReq updateDto);

    Task UpdateAllocatedAgv(ScheduleTaskWithRequest schedule);
}
