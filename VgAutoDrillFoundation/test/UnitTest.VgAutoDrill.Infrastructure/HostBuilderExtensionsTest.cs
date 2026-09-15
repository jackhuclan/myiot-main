using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using VgAutoDrill.Infrastructure.Plugin;

namespace UnitTest.VgAutoDrill.Infrastructure;

public class HostBuilderExtensionsTest
{
    [Fact]
    public void TestAddPlugin()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureHostConfiguration(configurationBuilder => configurationBuilder.AddJsonFile("PluginAppSettings.json"))
            .ConfigureServices(HostBuilderExtensions.ConfigurePlugins)
            .Build();
        _ = host.RunAsync();

        var plugin = host.Services.GetService<IPluginTest>();
        var plugin2 = host.Services.GetService<IPluginTest2>();
        var pluginOptions = host.Services.GetService<IOptions<FakePluginSetupOptions>>()?.Value;
        var pluginOptions2 = host.Services.GetService<IOptions<FakePluginSetup2Options>>()?.Value;
        Assert.NotNull(plugin);
        Assert.NotNull(plugin2);
        Assert.NotNull(plugin.DoSomething());
        Assert.NotNull(plugin2.DoSomething());
        Assert.NotNull(pluginOptions?.MyProperty);
        Assert.NotNull(pluginOptions2?.MyProperty);
    }
}
