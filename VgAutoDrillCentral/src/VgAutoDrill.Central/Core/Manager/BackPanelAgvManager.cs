using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Central.Core.Domain;

namespace VgAutoDrill.Central.Core.Manager;

public class BackPanelAgvManager : PanelAgvManager<BackPanelAgv>
{
    private readonly IDeviceManager _deviceHolder;

    public BackPanelAgvManager(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _deviceHolder = serviceProvider.GetRequiredService<IDeviceManager>();
    }

    public override IReadOnlyList<PanelAgv> DutyAgvs => _deviceHolder.BackPanelAgvs;

    public override Task RunAsync()
    {
        return Task.CompletedTask;
    }
}
