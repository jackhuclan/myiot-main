namespace VgAutoDrill.Central.Core.Schedule.Deliver;

internal interface IScheduleTaskDeliverPolicy
{
    //Task<AgvAllocationResult> DeliverScheduleTask(MatchedEventRequestPair matchedEventRequestPair);

    /// <summary>
    /// 为下发任务做准备工作，并下发调度任务到agv
    /// </summary>
    /// <param name="scheduleTaskRequirement"></param>
    /// <returns></returns>
    Task<AgvAllocationResult> DeliverScheduleTask(DeliverScheduleTaskRequirement scheduleTaskRequirement);
}
