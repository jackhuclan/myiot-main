using Mediator.Net;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.MicrosoftDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Event;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
namespace UnitTest.VgAutoDrill.Central;

public class PanelChangedEventTest : CentralTestBase
{
    [Fact]
    public void PanelsChangedShouldTriggerPanelChangedEvent()
    {
        var serviceCollection = ConstructRequiredServiceCollection();
        var mediatorBuilder = new MediatorBuilder();
        mediatorBuilder.RegisterHandlers(typeof(PanelChangedEventTest).Assembly);
        serviceCollection.RegisterMediator(mediatorBuilder);

        serviceCollection.AddSingleton<TestLogger>();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var logger = serviceProvider.GetRequiredService<TestLogger>();

        var location = new Location();
        location.OnPanelChanged += (loc) =>
        {
            return mediator.PublishAsync(new PanelChangedEvent(loc));
        };

        var panelList = Panel.HasSilo.HasPanelForSingleSpindle("40001", 1, 20, ProductStatus.EmptySiloBox);
        location.SetPanels(panelList);
        Assert.Equal(3, logger.Logs.Count());

        var panelList2 = Panel.HasSilo.HasPanelForSingleSpindle("40001", 1, 20, ProductStatus.Finished_PIN);
        location.SetPanels(panelList2);
        Assert.Equal(6, logger.Logs.Count());
    }

    [Fact]
    public async Task DeviceProxySetPanelsShouldTriggerPanelChangedEvent()
    {
        var serviceProvider = BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var logger = serviceProvider.GetRequiredService<TestLogger>();

        var deviceFactory = serviceProvider.GetRequiredService<IDeviceProxyFactory>();
        var deviceManager = serviceProvider.GetRequiredService<IDeviceManager>();
        var panelFork = CreateDeviceProxy<PanelSiloFork>(deviceFactory, "PanelFork", "PanelFork", DeviceKind.PanelSiloFork);
        Assert.NotNull(panelFork);
        await deviceManager.AddOrUpdateDevice(panelFork);

        var locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        await locationManager.Refresh();

        for (var i = 1; i <= 6; i++)
        {
            var panelList = Panel.HasSilo.HasPanelForSpindleFirst("abc", 20 * i, 20, ProductStatus.EmptySiloBox);
            await panelFork.SetPanels(panelList);
            Assert.Equal(20 * i, locationManager.GetDevicePanels(panelFork.DeviceId).Count);
            Assert.Equal(3 * i, logger.Logs.Count());
        }
    }

    private IServiceProvider BuildServiceProvider()
    {
        var mockDeviceService = new Mock<IDeviceService>();
        mockDeviceService.Setup(x => x.IsExistByDeviceId(It.IsAny<string>())).Returns(Task.FromResult(true));
        mockDeviceService.Setup(x => x.UpdateStatus(It.IsAny<DeviceDescriptor>(), It.IsAny<DeviceStatus>()))
            .Returns(Task.FromResult(new ResponseDto<DeviceInfoDto>()));

        var locationAdapterMock = new Mock<ILocationAdapter>();
        locationAdapterMock.Setup(x => x.GetLocations()).Returns(Task.FromResult(new List<Location> {
            new Location{ Code = "PanelFork001", DeviceId = "PanelFork"},
            new Location{ Code = "PanelFork002", DeviceId = "PanelFork"},
            new Location{ Code = "PanelFork003", DeviceId = "PanelFork"},
            new Location{ Code = "PanelFork004", DeviceId = "PanelFork"},
            new Location{ Code = "PanelFork005", DeviceId = "PanelFork"},
            new Location{ Code = "PanelFork006", DeviceId = "PanelFork"},
            new Location{ Code = "PanelShelf001", DeviceId = "PanelShelf"},
            new Location{ Code = "PanelShelf002", DeviceId = "PanelShelf"},
            new Location{ Code = "Pin001", DeviceId = "Pin"},
            new Location{ Code = "Pin002", DeviceId = "Pin"},
            new Location{ Code = "UnPin001", DeviceId = "UnPin"},
            new Location{ Code = "UnPin002", DeviceId = "UnPin"},
        }));

        var serviceCollection = ConstructRequiredServiceCollection();
        serviceCollection.AddSingleton<IDeviceService>(mockDeviceService.Object);
        serviceCollection.AddSingleton<IDeviceProxyFactory, DeviceProxyFactory>();
        serviceCollection.AddSingleton<ILocationManager, LocationManager>();
        serviceCollection.AddSingleton<IDeviceManager, DeviceManager>();
        serviceCollection.AddSingleton<IPartitionAdapter>(new Mock<IPartitionAdapter>().Object);
        serviceCollection.AddSingleton<ILocationAdapter>(locationAdapterMock.Object);

        var mediatorBuilder = new MediatorBuilder();
        mediatorBuilder.RegisterHandlers(typeof(PanelChangedEventTest).Assembly);
        serviceCollection.RegisterMediator(mediatorBuilder);

        serviceCollection.AddSingleton<TestLogger>();

        return serviceCollection.BuildServiceProvider();
    }
}

public class TestLogger
{
    private List<string> _logs = new List<string>();

    public List<string> Logs => _logs;

    public void AddLog(string message)
    {
        Logs.Add(message);
    }
}

public class Event1 : IEventHandler<PanelChangedEvent>
{
    private readonly TestLogger _testLogger;

    public Event1(TestLogger testLogger)
    {
        _testLogger = testLogger;
    }

    public Task Handle(IReceiveContext<PanelChangedEvent> context, CancellationToken cancellationToken)
    {
        _testLogger.AddLog("Event1 handled");
        return Task.CompletedTask;
    }
}

public class Event2 : IEventHandler<PanelChangedEvent>
{
    private readonly TestLogger _testLogger;

    public Event2(TestLogger testLogger)
    {
        _testLogger = testLogger;
    }

    public Task Handle(IReceiveContext<PanelChangedEvent> context, CancellationToken cancellationToken)
    {
        _testLogger.AddLog("Event2 handled");
        return Task.CompletedTask;
    }
}

public class Event3 : IEventHandler<PanelChangedEvent>
{
    private readonly TestLogger _testLogger;

    public Event3(TestLogger testLogger)
    {
        _testLogger = testLogger;
    }

    public Task Handle(IReceiveContext<PanelChangedEvent> context, CancellationToken cancellationToken)
    {
        _testLogger.AddLog("Event3 handled");
        throw new Exception("test");
    }
}
