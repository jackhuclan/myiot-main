using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VgAutoDrill.Infrastructure.Plugin;

namespace UnitTest.VgAutoDrill.Infrastructure;

public class HostApplicationBuilderExtensionsTest
{
    [Fact]
    public void TestAddPlugin()
    {
        var builder = WebApplication.CreateBuilder(Array.Empty<string>());
        builder.Configuration.AddJsonFile("PluginAppSettings.json");
        builder.AddPlugin();
        var app = builder.Build();
        _ = app.RunAsync();

        var plugin = app.Services.GetService<IPluginTest>();
        var plugin2 = app.Services.GetService<IPluginTest2>();
        var pluginOptions = app.Services.GetService<IOptions<FakePluginSetupOptions>>()?.Value;
        var pluginOptions2 = app.Services.GetService<IOptions<FakePluginSetup2Options>>()?.Value;
        Assert.NotNull(plugin);
        Assert.NotNull(plugin2);
        Assert.NotNull(plugin.DoSomething());
        Assert.NotNull(plugin2.DoSomething());
        Assert.NotNull(pluginOptions?.MyProperty);
        Assert.NotNull(pluginOptions2?.MyProperty);
    }
}
