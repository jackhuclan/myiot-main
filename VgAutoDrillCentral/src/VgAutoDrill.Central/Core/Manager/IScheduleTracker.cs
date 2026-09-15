
using System.Diagnostics;
using VgAutoDrill.Central.Core.Mes.Model;

namespace VgAutoDrill.Central.Core.Manager;

public interface IScheduleTracker
{
    Activity StartChildActivity(string activityName, string parentId);
    /// <summary>
    /// 开始某个调度的追踪
    /// </summary>
    /// <param name="scheduleTask"></param>
    /// <returns></returns>
    Activity StartScheduleActivity(ScheduleTask scheduleTask);
    bool TryGetScheduleActivity(long scheduleId, out Activity? activity);
    bool TryStopScheduleActivity(ScheduleTask scheduleTask);
}
