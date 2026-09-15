using Mediator.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using UnitTest.VgAutoDrill.Central.Mock;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Application.Redis;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Central.Configurations;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Handler.Alarm;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Mysql;
using VgAutoDrill.Central.Core.Reporter;
using VgAutoDrill.Central.Core.Reporter.Event;
using VgAutoDrill.Central.Core.Schedule;
using VgAutoDrill.Central.Core.Schedule.Deliver;
using VgAutoDrill.Fundation.Drill;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.Infrastructure.Clickhouse;

namespace UnitTest.VgAutoDrill.Central;

public class CentralTestBase
{
    public T CreateDeviceProxy<T>(IDeviceProxyFactory deviceFactory, string deviceId, string productId, DeviceKind deviceKind)
        where T : DeviceProxy
    {
        var panelForkDescriptor = new DeviceDescriptor
        {
            DeviceId = deviceId,
            ProductId = productId,
            DeviceKind = deviceKind
        };
        var panelFork = deviceFactory.Create(panelForkDescriptor);
        panelFork.Descriptor = panelForkDescriptor;
        panelFork.Status = DeviceStatus.Online;
        return (T)panelFork;
    }

    public ServiceCollection ConstructRequiredServiceCollection()
    {
        var services = new ServiceCollection();
        services.AddAutoMapperConfiguration();
        services.AddSingleton<IDeviceAndRouteService>(new Mock<IDeviceAndRouteService>().Object);
        services.AddSingleton<IDeviceService>(new Mock<IDeviceService>().Object);
        services.AddSingleton<IDeviceProxyFactory>(new Mock<IDeviceProxyFactory>().Object);
        services.AddSingleton<IAsyncTaskWaiter>(new Mock<IAsyncTaskWaiter>().Object);
        services.AddSingleton<IRedisClient>(new Mock<IRedisClient>().Object);
        services.AddSingleton<IDeviceManager>(new Mock<IDeviceManager>().Object);
        services.AddSingleton<IDeviceServiceInvoker>(new Mock<IDeviceServiceInvoker>().Object);
        services.AddSingleton<ISysConfigManager>(new Mock<ISysConfigManager>().Object);

        services.AddSingleton<ITaskScheduleStrategy>(new Mock<ITaskScheduleStrategy>().Object);
        services.AddSingleton<IScheduleTaskDeliverPolicyFactory>(new Mock<IScheduleTaskDeliverPolicyFactory>().Object);
        services.AddSingleton<IScheduleTaskManager>(new Mock<IScheduleTaskManager>().Object);
        services.AddSingleton<IDeviceEventReporter>(new Mock<IDeviceEventReporter>().Object);
        services.AddSingleton<IDeviceStatusReporter>(new Mock<IDeviceStatusReporter>().Object);
        services.AddSingleton<IDevicePanelReporter>(new Mock<IDevicePanelReporter>().Object);
        services.AddSingleton<IDeviceCutterReporter>(new Mock<IDeviceCutterReporter>().Object);
        services.AddSingleton<IDeviceServiceReporter>(new Mock<IDeviceServiceReporter>().Object);
        services.AddSingleton<IDeviceAlarmReporter>(new Mock<IDeviceAlarmReporter>().Object);
        services.AddSingleton<IDevicePropertyReporter>(new Mock<IDevicePropertyReporter>().Object);
        services.AddSingleton<IDeviceLoginReporter>(new Mock<IDeviceLoginReporter>().Object);
        services.AddSingleton<IAlarmLogManager>(new Mock<IAlarmLogManager>().Object);
        services.AddSingleton<IRouteProcessAndWorkStationService>(new Mock<IRouteProcessAndWorkStationService>().Object);

        services.AddSingleton<IObjectFactory>(new Mock<IObjectFactory>().Object);
        services.AddSingleton<IDeviceAlarmDispatcher>(new Mock<IDeviceAlarmDispatcher>().Object);
        services.AddSingleton<IClickHouseConnector>(new Mock<IClickHouseConnector>().Object);
        services.AddSingleton<IClickHouseLogger>(new Mock<IClickHouseLogger>().Object);
        services.AddSingleton<IDeviceServiceInvocationLogger>(new Mock<IDeviceServiceInvocationLogger>().Object);
        services.AddSingleton<IDeviceEventDelegatorFactory>(new Mock<IDeviceEventDelegatorFactory>().Object);
        services.AddSingleton<IWorkOrderTaskAdapter>(new Mock<IWorkOrderTaskAdapter>().Object);
        services.AddSingleton<IScheduleTaskAdapter>(new Mock<IScheduleTaskAdapter>().Object);
        services.AddSingleton<IItemAdapter>(new Mock<IItemAdapter>().Object);
        services.AddSingleton<IAPIHelper>(new Mock<IAPIHelper>().Object);
        services.AddSingleton<IPanelGeneratorAdapter>(new Mock<IPanelGeneratorAdapter>().Object);
        services.AddSingleton<ISiloAdapter>(new Mock<ISiloAdapter>().Object);
        services.AddSingleton<ILocationAdapter>(new Mock<ILocationAdapter>().Object);
        services.AddSingleton<IDrillFilePathLocator>(new Mock<IDrillFilePathLocator>().Object);
        services.AddSingleton<ILockKeeper>(new Mock<ILockKeeper>().Object);
        services.AddSingleton<IScheduleTaskListener>(new Mock<IScheduleTaskListener>().Object);
        services.AddSingleton<ITransferJobAdapter>(new Mock<ITransferJobAdapter>().Object);
        services.AddSingleton<IDeviceAdapter>(new Mock<IDeviceAdapter>().Object);
        services.AddSingleton<IPartitionAdapter>(new Mock<IPartitionAdapter>().Object);
        services.AddSingleton<IRestPointAdapter>(new Mock<IRestPointAdapter>().Object);
        services.AddSingleton<CentralFlags>(sp => ActivatorUtilities.CreateInstance<CentralFlags>(sp));

        //register manager
        services.AddSingleton<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>>(new Mock<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>>().Object);
        services.AddSingleton<IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask>>(new Mock<IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask>>().Object);
        services.AddSingleton<IPanelAgvManager<TransferSiloAgv>>(new Mock<IPanelAgvManager<TransferSiloAgv>>().Object);
        services.AddSingleton<IPanelAgvManager<BackPanelAgv>>(new Mock<IPanelAgvManager<BackPanelAgv>>().Object);
        services.AddSingleton<IDrillManager>(new Mock<IDrillManager>().Object);
        services.AddSingleton<IPinManager>(new Mock<IPinManager>().Object);
        services.AddSingleton<IUnpinManager>(new Mock<IUnpinManager>().Object);
        services.AddSingleton<ILocationManager>(new Mock<ILocationManager>().Object);
        services.AddSingleton<ITransferPlanManager>(new Mock<ITransferPlanManager>().Object);
        services.AddSingleton<IPartitionManager>(new Mock<IPartitionManager>().Object);
        services.AddSingleton<IMediator>(new Mock<IMediator>().Object);
        services.AddSingleton<IOptions<MysqlTaskSchedulerOptions>>(new MockOptions<MysqlTaskSchedulerOptions>().Mock(new MysqlTaskSchedulerOptions()
        {
        }).Object);
        services.AddSingleton<ILoggerFactory>(new LoggerFactory());
        return services;
    }
}
