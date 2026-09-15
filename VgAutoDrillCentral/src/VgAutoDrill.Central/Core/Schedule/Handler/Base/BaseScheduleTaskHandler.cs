using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Schedule.Deliver;

namespace VgAutoDrill.Central.Core.Schedule.Handler;

internal abstract class BaseScheduleTaskHandler : IScheduleTaskHandler
{
    private readonly IScheduleTaskDeliverPolicyFactory _scheduleTaskDeliverPolicyFactory;

    public BaseScheduleTaskHandler(IServiceProvider serviceProvider)
    {
        _scheduleTaskDeliverPolicyFactory = serviceProvider.GetRequiredService<IScheduleTaskDeliverPolicyFactory>();
    }

    public abstract Task Handle();

    public async Task<AgvAllocationResult> DeliverScheduleTask(DeliverScheduleTaskRequirement scheduleTaskRequirement)
    {
        var policy = _scheduleTaskDeliverPolicyFactory.Create(scheduleTaskRequirement.CallerDevice, scheduleTaskRequirement.ScheduleTask.EventRequest.RequestMaterialKind);
        return await policy.DeliverScheduleTask(scheduleTaskRequirement);
    }

    /// <summary>
    /// 钻机任务派发给agv
    /// </summary>
    /// <param name="agv"></param>
    /// <param name="drill"></param>
    /// <param name="drillScheduleTask"></param>
    /// <returns></returns>
    protected async Task<bool> TryExecuteDrillTask(PanelAgv agv, Drill drill, DrillScheduleTask drillScheduleTask)
    {
        var drillRequirement = drillScheduleTask.Requirement;
        if (drillRequirement == null)
            return false;

        if (agv.Location.CanMatch(drillRequirement))
        {
            await DeliverScheduleTask(new DeliverScheduleTaskRequirement
            {
                CallerDevice = drill,
                AgvDevice = agv,
                ScheduleTask = drillScheduleTask
            });

            return true;
        }

        return false;
    }
}
