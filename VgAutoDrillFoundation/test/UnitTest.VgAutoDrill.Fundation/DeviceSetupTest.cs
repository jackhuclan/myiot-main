using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Fundation.Channel;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Store;
using ConfigurationManager = Microsoft.Extensions.Configuration.ConfigurationManager;

namespace UnitTest.VgAutoDrill.Fundation;

public class DeviceSetupTest
{
    [Fact]
    public void CheckDeviceStore()
    {
        var provider = BuildServiceProvider();
        var deviceStore = provider.GetRequiredService<IDeviceStore>();

        Assert.NotNull(deviceStore);
    }

    [Fact]
    public void CheckPeriodicTimerExecutorFactory()
    {
        var provider = BuildServiceProvider();
        var periodicTimers = provider.GetRequiredService<IPeriodicTimerExecutorFactory>();

        Assert.NotNull(periodicTimers);
        Assert.NotNull(periodicTimers["50ms"]);

        int count = 0;
        periodicTimers["50ms"].OnTick += () =>
        {
            Interlocked.Increment(ref count);
            return Task.CompletedTask;
        };

        Thread.Sleep(100);
        Assert.True(count > 0);
    }

    private static ServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();
        var manager = new ConfigurationManager();
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonFile("appsettings.Development.json");
        var configuration = configurationBuilder.Build();
        manager.AddConfiguration(configuration);
        services.AddDevices(manager);
        var provider = services.BuildServiceProvider();
        return provider;
    }
}
