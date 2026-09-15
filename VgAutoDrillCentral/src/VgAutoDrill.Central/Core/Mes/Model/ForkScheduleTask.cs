using VgAutoDrill.Central.Core.AutoMapper;
using VgAutoDrill.Central.Core.Schedule;

namespace VgAutoDrill.Central.Core.Mes.Model;

public class ForkScheduleTask : ScheduleTaskWithRequest
{
    [IgnoreMap]
    public override ScheduleRequirement Requirement => new ScheduleRequirement()
    {
        CallerDeviceId = CallerDeviceId!,
        RequireDrilledItemCode = ItemCode!,
        MaterialKind = Behavior.MaterialKind,
    };
}
