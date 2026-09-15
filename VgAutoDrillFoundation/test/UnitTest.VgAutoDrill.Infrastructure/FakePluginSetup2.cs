using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Infrastructure.Plugin;

namespace UnitTest.VgAutoDrill.Infrastructure;

internal class FakePluginSetup2 : IPluginSetupItem
{
    public FakePluginSetup2(PluginSetupDescriptor pluginSetupDescriptor)
    {
        PluginDescriptor = pluginSetupDescriptor;
    }

    public PluginSetupDescriptor PluginDescriptor { get; }

    public bool HasInstalled { get; set; }
    public void Install(IServiceCollection services, IConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.AddJsonFile($"./Plugin2.json", false);
        var configuration = configurationBuilder.Build();
        services.Configure<FakePluginSetup2Options>(configuration.GetSection(nameof(FakePluginSetup2Options)));

        HasInstalled = true;
        services.AddSingleton<IPluginTest2, PluginTest2>();
    }
}
