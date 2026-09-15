using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Mes.Model;

namespace VgAutoDrill.Central.Core.Manager;

public class PanelSiloForkManager : PanelSiloRackManager<PanelSiloFork, ForkScheduleTask>
{
    private readonly IScheduleTaskManager _scheduleTaskManager;

    public PanelSiloForkManager(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
    }

    public override IReadOnlyList<PanelSiloRack> DutyRacks => PanelSiloForks;

    public override IReadOnlyList<ForkScheduleTask> NotStartedSchedules => _scheduleTaskManager.NotStartedForkSchedules;

    public override Task Fetch()
    {
        return Task.CompletedTask;
    }
}
