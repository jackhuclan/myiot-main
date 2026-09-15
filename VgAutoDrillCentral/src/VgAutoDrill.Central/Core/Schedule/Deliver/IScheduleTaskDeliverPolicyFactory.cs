using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Schedule.Deliver;

internal interface IScheduleTaskDeliverPolicyFactory
{
    IScheduleTaskDeliverPolicy Create(DeviceProxy callerDevice, MaterialKind materialKind);
}
