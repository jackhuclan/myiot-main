using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Mes.Model;

namespace VgAutoDrill.Central.Core.Schedule.Deliver;

/// <summary>
/// 下发调度任务需求
/// </summary>
public class DeliverScheduleTaskRequirement
{
    public DeviceProxy CallerDevice { get; set; }
    public Agv AgvDevice { get; set; }
    public ScheduleTaskWithRequest ScheduleTask { get; set; }
}
