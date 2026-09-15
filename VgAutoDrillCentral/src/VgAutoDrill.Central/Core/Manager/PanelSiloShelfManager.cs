using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Mes.Model;

namespace VgAutoDrill.Central.Core.Manager;

public class PanelSiloShelfManager : PanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask>
{
    private readonly IScheduleTaskManager _scheduleTaskManager;

    public PanelSiloShelfManager(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
    }

    public override IReadOnlyList<PanelSiloRack> DutyRacks => PanelSiloShelfs;

    public override IReadOnlyList<ShelfScheduleTask> NotStartedSchedules => _scheduleTaskManager.NotStartedShelfSchedules;
    public override Task Fetch()
    {
        return Task.CompletedTask;
    }
}
