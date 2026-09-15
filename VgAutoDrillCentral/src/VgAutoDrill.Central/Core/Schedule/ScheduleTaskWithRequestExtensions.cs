using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Central.Core.Schedule;

internal static class ScheduleTaskWithRequestExtensions
{
    private static List<ScheduledTaskStatus> _notFinishedScheduleStatus = new List<ScheduledTaskStatus>
    {
        ScheduledTaskStatus.Created,
        ScheduledTaskStatus.Allocated,
        ScheduledTaskStatus.Running,
    };

    public static IEnumerable<ScheduleTaskWithRequest> ToForkSchedules(this IEnumerable<ScheduleTaskWithRequest> schedules)
    {
        return schedules.Where(x => x.RequestDeviceKind == DeviceKind.PanelSiloFork);
    }

    public static IEnumerable<ScheduleTaskWithRequest> ToShelfSchedules(this IEnumerable<ScheduleTaskWithRequest> schedules)
    {
        return schedules.Where(x => x.RequestDeviceKind == DeviceKind.PublicPanelSiloWIP);
    }

    public static IEnumerable<ScheduleTaskWithRequest> ToDrillSchedules(this IEnumerable<ScheduleTaskWithRequest> schedules)
    {
        return schedules.Where(x => x.RequestDeviceKind == DeviceKind.CNC84Drill || x.RequestDeviceKind == DeviceKind.CNC95Drill);
    }

    /// <summary>
    /// 现在为空的插齿
    /// </summary>
    /// <param name="schedules"></param>
    /// <returns></returns>
    public static IEnumerable<ScheduleTaskWithRequest> EmptyPayloadForkScheduleNow(this IEnumerable<ScheduleTaskWithRequest> schedules)
    {
        return schedules.Where(x => x.RequestDeviceKind == DeviceKind.PanelSiloFork
            && x.InteractionSequence == InteractionSequence.LoadOnly
            && x.ScheduledTaskStatus == ScheduledTaskStatus.Created
            && x.IsEmptyPayload == true
            && string.IsNullOrEmpty(x.AllocatedAgv));
    }

    /// <summary>
    /// 现在为空料仓的插齿
    /// </summary>
    /// <param name="schedules"></param>
    /// <returns></returns>
    public static IEnumerable<ScheduleTaskWithRequest> EmptySiloBoxForkScheduleNow(this IEnumerable<ScheduleTaskWithRequest> schedules)
    {
        return schedules.Where(x => x.RequestDeviceKind == DeviceKind.PanelSiloFork
            && x.InteractionSequence == InteractionSequence.LoadOnly
            && x.ScheduledTaskStatus == ScheduledTaskStatus.Created
            && x.IsEmptySiloBox == true
            && string.IsNullOrEmpty(x.AllocatedAgv));
    }

    /// <summary>
    /// 即将变空的插齿
    /// </summary>
    /// <param name="schedules"></param>
    /// <returns></returns>
    public static IEnumerable<ScheduleTaskWithRequest> EmptyPayloadForkScheduleSoon(this IEnumerable<ScheduleTaskWithRequest> schedules)
    {
        return schedules.Where(x => x.RequestDeviceKind == DeviceKind.PanelSiloFork
            && x.InteractionSequence == InteractionSequence.UnloadOnly
            && x.ScheduledTaskStatus.HasValue && _notFinishedScheduleStatus.Contains(x.ScheduledTaskStatus.Value)
            && !string.IsNullOrEmpty(x.AllocatedAgv));
    }
}
