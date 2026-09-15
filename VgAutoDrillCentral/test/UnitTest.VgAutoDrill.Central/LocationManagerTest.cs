using Microsoft.Extensions.DependencyInjection;
using Moq;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Schedule;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
namespace UnitTest.VgAutoDrill.Central;

public class LocationManagerTest : CentralTestBase
{
    [Fact]
    public async Task TryFindDrillRequiredLocation()
    {
        var mockDeviceService = new Mock<IDeviceService>();
        mockDeviceService.Setup(x => x.IsExistByDeviceId(It.IsAny<string>())).Returns(Task.FromResult(true));
        mockDeviceService.Setup(x => x.UpdateStatus(It.IsAny<DeviceDescriptor>(), It.IsAny<DeviceStatus>()))
            .Returns(Task.FromResult(new ResponseDto<DeviceInfoDto>()));

        var list = new List<ScheduleTaskWithRequest>();
        var mockScheduleTaskManager = new Mock<IScheduleTaskManager>();
        mockScheduleTaskManager.Setup(x => x.NotStartedSchedules).Returns(list);
        mockScheduleTaskManager.Setup(x => x.TryGetLatestScheduleTaskOfLocation(It.IsAny<string>(), out It.Ref<ScheduleTaskWithRequest?>.IsAny))
            .Returns((string code, out ScheduleTaskWithRequest? request) =>
            {
                request = list.FirstOrDefault(x => x.LocationCode == code);
                return request != null;
            });

        var serviceCollection = ConstructRequiredServiceCollection();
        serviceCollection.AddSingleton<IDeviceProxyFactory, DeviceProxyFactory>();
        serviceCollection.AddSingleton<IDeviceService>(mockDeviceService.Object);
        serviceCollection.AddSingleton<IScheduleTaskManager>(mockScheduleTaskManager.Object);
        serviceCollection.AddSingleton<ILocationManager, LocationManager>();
        serviceCollection.AddSingleton<IDeviceManager, DeviceManager>();
        serviceCollection.AddSingleton<IDrillManager, DrillManager>();
        serviceCollection.AddSingleton<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>, PanelSiloForkManager>();
        serviceCollection.AddSingleton<IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask>, PanelSiloShelfManager>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var deviceFactory = serviceProvider.GetRequiredService<IDeviceProxyFactory>();
        var deviceManager = serviceProvider.GetRequiredService<IDeviceManager>();
        var drillManager = serviceProvider.GetRequiredService<IDrillManager>();
        var centralFlags = serviceProvider.GetRequiredService<CentralFlags>();

        var locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        await locationManager.Refresh();

        #region build Fork

        var panelFork = CreateDeviceProxy<PanelSiloFork>(deviceFactory, "PanelFork", "PanelFork", DeviceKind.PanelSiloFork);
        await deviceManager.AddOrUpdateDevice(panelFork);
        Assert.NotNull(panelFork);

        var forkPanelList = Panel.HasSilo.HasPanelForSpindleFirst("abc", 20 * 6, 20, ProductStatus.EmptySiloBox);
        forkPanelList.UpdatePanelInfo(1, forkPanelList.GetLayers(1), ProductStatus.Finished_PIN, "TO20240926000048");

        for (int i = 1; i <= 6; i++)
        {
            var forkSchedule = new ForkScheduleTask
            {
                Id = DateTime.Now.Ticks,
                RequestDeviceKind = DeviceKind.PanelSiloFork,
                CallerDeviceId = panelFork.DeviceId,
                RouteCode = "B001",
                Code = Guid.NewGuid().ToString(),
                TaskId = Guid.NewGuid().ToString(),
                ScheduledTaskStatus = ScheduledTaskStatus.Created,
                CreateTime = DateTime.Now,
                ItemCode = "TO20240926000048",
                LocationCode = panelFork.DeviceId + i.ToString().PadLeft(3, '0'),
                InteractionSequence = InteractionSequence.UnloadOnly,
                EventRequest = new DeviceEventReportRequest
                {
                    RequestDeviceKind = DeviceKind.PanelSiloFork,
                    DeviceId = panelFork.DeviceId,
                    EventId = panelFork.DeviceId + $"#" + i,
                    RequestInteractionBehavior = InteractionBehavior.Make(DeviceKind.PanelSiloFork, InteractionBehavior.FRONT_UNLOAD_SILO_ONLY),
                    RequestMaterialKind = MaterialKind.PanelSilo,
                    RequestInteractionDirection = InteractionPosition.Rear,
                    PayloadPanels = forkPanelList.GetPanelList(i),
                },
            };

            list.Add(forkSchedule);
        }
        await panelFork.SetPanels(forkPanelList);

        #endregion build Fork

        #region build drill

        var drill = CreateDeviceProxy<CNC84Drill>(deviceFactory, "D5-2849-241", "CNC84Drill", DeviceKind.CNC84Drill);
        var drillPanelList = Panel.HasSilo.HasPanelForSpindleFirst("abc", 3 * 2, 2, ProductStatus.Finished_DRILL);
        Assert.NotNull(drill);

        var schedule = new DrillScheduleTask
        {
            Id = DateTime.Now.Ticks,
            RequestDeviceKind = DeviceKind.CNC84Drill,
            CallerDeviceId = drill.DeviceId,
            RouteCode = "B001",
            Code = Guid.NewGuid().ToString(),
            TaskId = Guid.NewGuid().ToString(),
            ScheduledTaskStatus = ScheduledTaskStatus.Created,
            CreateTime = DateTime.Now,
            ItemCode = "TO20240926000048",
            LocationCode = drill.DeviceId,
            InteractionSequence = InteractionSequence.LoadThenUnload,
            EventRequest = new DeviceEventReportRequest
            {
                RequestDeviceKind = DeviceKind.CNC84Drill,
                DeviceId = drill.DeviceId,
                EventId = drill.DeviceId + $"#1",
                RequestInteractionBehavior = InteractionBehavior.Make(DeviceKind.CNC84Drill, InteractionBehavior.REAR_LOAD_PANEL_ONLY),
                RequestMaterialKind = MaterialKind.Panel,
                RequestInteractionDirection = InteractionPosition.Rear,
                PayloadPanels = drillPanelList,
            },
        };
        list.Add(schedule);
        await drill.SetPanels(drillPanelList);

        #endregion build drill

        #region build Agv

        var Agv01 = CreateDeviceProxy<TransferSiloAgv>(deviceFactory, "Agv01", "Agv", DeviceKind.ShelfSiloAgv);
        Agv01.Status = DeviceStatus.Ready;
        await deviceManager.AddOrUpdateDevice(Agv01);
        Assert.NotNull(Agv01);

        var agvPanelList = Panel.HasSilo.HasPanelForSpindleFirst("abc", 20 * 1, 20, ProductStatus.EmptyPayload);
        await Agv01.SetPanels(agvPanelList);

        #endregion build Agv

        var panelSiloForkManager = (PanelSiloForkManager)serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>>();
        var panelSiloShelfManager = (PanelSiloShelfManager)serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask>>();

        var found = panelSiloForkManager.TryFindLocation((ScheduleRequirement)schedule.Requirement, out var outboundingLocation);
        Assert.True(found);
        Assert.NotNull(outboundingLocation);
        Assert.Equal(1, outboundingLocation.Position);

        var locationOfAGV01 = locationManager.GetLocation("Agv01");
        Assert.NotNull(locationOfAGV01);
        Assert.True(locationOfAGV01.IsEmptyPayload);
    }
}
