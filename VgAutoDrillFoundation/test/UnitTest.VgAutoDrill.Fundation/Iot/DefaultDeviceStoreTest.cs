using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UnitTest.VgAutoDrill.Fundation.Mock;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Store;

namespace UnitTest.VgAutoDrill.Fundation.Iot;

public class DefaultDeviceStoreTest
{
    [Fact]
    public void ConstructDeviceStoreShouldWork()
    {
        IDeviceStore store = ConstructDeviceStore();
        Assert.NotNull(store);
    }

    [Fact]
    public async Task SaveDevicePayloadPanelListShouldWork()
    {
        IDeviceStore store = ConstructDeviceStore();
        MockDevice mockDevice = DeviceTest.ConstructMockDevice();
        mockDevice.PayloadPanels.ChangeListSafely("", Task.Run(() =>
        {
            mockDevice.PayloadPanels.AddRange(new[] {
                new Panel { PanelCode = "0001" },
                new Panel { PanelCode = "0002" },
                new Panel { PanelCode = "0003" },
                new Panel { PanelCode = "0004" },
                new Panel { PanelCode = "0005" } });
        }));
        mockDevice.SchedulingTasks.Enqueue(new DeviceServiceInvokeRequest
        {
            DeviceId = "agv1",
            //CallerInputCapabilities = new() { ProductStatus.EmptyPayload }
        }, 1);

        await store.SavePayloadPanels(mockDevice);
        await store.SaveScheduleTasks(mockDevice);
        await store.SavePayloadCutterTrays(mockDevice);

        mockDevice.PayloadPanels.Clear();
        mockDevice.SchedulingTasks.Clear();
        mockDevice.PayloadCutterTrays.Clear();

        await store.LoadPayloadPanels(mockDevice);
        await store.LoadScheduleTasks(mockDevice);
        await store.LoadPayloadCutterTrays(mockDevice);

        Assert.Equal(5, mockDevice.PayloadPanels.Count);
        Assert.Equal(1, mockDevice.SchedulingTasks.Count);
        Assert.Equal(0, mockDevice.PayloadCutterTrays.Count);
    }

    [Fact]
    public async Task TestLoadAndSavePayloadPanelList()
    {
        IDeviceStore store = ConstructDeviceStore();
        MockDevice mockDevice = DeviceTest.ConstructMockDevice();
        mockDevice.PayloadPanels.ChangeListSafely("", Task.Run(() =>
        {
            mockDevice.PayloadPanels.AddRange(new[] {
                new Panel { LocationCode="MockLocationCode1", PanelCode = "0001" },
                new Panel { LocationCode="MockLocationCode1", PanelCode = "0002" },
                new Panel { LocationCode="MockLocationCode1", PanelCode = "0003" },
                new Panel { LocationCode="MockLocationCode1", PanelCode = "0004" },
                new Panel { LocationCode="MockLocationCode1", PanelCode = "0005" } });
        }));

        await store.SavePayloadPanels(mockDevice.PayloadPanels);

        mockDevice.PayloadPanels.Clear();

        await store.LoadPayloadPanels("MockLocationCode1", mockDevice.PayloadPanels);

        Assert.Equal(5, mockDevice.PayloadPanels.Count);
    }

    [Fact]
    public async Task LoadAndSavePayloadPanelList_WithMultipleThreads_NoException()
    {
        IDeviceStore store = ConstructDeviceStore();
        List<Task> tasks1 = new List<Task>();
        List<Task> tasks2 = new List<Task>();
        for (int i = 0; i < 100; i++)
        {
            var t = i;

            var writeTask = Task.Run(async () =>
               {
                   PanelList panels = new PanelList();
                   panels.AddRange(new[]
                   {
                        new Panel { LocationCode="MockLocationCode1"+t, PanelCode = "0001" },
                        new Panel { LocationCode="MockLocationCode1"+t, PanelCode = "0002" },
                        new Panel { LocationCode="MockLocationCode1"+t, PanelCode = "0003" },
                        new Panel { LocationCode="MockLocationCode1"+t, PanelCode = "0004" },
                        new Panel { LocationCode="MockLocationCode1"+t, PanelCode = "0005" }
                   });

                   await store.SavePayloadPanels(panels);
               });

            tasks1.Add(writeTask);

            var readerTask = Task.Run(async () =>
               {
                   PanelList panels = new PanelList();
                   await store.LoadPayloadPanels("MockLocationCode1" + t, panels);
                   //Assert.Equal(5, panels.Count);
               });
            tasks2.Add(readerTask);
        }

        Task.WaitAll(tasks1.ToArray());
        Task.WaitAll(tasks2.ToArray());

        for (int i = 0; i < 100; i++)
        {
            PanelList panels = new PanelList();
            await store.LoadPayloadPanels("MockLocationCode1" + i, panels);
            Assert.Equal(5, panels.Count);
        }
    }

    private static IDeviceStore ConstructDeviceStore()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IOptions<DeviceStoreOptions>>(new MockOptions<DeviceStoreOptions>().Mock(new DeviceStoreOptions()
        {
            SavePath = "data",
            Enabled = true,
        }).Object);
        services.AddSingleton<ILoggerFactory>(new LoggerFactory());
        services.AddSingleton<IDeviceStore, DefaultDeviceStore>();
        var serviceProvider = services.BuildServiceProvider();
        var store = serviceProvider.GetRequiredService<IDeviceStore>();
        return store;
    }
}
