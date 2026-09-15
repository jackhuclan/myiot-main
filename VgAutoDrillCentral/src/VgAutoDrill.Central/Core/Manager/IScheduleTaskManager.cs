using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Central.Core.Manager;

public interface IScheduleTaskManager
{
    /// <summary>
    /// 未完成的钻机调度
    /// </summary>
    public IReadOnlyList<DrillScheduleTask> NotStartedDrillSchedules { get; }

    /// <summary>
    /// 未完成的Fork调度
    /// </summary>
    public IReadOnlyList<ForkScheduleTask> NotStartedForkSchedules { get; }

    /// <summary>
    /// 未完成的Pin调度
    /// </summary>
    public IReadOnlyList<PinScheduleTask> NotStartedPinSchedules { get; }

    /// <summary>
    /// 未完成的调度
    /// </summary>
    public IReadOnlyList<ScheduleTaskWithRequest> NotStartedSchedules { get; }

    IReadOnlyList<ScheduleTaskWithRequest> AllTodoOrDoingSchedules { get; }

    /// <summary>
    /// 未完成的Shelf调度
    /// </summary>
    public IReadOnlyList<ShelfScheduleTask> NotStartedShelfSchedules { get; }

    /// <summary>
    /// 未完成的Unpin调度
    /// </summary>
    public IReadOnlyList<UnpinScheduleTask> NotStartedUnpinSchedules { get; }

    IReadOnlyList<ScheduleTaskWithRequest> Tasks { get; }

    /// <summary>
    /// 未完成的Fork和Shelf调度
    /// </summary>
    IReadOnlyList<ScheduleTaskWithRequest> NotStartedForkOrShelfSchedules { get; }

    /// <summary>
    /// 没有开始的，指定了agv来执行的调度
    /// </summary>
    IReadOnlyList<ScheduleTaskWithRequest> NotStartedSpecifiedAgvSchedules { get; }

    IReadOnlyList<ForkScheduleTask> ForkSchedules { get; }

    Task<string> AllocateDrillScheduleTask(ScheduleTaskWithRequest schedule, Agv agv, InteractionSequence convertedInteractionSequence);

    Task Evict();

    /// <summary>
    /// 是否存在未开始或者正在执行的调度任务
    /// </summary>
    /// <param name="routingKey"></param>
    /// <returns></returns>
    bool HasUncompletedTaskOfRoutingKey(string routingKey);

    /// <summary>
    /// agv存在未完成的任务
    /// </summary>
    /// <param name="allocatedAgv"></param>
    /// <param name="uncompletedScheduleIds">未完成的任务</param>
    /// <param name="uncompletedDeviceIds">未完成的设备</param>
    /// <returns></returns>
    bool HasUncompletedTaskOfAgv(string allocatedAgv, out long[] uncompletedScheduleIds, out string[] uncompletedDeviceIds);

    /// <summary>
    /// 从数据库获取多长时间的任务，刷新内存中数据
    /// </summary>
    Task Refresh();

    /// <summary>
    /// 指定下一个接着要做的任务
    /// </summary>
    /// <param name="currentScheduleCode"></param>
    /// <param name="macthedAgv"></param>
    /// <param name="relateCode"></param>
    /// <returns></returns>
    Task SpecifyFollowedSchedule(string currentScheduleCode, string macthedAgv, string relateCode, string schedulePath = "");

    Task UpdateScheduleForChangingSilo(string eventTraceId, DeviceProxy agvDevice, string schedulePath = "");

    Task<DeviceEventReportResponse> ScheduleAGVDevcieViaMysql(DeviceEventReportRequest eventRequest, DeviceProxy callerDevice);

    Task<string> SetScheduleAsException(FailScheduleTaskRequest request, Agv? deviceProxy = null);

    Task<string> SetScheduleAsFinished(CompleteScheduleTaskRequest request, Agv? deviceProxy = null);

    Task<string> SetScheduleAsWorking(StartScheduleTaskRequest startScheduleTaskRequest, Agv agv = null);

    Task<string> CancelSingleSchedule(string traceId, bool isForced, string reason = "");

    bool TryGetScheduleTaskByTraceId(string eventTraceId, out ScheduleTaskWithRequest? scheduleTask);

    /// <summary>
    /// 获取库位上最新的调度
    /// </summary>
    /// <param name="locationCode">库位编号</param>
    /// <param name="scheduleTask">库位上的调度</param>
    /// <returns></returns>
    bool TryGetLatestScheduleTaskOfLocation(string locationCode, out ScheduleTaskWithRequest? scheduleTask);

    ScheduleTaskWithRequest FindScheduleByTraceId(string traceId);

    bool TryGetNotStartedScheduleByLocationCode(string locationCode, out ScheduleTaskWithRequest? scheduleTas);

    bool TryGetTodoOrDoingScheduleByLocationCode(string locationCode, out ScheduleTaskWithRequest? scheduleTas);

    Task<AddScheduleLogResponse> AddScheduleLog(AddScheduleLogRequest request);

    //Task<string> SetScheduleAsFinished(CompleteScheduleTaskRequest request);

    bool TryGetScheduleTaskById(long id, out ScheduleTaskWithRequest? scheduleTask);

    Task UpdateAllocatedAgv(ScheduleTaskWithRequest schedule);

    Task<string> SetScheduleUrgent(long id);

    Task<int> RefreshTimeoutSchedule();
    Task AddLog(long masterId, string reason);
}
