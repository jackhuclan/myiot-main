using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VgAutoDrill.Fundation.Channel;
using VgAutoDrill.Infrastructure.Plugin;

namespace VgDeviceMonitor;

public class DeviceMonitorPluginSetup : IPluginSetupItem
{
    public DeviceMonitorPluginSetup(PluginSetupDescriptor pluginSetupDescriptor)
    {
        PluginDescriptor = pluginSetupDescriptor;
    }

    public PluginSetupDescriptor PluginDescriptor { get; }

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IExternalDataReportFactory, DeviceMonitorReportFactory>());
    }
}
