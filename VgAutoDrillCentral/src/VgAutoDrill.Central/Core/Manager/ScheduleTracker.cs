using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Central.Core.Mes.Model;

namespace VgAutoDrill.Central.Core.Manager;

/// <summary>
/// 调度追踪
/// </summary>
public class ScheduleTracker : IScheduleTracker
{
    private static readonly ActivitySource ActivitySource = new(nameof(ScheduleTracker));
    private readonly ILogger<ScheduleTracker> _logger;
    private ConcurrentDictionary<long, Activity> _activities = new();

    public ScheduleTracker(IWorkOrderTaskAdapter workOrderTaskAdapter,
        ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ScheduleTracker>();
    }

    public Activity StartScheduleActivity(ScheduleTask scheduleTask)
    {
        var activity = ActivitySource.StartActivity(scheduleTask.Id.ToString(), ActivityKind.Internal);
        activity?.SetTag("schedule.itemCode", scheduleTask.ItemCode);
        activity?.SetTag("schedule.taskCode", scheduleTask.TaskId);
        activity?.SetTag("schedule.code", scheduleTask.Code);
        activity?.SetTag("schedule.callerDeviceId", scheduleTask.CallerDeviceId);
        _activities.TryAdd(scheduleTask.Id, activity);
        return activity;
    }

    public Activity StartChildActivity(string activityName, string parentId)
    {
        return ActivitySource.StartActivity(activityName, ActivityKind.Internal, parentId);
    }

    public bool TryGetScheduleActivity(long scheduleId, out Activity? activity)
    {
        return _activities.TryGetValue(scheduleId, out activity);
    }

    public bool TryStopScheduleActivity(ScheduleTask scheduleTask)
    {
        if (TryGetScheduleActivity(scheduleTask.Id, out var activity))
        {
            activity?.AddEvent(new ActivityEvent($"schedule.status.{scheduleTask.ScheduledTaskStatus}"));
            activity?.Stop();
            return true;
        }

        return false;
    }
}
