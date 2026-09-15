using VgAutoDrill.Central.Core.Schedule.Deliver;

namespace UnitTest.VgAutoDrill.Central;

internal class SiloTestContext
{
    public event Action<DeliverScheduleTaskRequirement>? OnDeliveredReqirementsAdded;
    public List<DeliverScheduleTaskRequirement> DeliveredRequirements { get; set; } = new();

    public void RaiseRequirementsAddedEvent(DeliverScheduleTaskRequirement scheduleTaskRequirement)
    {
        OnDeliveredReqirementsAdded?.Invoke(scheduleTaskRequirement);
    }
}
