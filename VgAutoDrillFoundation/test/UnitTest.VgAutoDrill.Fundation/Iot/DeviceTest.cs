using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using UnitTest.VgAutoDrill.Fundation.Mock;
using VgAutoDrill.Fundation.Alarm;

using VgAutoDrill.Fundation.Channel;

using VgAutoDrill.Fundation.Channel;

using VgAutoDrill.Fundation.Drill;
using VgAutoDrill.Fundation.Event;
using VgAutoDrill.Fundation.Interaction;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Mqtt.Client;
using VgAutoDrill.Fundation.Pipe;
using VgAutoDrill.Fundation.Property;
using VgAutoDrill.Fundation.Schedule;
using VgAutoDrill.Fundation.State;
using VgAutoDrill.Fundation.Store;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.OpenAPI;
using ObjectFactory = VgAutoDrill.Infrastructure.ObjectFactory;

namespace UnitTest.VgAutoDrill.Fundation.Iot;

public class DeviceTest
{
    [Fact]
    public void DeviceCouldBeConstructed()
    {
        MockDevice mockDevice = ConstructMockDevice();

        Assert.NotNull(mockDevice);
        Assert.NotNull(mockDevice.PropertyHandler);
        Assert.NotNull(mockDevice.PropertyHandler.ApplicationServices);
        Assert.Same(mockDevice, ((MockPropertyHandler)mockDevice.PropertyHandler).InteractingDevice);
        Assert.Same(mockDevice.PayloadPanels, ((MockPropertyHandler)mockDevice.PropertyHandler).InteractingDevice.PayloadPanels);

        Assert.NotNull(mockDevice.AlarmHandler);
        Assert.NotNull(mockDevice.AlarmHandler.ApplicationServices);
        Assert.Same(mockDevice, ((MockAlarmHandler)mockDevice.AlarmHandler).InteractingDevice);
        Assert.Same(mockDevice.PayloadPanels, ((MockAlarmHandler)mockDevice.AlarmHandler).InteractingDevice.PayloadPanels);

        Assert.NotNull(mockDevice.EventHandler);
        Assert.NotNull(mockDevice.EventHandler.ApplicationServices);
        Assert.Same(mockDevice, ((MockEventHandler)mockDevice.EventHandler).InteractingDevice);
        Assert.Same(mockDevice.PayloadPanels, ((MockEventHandler)mockDevice.EventHandler).InteractingDevice.PayloadPanels);

        Assert.NotNull(mockDevice.StateHandler);
        Assert.NotNull(mockDevice.StateHandler.ApplicationServices);
        Assert.Same(mockDevice, ((MockStateHandler)mockDevice.StateHandler).InteractingDevice);

        Assert.NotNull(mockDevice.ScheduleHandler);
        Assert.NotNull(mockDevice.ScheduleHandler.ApplicationServices);
        Assert.Same(mockDevice, ((MockScheduleHandler)mockDevice.ScheduleHandler).InteractingDevice);

        Assert.NotNull(mockDevice.LoadMaterialPolicy);
        Assert.NotNull(mockDevice.LoadMaterialPolicy.ApplicationServices);
        Assert.Same(mockDevice, ((MockLoadMaterialInteractionPolicy)mockDevice.LoadMaterialPolicy).InteractingDevice);
        Assert.Same(mockDevice.PayloadPanels, ((MockLoadMaterialInteractionPolicy)mockDevice.LoadMaterialPolicy).InteractingDevice.PayloadPanels);

        Assert.NotNull(mockDevice.UnloadMaterialPolicy);
        Assert.NotNull(mockDevice.UnloadMaterialPolicy.ApplicationServices);
        Assert.Same(mockDevice, ((MockUnloadMaterialInteractionPolicy)mockDevice.UnloadMaterialPolicy).InteractingDevice);
        Assert.Same(mockDevice.PayloadPanels, ((MockUnloadMaterialInteractionPolicy)mockDevice.UnloadMaterialPolicy).InteractingDevice.PayloadPanels);

        var mockDeviceBrother = mockDevice.ObjectFactory.CreateObject<MockDeviceBrother>(mockDevice);
        Assert.NotNull(mockDeviceBrother);
    }

    public static MockDevice ConstructMockDevice()
    {
        var services = ConstructDeviceRequiredServiceCollection();
        var deviceDescriptor = new DeviceDescriptor
        {
            DeviceId = "MockDevice01",
            ProductId = "Mock",
        };
        var mockDevice = new MockDevice(deviceDescriptor, new MockDeviceEngine().Mock().Object, services.BuildServiceProvider());
        return mockDevice;
    }

    [Fact]
    public void CollectionChangedShouldBeRaised_WhenPayloadPanelsChanged()
    {
        var services = ConstructDeviceRequiredServiceCollection();
        var deviceDescriptor = new DeviceDescriptor
        {
            DeviceId = "MockDevice01",
            ProductId = "Mock",
        };
        var mockDevice = new MockDevice(deviceDescriptor, new MockDeviceEngine().Mock().Object, services.BuildServiceProvider());

        mockDevice.PayloadPanels.Add(new Panel() { PanelCode = "0001" });
        mockDevice.PayloadPanels.RaiseCollectionChangedEvent(string.Empty);
        Assert.Equal(1, mockDevice.PanelChangedCount);
    }

    public static ServiceCollection ConstructDeviceRequiredServiceCollection()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IHttpRequestInvoker>(new MockHttpRequestInvoker().Mock(ErrorCodes.Sys.SUCCESS).Object);
        services.AddSingleton<IDeviceProvider>(new MockDeviceProvider().Mock().Object);
        services.AddSingleton<IMessageChannel>(new MockDataExporter().Mock().Object);
        services.AddSingleton<IEventHandlerContainer>(sp => new EventHandlerContainer(sp));
        services.AddSingleton<IStateHandlerContainer>(sp => new StateHandlerContainer(sp));
        services.AddSingleton<IPropertyHandlerContainer>(sp => new PropertyHandlerContainer(sp));
        services.AddSingleton<IAlarmHandlerContainer>(sp => new AlarmHandlerContainer(sp));
        services.AddSingleton<IScheduleHandlerContainer>(sp => new ScheduleHandlerContainer(sp));
        services.AddSingleton<IObjectFactory>(sp => new ObjectFactory(sp));
        services.AddSingleton<IMessageChannelFactory>(sp => new MessageChannelFactory(sp));
        services.AddSingleton<IMqttClientWrapper>(new MockMqttClientWrapper().Mock().Object);
        services.AddSingleton<IDeviceStore>(new Mock<IDeviceStore>().Object);
        services.AddSingleton<IDrillFilePathLocator>(new Mock<IDrillFilePathLocator>().Object);
        services.AddSingleton<IHostApplicationLifetime>(new Mock<IHostApplicationLifetime>().Object);
        services.AddSingleton<IOptions<CentralWebOptions>>(new MockOptions<CentralWebOptions>().Mock(new CentralWebOptions()
        {
            Channel = "local"
        }).Object);
        services.AddSingleton<IOptions<ClickHouseOptions>>(new MockOptions<ClickHouseOptions>().Mock(new ClickHouseOptions()
        {
        }).Object);
        services.AddSingleton<ILoggerFactory>(new LoggerFactory());
        services.AddSingleton<IPeriodicTimerExecutorFactory>(new Mock<IPeriodicTimerExecutorFactory>().Object);
        return services;
    }
}

public class MockDeviceBrother
{
    public MockDeviceBrother(MockDevice mockDevice)
    {
    }
}

public class MockDevice : Device
{
    public IPropertyHandler PropertyHandler { get; }
    public IAlarmHandler AlarmHandler { get; }
    public IEventHandler EventHandler { get; }
    public IStateHandler StateHandler { get; }
    public IScheduleHandler ScheduleHandler { get; }
    public ILoadMaterialInteractionPolicy LoadMaterialPolicy { get; }
    public IUnloadMaterialInteractionPolicy UnloadMaterialPolicy { get; }

    public int PanelChangedCount = 0;

    public MockDevice(DeviceDescriptor deviceDescriptor,
        IDeviceEngine deviceEngine,
        IServiceProvider serviceProvider)
        : base(deviceDescriptor, deviceEngine, serviceProvider)
    {
        this.CollectDataFunc = (d) => this.PropertyContainer[d.GetType()].CollectPropertyValues();

        PropertyHandler = this.PropertyContainer.AddHandler<MockPropertyHandler, MockDevice>(this);
        AlarmHandler = this.AlarmContainer.AddHandler<MockAlarmHandler, MockDevice>(this);
        EventHandler = this.EventContainer.AddHandler<MockEventHandler, MockDevice>(this);
        StateHandler = this.StateContainer.AddHandler<MockStateHandler, MockDevice>(this);
        ScheduleHandler = this.ScheduleContainer.AddHandler<MockScheduleHandler, MockDevice>(this);

        LoadMaterialPolicy = this.ObjectFactory.CreateObject<MockLoadMaterialInteractionPolicy>(this);
        UnloadMaterialPolicy = this.ObjectFactory.CreateObject<MockUnloadMaterialInteractionPolicy>(this);
    }

    protected override async Task<DevicePanelChangedResponse> OnPanelsChanged(string locationCode)
    {
        PanelChangedCount++;
        return await base.OnPanelsChanged(locationCode);
    }

    public override Task<DeviceServiceInvokeResponse> CompleteLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        throw new NotImplementedException();
    }

    public override Task<DeviceServiceInvokeResponse> CompleteUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        throw new NotImplementedException();
    }

    public override Task<DeviceServiceInvokeResponse> InvokeLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        throw new NotImplementedException();
    }

    public override Task<DeviceServiceInvokeResponse> InvokeUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        throw new NotImplementedException();
    }

    public override Task<DeviceServiceInvokeResponse> PrepareLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        throw new NotImplementedException();
    }

    public override Task<DeviceServiceInvokeResponse> PrepareUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        throw new NotImplementedException();
    }

    public override Task<DeviceServiceInvokeResponse> ReadProperties(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        throw new NotImplementedException();
    }

    public override Task<DeviceServiceInvokeResponse> ScheduleTask(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        throw new NotImplementedException();
    }

    public override Task<DeviceServiceInvokeResponse> Shutdown(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        throw new NotImplementedException();
    }

    public override Task<DeviceServiceInvokeResponse> Standby(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        throw new NotImplementedException();
    }

    public override Task<DeviceServiceInvokeResponse> Work(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        throw new NotImplementedException();
    }

    public override Task<DeviceServiceInvokeResponse> WriteProperties(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        throw new NotImplementedException();
    }
}

public class MockPropertyHandler : AbstractPropertyHandler<MockDevice>
{
    public MockPropertyHandler(IServiceProvider serviceProvider, MockDevice device) : base(serviceProvider, device)
    {
    }

    public override void AddWatchingProperties()
    { }

    public override void CollectPropertyValues()
    { }
}

public class MockEventHandler : AbstractEventHandler<MockDevice>
{
    public MockEventHandler(IServiceProvider serviceProvider, MockDevice device) : base(serviceProvider, device)
    {
    }

    public override void AddWatchingEvents()
    { }
}

public class MockAlarmHandler : AbstractAlarmHandler<MockDevice>
{
    public MockAlarmHandler(IServiceProvider serviceProvider, MockDevice device) : base(serviceProvider, device)
    {
    }

    public override void AddWatchingAlarms()
    { }
}

public class MockStateHandler : AbstractStateHandler<MockDevice>
{
    public MockStateHandler(IServiceProvider serviceProvider, MockDevice device) : base(serviceProvider, device)
    {
    }

    public override void AddWatchingStates()
    { }
}

public class MockScheduleHandler : AbstractScheduleHandler<MockDevice>
{
    public MockScheduleHandler(IServiceProvider serviceProvider, MockDevice device) : base(serviceProvider, device)
    {
    }

    public override Task<DeviceServiceInvokeResponse> ScheduleTask(DeviceServiceInvokeRequest deviceServiceInvokeRequest) => throw new NotImplementedException();
}

public class MockLoadMaterialInteractionPolicy : AbstractLoadMaterialInteractionPolicy<MockDevice>
{
    public MockLoadMaterialInteractionPolicy(IServiceProvider serviceProvider, MockDevice device) : base(serviceProvider, device)
    {
    }

    public override Task<DeviceServiceInvokeResponse> CompleteLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest) => throw new NotImplementedException();

    public override Task<DeviceServiceInvokeResponse> InvokeLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest) => throw new NotImplementedException();

    public override Task<DeviceServiceInvokeResponse> PrepareLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest) => throw new NotImplementedException();
}

public class MockUnloadMaterialInteractionPolicy : AbstractUnloadMaterialInteractionPolicy<MockDevice>
{
    public MockUnloadMaterialInteractionPolicy(IServiceProvider serviceProvider, MockDevice device) : base(serviceProvider, device)
    {
    }

    public override Task<DeviceServiceInvokeResponse> CompleteUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest) => throw new NotImplementedException();

    public override Task<DeviceServiceInvokeResponse> InvokeUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest) => throw new NotImplementedException();

    public override Task<DeviceServiceInvokeResponse> PrepareUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest) => throw new NotImplementedException();
}
