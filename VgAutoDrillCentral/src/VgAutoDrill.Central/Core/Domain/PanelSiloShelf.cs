using Microsoft.Extensions.DependencyInjection;

namespace VgAutoDrill.Central.Core.Domain;

/// <summary>
/// shelf就是wip
/// </summary>
public class PanelSiloShelf : PanelSiloRack
{
    [ActivatorUtilitiesConstructor]
    public PanelSiloShelf(IServiceProvider serviceProvider) : base(serviceProvider) { }
}
