using Microsoft.Extensions.DependencyInjection;
using Moq;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using Panel = VgAutoDrill.Fundation.Iot.Models.Panel;
namespace UnitTest.VgAutoDrill.Central;

public class DeviceProxyTest : CentralTestBase
{
    [Fact]
    public void ConstructDeviceProxyShouldWork()
    {
        var serviceCollection = ConstructRequiredServiceCollection();
        serviceCollection.AddSingleton<IDeviceProxyFactory, DeviceProxyFactory>();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var deviceFactory = serviceProvider.GetRequiredService<IDeviceProxyFactory>();
        var instance = deviceFactory.Create(new DeviceDescriptor
        {
            DeviceId = "PanelFork",
            ProductId = "PanelFork",
            DeviceKind = DeviceKind.PanelSiloFork
        });

        Assert.NotNull(instance);
    }

    [Fact]
    public async Task DeviceProxySetPanelsShouldWork()
    {
        var serviceProvider = BuildServiceProvider();
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
        }
    }

    [Fact]
    public async Task VerifyProductStatusOfDifferentPostion()
    {
        var serviceProvider = BuildServiceProvider();
        var deviceFactory = serviceProvider.GetRequiredService<IDeviceProxyFactory>();
        var deviceManager = serviceProvider.GetRequiredService<IDeviceManager>();
        var locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        var panelFork = CreateDeviceProxy<PanelSiloFork>(deviceFactory, "PanelFork", "PanelFork", DeviceKind.PanelSiloFork);
        Assert.NotNull(panelFork);
        await deviceManager.AddOrUpdateDevice(panelFork);

        PanelList listAll = new PanelList();
        for (var i = 1; i <= 6; i++)
        {
            var siloCode = (40000 + i).ToString();
            var panelList = Panel.HasSilo.NoPanelForSingleSpindle(siloCode, i, 0, 20);
            await panelFork.SetPanels(panelList);
            listAll.AddRange(panelList);
        }

        Assert.Equal(6, locationManager.SiloCount());

        listAll.UpdatePanelInfo(2, ProductStatus.Finished_DRILL);
        listAll.UpdatePanelInfo(3, ProductStatus.Finished_DRILL);
        listAll.UpdatePanelInfo(4, ProductStatus.Finished_PIN);

        for (var i = 1; i <= 6; i++)
        {
            var siloCode = (40000 + i).ToString();
            Assert.Equal(1, locationManager.SiloCount(siloCode));
            Assert.Equal(20, locationManager.PanelCount(siloCode));
        }

        Assert.Equal(20 * 3, locationManager.GetEmptyLayerCount());
        Assert.Equal(20 * 3, locationManager.GetDeviceEmptyLayerCount(panelFork.DeviceId));
        Assert.Equal(20, locationManager.PanelCount(ProductStatus.Finished_PIN));
        Assert.Equal(20 * 2, locationManager.PanelCount(ProductStatus.Finished_DRILL));
    }

    [Fact]
    public async Task DeviceProxySetPanelsShouldWork_AfterResettingSiloCode()
    {
        var serviceProvider = BuildServiceProvider();
        var deviceFactory = serviceProvider.GetRequiredService<IDeviceProxyFactory>();
        var deviceManager = serviceProvider.GetRequiredService<IDeviceManager>();
        var locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        var panelFork = CreateDeviceProxy<PanelSiloFork>(deviceFactory, "PanelFork", "PanelFork", DeviceKind.PanelSiloFork);
        Assert.NotNull(panelFork);
        await deviceManager.AddOrUpdateDevice(panelFork);

        for (var i = 1; i <= 6; i++)
        {
            var siloCode = (40000 + i).ToString();
            var panelList = Panel.HasSilo.HasPanelForSingleSpindle(siloCode, i, 20, ProductStatus.Finished_DRILL);
            await panelFork.SetPanels(panelList);
        }

        for (var i = 1; i <= 6; i++)
        {
            var locationCode = $"PanelFork{i.ToString().PadLeft(3, '0')}";
            var siloCode = (40000 + i).ToString();

            Assert.Equal(6, locationManager.SiloCount());
            Assert.Equal(1, locationManager.SiloCount(siloCode));

            var found = locationManager.TryFindLocationWithSiloCode(siloCode, out var location);
            Assert.True(found);
            Assert.NotNull(location);
            Assert.NotNull(location.Code);
            Assert.Equal(i, location.Position);
            Assert.Equal(locationCode, location.Code);

            Assert.Equal(20, locationManager.PanelCount(siloCode));
            Assert.Equal(20 * 6, locationManager.PanelCount(ProductStatus.Finished_DRILL));
            Assert.Equal(20 * 6, locationManager.PanelCount(new List<ProductStatus> { ProductStatus.Finished_DRILL }.AsReadOnly()));
            Assert.Equal(20, locationManager.PanelCount(siloCode, ProductStatus.Finished_DRILL));
            Assert.Equal(20, locationManager.PanelCount(siloCode, new List<ProductStatus> { ProductStatus.Finished_DRILL }.AsReadOnly()));
            Assert.Equal(20, locationManager.PanelCount(panelFork.DeviceId, siloCode, ProductStatus.Finished_DRILL));
            Assert.Equal(20, locationManager.PanelCount(panelFork.DeviceId, siloCode, new List<ProductStatus> { ProductStatus.Finished_DRILL }.AsReadOnly()));
            Assert.Equal(20, locationManager.PanelCount(siloCode, ProductStatus.Finished_DRILL));
        }
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(6, 20)]
    public async Task SetPanelsShouldAssignToRightLocation(int spindles, int layerLimit)
    {
        var serviceProvider = BuildServiceProvider();
        var deviceFactory = serviceProvider.GetRequiredService<IDeviceProxyFactory>();
        var deviceManager = serviceProvider.GetRequiredService<IDeviceManager>();
        var locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        var panelFork = CreateDeviceProxy<PanelSiloFork>(deviceFactory, "PanelFork", "PanelFork", DeviceKind.PanelSiloFork);
        Assert.NotNull(panelFork);
        await deviceManager.AddOrUpdateDevice(panelFork);

        for (var i = 1; i <= spindles; i++)
        {
            var siloCode = (40000 + i).ToString();
            var panelList = Panel.HasSilo.HasPanelForSingleSpindle(siloCode, i, layerLimit, ProductStatus.Finished_DRILL);
            await panelFork.SetPanels(panelList);
        }

        var forkLocations = locationManager.Locations.Where(x => x.DeviceId == "PanelFork").ToList();

        Assert.Equal(spindles, locationManager.SiloCount());
        for (int i = 1; i <= spindles; i++)
        {
            var siloCode = (40000 + i).ToString();
            var location = locationManager.GetLocation(panelFork.DeviceId, i);
            Assert.NotNull(location);
            Assert.Equal(location.SiloCode, siloCode);
            Assert.Equal(layerLimit, location.Panels.Count);
            Assert.Equal($"PanelFork{i.ToString().PadLeft(3, '0')}", location.Code);
        }
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(6, 20)]
    public async Task MultiThreadSetPanelsShouldAssignToRightLocation(int spindles, int layerLimit)
    {
        var serviceProvider = BuildServiceProvider();
        var deviceFactory = serviceProvider.GetRequiredService<IDeviceProxyFactory>();
        var deviceManager = serviceProvider.GetRequiredService<IDeviceManager>();
        var locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        var panelFork = CreateDeviceProxy<PanelSiloFork>(deviceFactory, "PanelFork", "PanelFork", DeviceKind.PanelSiloFork);
        Assert.NotNull(panelFork);
        await deviceManager.AddOrUpdateDevice(panelFork);

        var list = new List<Task>();
        var ewh = new EventWaitHandle(false, EventResetMode.ManualReset);
        for (int loop = 0; loop < 200; loop++)
        {
            var task = Task.Factory.StartNew(async (obj) =>
            {
                ewh.WaitOne();

                Func<object, int, string> getSiloCode = (a, b) => (((int)a + 1) * 100 + b).ToString().PadLeft(5, '0');

                for (var i = 1; i <= spindles; i++)
                {
                    var siloCode = getSiloCode(obj!, i);
                    var panelList = Panel.HasSilo.NoPanelForSingleSpindle(siloCode, i, 0, layerLimit);
                    await panelFork.SetPanels(panelList);
                }

                var forkLocations = locationManager.Locations.Where(x => x.DeviceId == "PanelFork").ToList();

                Assert.Equal(spindles, locationManager.SiloCount());
                for (int i = 1; i <= spindles; i++)
                {
                    var siloCode = getSiloCode(obj!, i);
                    var location = locationManager.GetLocation(panelFork.DeviceId, i);
                    Assert.NotNull(location);
                    Assert.True(location.HasSilo);
                    Assert.Equal(layerLimit, location.Panels.Count);
                    Assert.Equal($"PanelFork{i.ToString().PadLeft(3, '0')}", location.Code);
                    Assert.Equal(siloCode, location.SiloCode);
                }
            }, loop);

            list.Add(task);
        }
        ewh.Set();

        Task.WaitAll(list.ToArray());
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
        return serviceCollection.BuildServiceProvider();
    }
}
