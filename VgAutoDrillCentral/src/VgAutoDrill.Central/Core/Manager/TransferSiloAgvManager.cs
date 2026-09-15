using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Central.Core.Domain;

namespace VgAutoDrill.Central.Core.Manager;

public class TransferSiloAgvManager : PanelAgvManager<TransferSiloAgv>
{
    private readonly IDeviceManager _deviceManager;

    public TransferSiloAgvManager(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _deviceManager = serviceProvider.GetRequiredService<IDeviceManager>();
    }

    public override IReadOnlyList<PanelAgv> DutyAgvs => _deviceManager.TransferSiloAgvs;

    public override Task RunAsync()
    {
        return Task.CompletedTask;
    }
}
