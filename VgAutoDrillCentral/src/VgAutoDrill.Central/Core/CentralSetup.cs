using Mediator.Net;
using Mediator.Net.MicrosoftDependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Admin.Application.Redis;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Central.Core.Calculator;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Flusher;
using VgAutoDrill.Central.Core.Handler.Alarm;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Central.Core.Mes.Adapter;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Mysql;
using VgAutoDrill.Central.Core.Reporter;
using VgAutoDrill.Central.Core.Reporter.Event;
using VgAutoDrill.Central.Core.Schedule;
using VgAutoDrill.Central.Core.Schedule.Deliver;
using VgAutoDrill.Central.Core.Schedule.Handler;
using VgAutoDrill.Central.Core.Worker;
using VgAutoDrill.Fundation.Drill;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.Infrastructure.Clickhouse;
using ObjectFactory = VgAutoDrill.Infrastructure.ObjectFactory;

namespace VgAutoDrill.Central.Core;

public static class CentralSetup
{
    public static void AddCentralCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RedisCacheOptions>(configuration.GetSection(RedisCacheOptions.Options));
        services.Configure<MysqlTaskSchedulerOptions>(configuration.GetSection(MysqlTaskSchedulerOptions.Options));
        services.Configure<ClickHouseOptions>(configuration.GetSection(ClickHouseOptions.Options));
        services.Configure<DeviceAlarmListenerOptions>(configuration.GetSection(DeviceAlarmListenerOptions.Options));
        services.Configure<DeviceServiceInvocationReplayerOptions>(configuration.GetSection(nameof(DeviceServiceInvocationReplayerOptions)));
        services.Configure<AlarmLogPersistOptions>(configuration.GetSection(nameof(AlarmLogPersistOptions)));

        RedisCacheOptions? redisCacheOptions = configuration.GetSection(RedisCacheOptions.Options).Get<RedisCacheOptions>();
        if (redisCacheOptions == null)
        {
            ThrowHelper.ThrowArgumentException(nameof(RedisCacheOptions));
        }

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisCacheOptions.ConnectionString;
            //options.InstanceName = redisCacheOptions.InstanceName;
            options.ConfigurationOptions = StackExchange.Redis.ConfigurationOptions.Parse(redisCacheOptions.ConnectionString);
        });

        var mediatorBuilder = new MediatorBuilder();
        mediatorBuilder.RegisterHandlers(typeof(CentralSetup).Assembly);
        services.RegisterMediator(mediatorBuilder);

        services.AddHttpClient();
        services.AddSingleton<IDeviceProxyFactory, DeviceProxyFactory>();
        services.AddSingleton<IAsyncTaskWaiter, AsyncTaskWaiter>();
        services.AddSingleton<IRedisClient, RedisClient>();
        services.AddSingleton<IDeviceManager, DeviceManager>();

        services.AddSingleton<ITaskScheduleStrategy, MysqlTaskScheduleStrategy>();
        services.AddSingleton<IScheduleTaskDeliverPolicyFactory, ScheduleTaskDeliverPolicyFactory>();
        services.AddSingleton<IScheduleTaskManager, ScheduleTaskManager>();
        services.AddSingleton<IScheduleTracker, ScheduleTracker>();
        services.AddSingleton<IAutoSiloTransferStrategyFactory, AutoSiloTransferStrategyFactory>();

        //register device handdlers
        services.AddSingleton<IDeviceEventReporter, DeviceEventReporter>();
        services.AddSingleton<IDeviceStatusReporter, DeviceStatusReporter>();
        services.AddSingleton<IDevicePanelReporter, DevicePanelReporter>();
        services.AddSingleton<IDeviceCutterReporter, DeviceCutterReporter>();
        services.AddSingleton<IDeviceLoginReporter, DeviceLoginReporter>();
        services.AddSingleton<IDevicePropertyReporter, DevicePropertyReporter>();
        services.AddSingleton<IDeviceServiceReporter, DeviceServiceReporter>();
        services.AddSingleton<IDeviceAlarmReporter, DeviceAlarmReporter>();

        services.AddSingleton<IObjectFactory, ObjectFactory>();
        services.AddSingleton<IDeviceAlarmDispatcher, DeviceAlarmDispatcher>();
        services.AddSingleton<IClickHouseConnector, ClickHouseConnector>();
        services.AddSingleton<IClickHouseLogger, ClickHouseLogger>();
        services.AddSingleton<IDeviceServiceInvocationLogger, DeviceServiceInvocationLogger>();
        services.AddSingleton<IDeviceEventDelegatorFactory, DeviceEventDelegatorFactory>();
        services.AddSingleton<IPinScheduleHandler, PinScheduleHandler>();
        services.AddSingleton<IUnpinScheduleHandler, UnpinScheduleHandler>();
        services.AddHostedService<TaskBackgroundScheduler>();
        services.AddHostedService<PinScheduleWorker>();
        services.AddHostedService<UnPinScheduleWorker>();
        services.AddHostedService<WorkOrderTaskChecker>();
        services.AddHostedService<LoationMissingScheduleChecker>();
        services.AddHostedService<PartitionHeartBeatWorker>();
        services.AddHostedService<SystemPreloader>();
        services.AddHostedService<DeviceServiceInvocationReplayer>();
        services.AddHostedService(sp => sp.GetRequiredService<IAlarmLogManager>());
        services.AddSingleton<IAlarmLogManager, AlarmLogManager>();
        services.AddSingleton<IWorkOrderTaskAdapter, WorkOrderTaskAdapter>();
        services.AddSingleton<IScheduleTaskAdapter, ScheduleTaskAdapter>();
        services.AddSingleton<IItemAdapter, ItemAdapter>();
        services.AddSingleton<IAPIHelper, APIHelper>();
        services.AddSingleton<IPanelGeneratorAdapter, PanelGeneratorAdapter>();
        services.AddSingleton<ISiloAdapter, SiloAdapter>();
        services.AddSingleton<IAlarmAdapter, AlarmAdapter>();
        services.AddSingleton<ILocationAdapter, LocationAdapter>();
        services.AddSingleton<IDrillFilePathLocator, EmptyDrillFilePathLocator>();
        services.AddSingleton<ILockKeeper, LockKeeper>();
        services.AddSingleton<IScheduleTaskListener, ScheduleTaskListener>();
        services.AddSingleton<ITransferJobListener, TransferJobListener>();
        services.AddSingleton<ITransferJobAdapter, TransferJobAdapter>();
        services.AddSingleton<IDeviceAdapter, DeviceAdapter>();
        services.AddSingleton<IPartitionAdapter, PartitionAdapter>();
        services.AddSingleton<IRestPointAdapter, RestPointAdapter>();
        services.AddSingleton<ICutterGroupAdapter, CutterGroupAdapter>();

        services.AddSingleton<CentralFlags>();

        //register manager
        services.AddSingleton<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>, PanelSiloForkManager>();
        services.AddSingleton<IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask>, PanelSiloShelfManager>();
        services.AddSingleton<IPanelAgvManager<TransferSiloAgv>, TransferSiloAgvManager>();
        services.AddSingleton<IPanelAgvManager<BackPanelAgv>, BackPanelAgvManager>();
        services.AddSingleton<IDrillManager, DrillManager>();
        services.AddSingleton<IPinManager, PinManager>();
        services.AddSingleton<IUnpinManager, UnpinManager>();
        services.AddSingleton<ILocationManager, LocationManager>();
        services.AddSingleton<ITransferPlanManager, TransferPlanManager>();
        services.AddSingleton<IPartitionManager, PartitionManager>();

        //register data flusher
        services.AddTransient<ICutterFlusher, CutterFlusher>();
        services.AddTransient<IPropertiesFlusher, PropertiesFlusher>();

        services.AddSingleton<IPanelBarCodeValidator, DefaultPanelBarCodeValidator>();
    }
}
