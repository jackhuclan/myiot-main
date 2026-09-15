using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using VgAutoDrill.Fundation.Alarm;
using VgAutoDrill.Fundation.Channel;
using VgAutoDrill.Fundation.Drill;
using VgAutoDrill.Fundation.Event;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Mqtt.Client;
using VgAutoDrill.Fundation.Pipe;
using VgAutoDrill.Fundation.Property;
using VgAutoDrill.Fundation.State;
using VgAutoDrill.Fundation.Store;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.OpenAPI;
using ObjectFactory = VgAutoDrill.Infrastructure.ObjectFactory;

namespace VgAutoDrill.Fundation.Iot;

public static class DeviceSetup
{
    public static void AddPeriodicTimers(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PeriodicTimerExecutorFactoryOptions>(configuration.GetSection(nameof(PeriodicTimerExecutorFactoryOptions)));
        services.AddSingleton<IPeriodicTimerExecutorFactory, PeriodicTimerExecutorFactory>();
    }

    public static void AddHttpRequest(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMqttClient(configuration);
        services.AddHttpClient();
        services.AddSingleton<IHttpRequestInvoker, HttpRequestInvoker>();
    }

    public static void AddDevicesCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MqttServerConnectionOptions>(configuration.GetSection(nameof(MqttServerConnectionOptions)));
        services.Configure<CentralWebOptions>(configuration.GetSection(CentralWebOptions.Options));
        services.Configure<DeviceListOptions>(configuration.GetSection(DeviceListOptions.Options));
        services.Configure<DeviceStoreOptions>(configuration.GetSection(nameof(DeviceStoreOptions)));

        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
        });

        services.AddQuartzHostedService(opt =>
        {
            opt.WaitForJobsToComplete = true;
        });

        var deviceListOptions = configuration.GetSection(DeviceListOptions.Options).Get<DeviceListOptions>() ?? new DeviceListOptions();
        var deviceDescriptorCollection = new DeviceDescriptorCollection();
        foreach (DeviceDescriptor item in deviceListOptions.Devices)
        {
            uint? port = configuration.GetValue<uint?>("port");
            if (!port.HasValue) port = 8004;
            item.HostPort = port.HasValue ? port.Value : 8004;
            item.HostAddress = item.HostAddress.Replace("8004", port.ToString());
            deviceDescriptorCollection.Add(item.DeviceId, item);
        }

        services.AddSingleton<IMqttClientWrapper, MqttClientWrapper>();
        services.AddSingleton<IDeviceProvider, DeviceProvider>(deviceDescriptorCollection.BuildDeviceProvider);
        services.AddSingleton<IEventHandlerContainer, EventHandlerContainer>();
        services.AddSingleton<IStateHandlerContainer, StateHandlerContainer>();
        services.AddSingleton<IPropertyHandlerContainer, PropertyHandlerContainer>();
        services.AddSingleton<IAlarmHandlerContainer, AlarmHandlerContainer>();
        services.AddSingleton<IScheduleHandlerContainer, ScheduleHandlerContainer>();
        services.AddSingleton<IMessageChannelFactory, MessageChannelFactory>();
        services.AddSingleton<IObjectFactory, ObjectFactory>();
        services.AddSingleton<IMqttMessagePublisher, MqttMessagePublisher>();
        services.AddSingleton<IClientMqttListener, DefaultClientMqttListener>();
        services.AddSingleton<IClientMqttApplicationMessageListener, DefaultClientMqttApplicationMessageListener>();
        services.AddSingleton<IAsyncTaskWaiter, AsyncTaskWaiter>();
        services.AddSingleton<IDeviceStore, DefaultDeviceStore>();
        services.AddSingleton<IDeviceConnectorProvider, DefaultDeviceConnectorProvider>();
        services.AddSingleton<IDrillFilePathLocator, EmptyDrillFilePathLocator>();
        services.AddSingleton<ILockKeeper, LockKeeper>();
        services.AddHostedService<EngineStarter>();
    }

    public static void AddDevices(this IServiceCollection services, IConfiguration configuration)
    {
        AddDevicesCore(services, configuration);
        AddHttpRequest(services, configuration);
        AddPeriodicTimers(services, configuration);
        services.AddPipeServer(configuration);
        services.AddPipeClient(configuration);

        services.Configure<ClickHouseOptions>(configuration.GetSection(ClickHouseOptions.Options));
        services.Configure<DeviceGatewayOptions>(configuration.GetSection(nameof(DeviceGatewayOptions)));
        services.AddHostedService<DeviceGatewayMonitor>();
    }
}
