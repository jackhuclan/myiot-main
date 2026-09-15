namespace VgAutoDrill.Central.Core.Schedule.Handler;

[Flags]
public enum ScheduleDeliveryOption
{
    Unspecified = 0,
    MasterFirst,
    ServantFirst,
    MasterOnly,
    ServantOnly,
    Both
}
