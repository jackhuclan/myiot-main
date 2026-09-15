using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UnitTest.VgAutoDrill.Fundation.Mock;
using VgAutoDrill.Fundation.Channel;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;

namespace UnitTest.VgAutoDrill.Fundation;

public class PeriodicTimerExecutorFactoryTest
{
    [Fact]
    public void PeriodicTimerExecutorFactoryCouldBeConstructed()
    {
        IPeriodicTimerExecutorFactory executorFactory = BuildTimerExcutorFactory("100ms,5s,10s,20s,30s,1m,2d");
        var executor_100ms = executorFactory["100ms"];
        var executor_5s = executorFactory["5s"];
        var executor_10s = executorFactory["10s"];
        var executor_20s = executorFactory["20s"];
        var executor_30s = executorFactory["30s"];
        var executor_1m = executorFactory["1m"];
        var executor_2d = executorFactory["2d"];

        Assert.NotNull(executorFactory);
        Assert.NotNull(executor_100ms);
        Assert.NotNull(executor_5s);
        Assert.NotNull(executor_10s);
        Assert.NotNull(executor_20s);
        Assert.NotNull(executor_30s);
        Assert.NotNull(executor_1m);
        Assert.NotNull(executor_2d);
    }

    [Fact]
    public void PeriodicTimer100msExecutorShouldWorks()
    {
        int count_100ms = 0;
        IPeriodicTimerExecutorFactory executorFactory = BuildTimerExcutorFactory("100ms,5s");
        var executor_100ms = executorFactory["100ms"];
        executor_100ms.OnTick += () =>
        {
            Interlocked.Increment(ref count_100ms);
            return Task.CompletedTask;
        };

        Thread.Sleep(1000);
        Assert.True(count_100ms >= 9);
    }

    [Fact]
    public void PeriodicTimer1sExecutorShouldWorks()
    {
        int count_100ms = 0, count_1s = 0;
        IPeriodicTimerExecutorFactory executorFactory = BuildTimerExcutorFactory("100ms,1s");

        var executor_100ms = executorFactory["100ms"];
        executor_100ms.OnTick += () =>
        {
            Interlocked.Increment(ref count_100ms);
            return Task.CompletedTask;
        };

        var executor_1s = executorFactory["1s"];
        executor_1s.OnTick += () =>
        {
            Interlocked.Increment(ref count_1s);
            return Task.CompletedTask;
        };

        Thread.Sleep(5 * 1000);

        Assert.True(count_100ms >= 49);
        Assert.True(count_1s >= 4);
    }

    private static IPeriodicTimerExecutorFactory BuildTimerExcutorFactory(string intervals)
    {
        var services = new ServiceCollection();
        services.AddSingleton<ILoggerFactory>(new LoggerFactory());
        services.AddSingleton<IPeriodicTimerExecutorFactory, PeriodicTimerExecutorFactory>();
        services.AddSingleton<IOptions<PeriodicTimerExecutorFactoryOptions>>(new MockOptions<PeriodicTimerExecutorFactoryOptions>()
            .Mock(new PeriodicTimerExecutorFactoryOptions
            {
                Intervals = intervals.Split(",").ToList()
            }).Object);

        var serviceProvider = services.BuildServiceProvider();
        var executorFactory = serviceProvider.GetRequiredService<IPeriodicTimerExecutorFactory>();
        return executorFactory;
    }
}
