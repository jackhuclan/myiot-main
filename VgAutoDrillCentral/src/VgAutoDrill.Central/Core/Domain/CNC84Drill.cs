using Microsoft.Extensions.DependencyInjection;

namespace VgAutoDrill.Central.Core.Domain;

public class CNC84Drill : Drill
{
    [ActivatorUtilitiesConstructor]
    public CNC84Drill(IServiceProvider serviceProvider) : base(serviceProvider) { }
}
