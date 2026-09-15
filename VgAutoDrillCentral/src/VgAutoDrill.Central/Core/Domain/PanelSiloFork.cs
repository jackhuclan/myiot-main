using Microsoft.Extensions.DependencyInjection;

namespace VgAutoDrill.Central.Core.Domain;

public class PanelSiloFork : PanelSiloRack
{
    [ActivatorUtilitiesConstructor]
    public PanelSiloFork(IServiceProvider serviceProvider) : base(serviceProvider) { }
}
