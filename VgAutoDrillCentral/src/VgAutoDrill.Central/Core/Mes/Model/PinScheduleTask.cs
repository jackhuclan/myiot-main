using VgAutoDrill.Central.Core.AutoMapper;
using VgAutoDrill.Central.Core.Schedule;

namespace VgAutoDrill.Central.Core.Mes.Model;

public class PinScheduleTask : ScheduleTaskWithRequest
{
    [IgnoreMap]
    public override ScheduleRequirement Requirement => new ScheduleRequirement()
    {
    };
}
