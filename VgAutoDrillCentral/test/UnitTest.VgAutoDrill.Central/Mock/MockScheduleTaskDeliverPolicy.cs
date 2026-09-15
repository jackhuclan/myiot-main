using VgAutoDrill.Central.Core.Schedule;
using VgAutoDrill.Central.Core.Schedule.Deliver;

namespace UnitTest.VgAutoDrill.Central.Mock;

internal class MockScheduleTaskDeliverPolicy : IScheduleTaskDeliverPolicy
{
    private readonly SiloTestContext _siloTestContext;

    public MockScheduleTaskDeliverPolicy(SiloTestContext siloTestContext)
    {
        _siloTestContext = siloTestContext;
    }

    public Task<AgvAllocationResult> DeliverScheduleTask(DeliverScheduleTaskRequirement scheduleTaskRequirement)
    {
        _siloTestContext.DeliveredRequirements.Add(scheduleTaskRequirement);
        _siloTestContext.RaiseRequirementsAddedEvent(scheduleTaskRequirement);
        return Task.FromResult(new AgvAllocationResult(AgvAllocationResultCode.Success, AgvAllocationFailedReason.Completed));
    }
}
