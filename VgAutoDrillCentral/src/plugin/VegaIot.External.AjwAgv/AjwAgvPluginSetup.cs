using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Infrastructure.Plugin;

namespace VegaIot.External.AjwAgv;

public class AjwAgvPluginSetup : IPluginSetupItem
{
    public AjwAgvPluginSetup(PluginSetupDescriptor pluginSetupDescriptor)
    {
        PluginDescriptor = pluginSetupDescriptor;
    }

    public PluginSetupDescriptor PluginDescriptor { get; }

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        if (configuration is IConfigurationBuilder configurationBuilder)
        {
            var ajwJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.ajw.json");
            configurationBuilder.AddJsonFile(ajwJsonPath, false);
        }

        services.Configure<AjwAgvAgentOptions>(configuration.GetSection(nameof(AjwAgvAgentOptions)));
        services.AddControllers().AddApplicationPart(typeof(AjwAgvPluginSetup).Assembly);
    }
}
