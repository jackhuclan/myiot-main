using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VgAutoDrill.Infrastructure.Plugin;

public interface IPluginSetupItem
{
    PluginSetupDescriptor PluginDescriptor { get; }
    void Install(IServiceCollection services, IConfigurationBuilder configuration);
}
