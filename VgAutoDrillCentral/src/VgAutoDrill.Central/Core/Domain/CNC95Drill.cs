using Microsoft.Extensions.DependencyInjection;

namespace VgAutoDrill.Central.Core.Domain;

public class CNC95Drill : Drill
{
    [ActivatorUtilitiesConstructor]
    public CNC95Drill(IServiceProvider serviceProvider) : base(serviceProvider) { }
}
