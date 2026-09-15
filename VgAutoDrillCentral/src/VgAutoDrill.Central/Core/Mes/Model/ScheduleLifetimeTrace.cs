namespace VgAutoDrill.Central.Core.Mes.Model;

/// <summary>
/// 调度任务生命轨迹
/// </summary>
public class ScheduleLifetimeTrace
{
    private List<string> _schedules;
    private static ScheduleLifetimeTrace _scheduleLifetimeTrace = new();
    private ScheduleLifetimeTrace() { }
    public static ScheduleLifetimeTrace Instance => _scheduleLifetimeTrace;

    public void AddLog(string methodName, string message)
    {

    }
}
