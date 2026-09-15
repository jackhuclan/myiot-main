using VgAutoDrill.Central.Core.Schedule.Deliver;

namespace VgAutoDrill.Central.Core.Schedule;

internal class AgvAllocationResult
{
    public AgvAllocationResult(AgvAllocationResultCode code,
        AgvAllocationFailedReason allocationFailedReason)
    {
        Code = code;
        Reason = allocationFailedReason;
    }

    public AgvAllocationResultCode Code { get; }
    public AgvAllocationFailedReason Reason { get; }
    /// <summary>
    /// 旧的任务下发需求
    /// </summary>
    public DeliverScheduleTaskRequirement ScheduleTaskRequirement { get; set; }
}
