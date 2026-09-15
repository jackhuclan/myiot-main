using Microsoft.Extensions.DependencyInjection;

namespace VgAutoDrill.Central.Core.Domain;

public class TransferSiloAgv : PanelAgv
{
    [ActivatorUtilitiesConstructor]
    public TransferSiloAgv(IServiceProvider serviceProvider) : base(serviceProvider) { }
}
