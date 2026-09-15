using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VgAutoDrill.Infrastructure.Plugin;

namespace UnitTest.VgAutoDrill.Infrastructure;

public class PluginSetupSelectorTest
{
    [Fact]
    public void CanInstallPlugin()
    {
        var serviceProvider = ConfigurePluginSetupOptions("PluginAppSettings.json");

        var plugin = serviceProvider.GetService<IPluginTest>();
        var plugin2 = serviceProvider.GetService<IPluginTest2>();
        var pluginOptions = serviceProvider.GetService<IOptions<FakePluginSetupOptions>>()?.Value;
        var pluginOptions2 = serviceProvider.GetService<IOptions<FakePluginSetup2Options>>()?.Value;
        Assert.NotNull(plugin);
        Assert.NotNull(plugin2);
        Assert.NotNull(plugin.DoSomething());
        Assert.NotNull(plugin2.DoSomething());
        Assert.NotNull(pluginOptions?.MyProperty);
        Assert.NotNull(pluginOptions2?.MyProperty);
    }

    [Fact]
    public void Plugin2IsNotEnabled()
    {
        var serviceProvider = ConfigurePluginSetupOptions("PluginAppSettings2.json");

        var plugin = serviceProvider.GetService<IPluginTest>();
        var plugin2 = serviceProvider.GetService<IPluginTest2>();
        var pluginOptions = serviceProvider.GetService<IOptions<FakePluginSetupOptions>>()?.Value;
        var pluginOptions2 = serviceProvider.GetService<IOptions<FakePluginSetup2Options>>()?.Value;

        Assert.NotNull(plugin);
        Assert.NotNull(plugin.DoSomething());
        Assert.NotNull(pluginOptions?.MyProperty);

        Assert.Null(plugin2);
        Assert.Null(pluginOptions2?.MyProperty);
    }

    private static ServiceProvider ConfigurePluginSetupOptions(string jsonFile)
    {
        var serviceCollection = new ServiceCollection();

        ConfigurationManager manager = new ConfigurationManager();
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonFile(jsonFile);
        var configuration = configurationBuilder.Build();
        manager.AddConfiguration(configuration);

        serviceCollection.AddPlugin(manager);

        var serviceProvider = serviceCollection.BuildServiceProvider();
        return serviceProvider;
    }
}
