using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Infrastructure.Plugin;

namespace UnitTest.VgAutoDrill.Infrastructure;

internal class FakePluginSetup : IPluginSetupItem
{
    public FakePluginSetup(PluginSetupDescriptor pluginSetupDescriptor)
    {
        PluginDescriptor = pluginSetupDescriptor;
    }

    public PluginSetupDescriptor PluginDescriptor { get; }

    public bool HasInstalled { get; set; }
    public void Install(IServiceCollection services, IConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.AddJsonFile($"./Plugin1.json", false);

        var configuration = configurationBuilder.Build();
        services.Configure<FakePluginSetupOptions>(configuration.GetSection(nameof(FakePluginSetupOptions)));

        HasInstalled = true;
        services.AddSingleton<IPluginTest, PluginTest>();
    }
}
