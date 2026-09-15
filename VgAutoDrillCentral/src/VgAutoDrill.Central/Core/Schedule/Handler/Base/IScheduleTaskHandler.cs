using VgAutoDrill.Central.Core.Schedule.Deliver;

namespace VgAutoDrill.Central.Core.Schedule.Handler;

internal interface IScheduleTaskHandler
{
    Task Handle();
    Task<AgvAllocationResult> DeliverScheduleTask(DeliverScheduleTaskRequirement scheduleTaskRequirement);
}
