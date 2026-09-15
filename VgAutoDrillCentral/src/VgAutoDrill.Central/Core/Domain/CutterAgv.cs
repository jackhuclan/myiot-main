using Microsoft.Extensions.DependencyInjection;

namespace VgAutoDrill.Central.Core.Domain;

internal class CutterAgv : Agv
{
    [ActivatorUtilitiesConstructor]
    public CutterAgv(IServiceProvider serviceProvider) : base(serviceProvider) { }
}
