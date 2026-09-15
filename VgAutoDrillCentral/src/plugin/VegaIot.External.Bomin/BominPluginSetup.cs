using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Schedule.Handler;
using VgAutoDrill.Infrastructure.Plugin;

namespace VegaIot.External.Bomin;

public class BominPluginSetup : IPluginSetupItem
{
    public BominPluginSetup(PluginSetupDescriptor pluginSetupDescriptor)
    {
        PluginDescriptor = pluginSetupDescriptor;
    }

    public PluginSetupDescriptor PluginDescriptor { get; }

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IPinScheduleHandler, BominPinScheduleHandler>();
        services.AddSingleton<IUnpinScheduleHandler, BominUnpinScheduleHandler>();
        services.AddSingleton<IPanelBarCodeValidator, BominPanelBarCodeValidator>();
    }
}
