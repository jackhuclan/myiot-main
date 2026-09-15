using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Schedule;
using VgAutoDrill.Central.Core.Schedule.Deliver;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace UnitTest.VgAutoDrill.Central.Manager;

public class TryFindEmptyPayloadLocationTest : CentralTestBase
{
    [Fact]
    public async Task Test()
    {
        #region mock IScheduleTaskDeliverPolicy

        var mockScheduleTaskDeliverPolicy = new Mock<IScheduleTaskDeliverPolicy>();
        mockScheduleTaskDeliverPolicy.Setup(x => x.DeliverScheduleTask(It.IsAny<DeliverScheduleTaskRequirement>()))
            .Returns(() =>
            {
                return Task.FromResult(new AgvAllocationResult(AgvAllocationResultCode.Success, AgvAllocationFailedReason.Completed));
            });

        var mockScheduleTaskDeliverPolicyFactory = new Mock<IScheduleTaskDeliverPolicyFactory>();
        mockScheduleTaskDeliverPolicyFactory.Setup(x => x.Create(It.IsAny<DeviceProxy>(), It.IsAny<MaterialKind>()))
            .Returns(mockScheduleTaskDeliverPolicy.Object);

        #endregion mock IScheduleTaskDeliverPolicy

        #region mock ISysConfigManager

        var mockSysConfigManager = new Mock<ISysConfigManager>();
        mockSysConfigManager.Setup(x => x.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN,
            SysConfigCategoryEnum.None,
            false))
            .Returns(Task.FromResult(false));
        mockSysConfigManager.Setup(x => x.GetIntValue("CountOfEmptyForkAtLeast", SysConfigCategoryEnum.None, false))
            .Returns(Task.FromResult(1));

        #endregion mock ISysConfigManager

        #region mock IPartitionManager

        var partitionList = new List<MockPartition> {
                new MockPartition{ PartCode="P1", RouteCodes=new() {"a","b","c" } },
                new MockPartition{ PartCode="P2", RouteCodes=new() {"d","e","f" } },
            };
        var mockPartitionManager = new Mock<IPartitionManager>();
        mockPartitionManager.SetupGet(x => x.Partitions)
            .Returns(partitionList);
        mockPartitionManager.Setup(x => x.TryGetPartition(It.IsAny<string>(), out It.Ref<Partition?>.IsAny))
            .Returns((string partitionCode, out Partition? partition) =>
            {
                partition = partitionList.FirstOrDefault(x => x.PartCode == partitionCode);
                return partition != null;
            });

        #endregion mock IPartitionManager

        #region mock IDeviceAdapter

        var mockIDeviceAdapter = new Mock<IDeviceAdapter>();
        mockIDeviceAdapter.Setup(x => x.GetDrillRouteCodes())
            .Returns(Task.FromResult(new List<DeviceRouteCodesPair>()
            {
                new DeviceRouteCodesPair{ DeviceId= "D5-2849-241", RouteCodes = new List<string> { "a","b","c" } },
            }));

        mockIDeviceAdapter.Setup(x => x.GetAgvRouteCodes())
            .Returns(Task.FromResult(new List<DeviceRouteCodesPair>()
            {
                //new DeviceRouteCodesPair{ DeviceId= "D5-2849-241", RouteCodes = new List<string> { "A","B","C" } },
            }));

        #endregion mock IDeviceAdapter

        #region mockDeviceService

        var mockDeviceService = new Mock<IDeviceService>();
        mockDeviceService.Setup(x => x.IsExistByDeviceId(It.IsAny<string>())).Returns(Task.FromResult(true));
        mockDeviceService.Setup(x => x.UpdateStatus(It.IsAny<DeviceDescriptor>(), It.IsAny<DeviceStatus>()))
            .Returns(Task.FromResult(new ResponseDto<DeviceInfoDto>()));

        #endregion mockDeviceService

        #region mockScheduleTaskManager

        var list = new List<ScheduleTaskWithRequest>();
        var forkList = new List<ForkScheduleTask>();
        var mockScheduleTaskManager = new Mock<IScheduleTaskManager>();
        mockScheduleTaskManager.Setup(x => x.NotStartedSchedules).Returns(list);
        mockScheduleTaskManager.Setup(x => x.NotStartedForkSchedules).Returns(forkList);
        mockScheduleTaskManager.Setup(x => x.TryGetLatestScheduleTaskOfLocation(It.IsAny<string>(), out It.Ref<ScheduleTaskWithRequest?>.IsAny))
                .Returns((string code, out ScheduleTaskWithRequest? request) =>
                {
                    request = list.FirstOrDefault(x => x.LocationCode == code);
                    return request != null;
                });

        #endregion mockScheduleTaskManager

        #region mock ITransferJobAdapter

        var transferJobs = new List<TransferJob>();
        var mockTransferJobAdapter = new Mock<ITransferJobAdapter>();
        mockTransferJobAdapter.Setup(x => x.Create(It.IsAny<TransferJob>()))
            .Returns((TransferJob job) =>
            {
                transferJobs.Add(job);
                return Task.FromResult("");
            });
        mockTransferJobAdapter.Setup(x => x.GetTodoTransferRacks())
            .Returns(Task.FromResult(transferJobs));

        #endregion mock ITransferJobAdapter

        #region mock ILocationAdapter

        var locations = new List<Location>() {
            new Location{ Code = "PanelFork001", Status = 1, PartitionCode = "P1"},
            new Location{ Code = "PanelFork002", Status = 1, PartitionCode = "P1"},
            new Location{ Code = "PanelFork003", Status = 1, PartitionCode = "P1"},
            new Location{ Code = "PanelFork004", Status = 1, PartitionCode = "P2"},
            new Location{ Code = "PanelFork005", Status = 1, PartitionCode = "P2"},
            new Location{ Code = "PanelFork006", Status = 1, PartitionCode = "P2"},
            new Location{ Code = "D5-2849-241", Status = 1},
        };
        var mockLocationAdapter = new Mock<ILocationAdapter>();
        mockLocationAdapter.Setup(x => x.GetLocations())
            .Returns(Task.FromResult(locations));

        #endregion mock ILocationAdapter

        #region build serviceCollection

        var serviceCollection = ConstructRequiredServiceCollection();
        serviceCollection.AddSingleton<ILocationAdapter>(mockLocationAdapter.Object);
        serviceCollection.AddSingleton<IDeviceService>(mockDeviceService.Object);
        serviceCollection.AddSingleton<IScheduleTaskManager>(mockScheduleTaskManager.Object);
        serviceCollection.AddSingleton<IScheduleTaskManager>(mockScheduleTaskManager.Object);
        serviceCollection.AddSingleton<ISysConfigManager>(mockSysConfigManager.Object);
        serviceCollection.AddSingleton<IPartitionManager>(mockPartitionManager.Object);
        serviceCollection.AddSingleton<ITransferJobAdapter>(mockTransferJobAdapter.Object);
        serviceCollection.AddSingleton<IScheduleTaskDeliverPolicyFactory>(mockScheduleTaskDeliverPolicyFactory.Object);
        serviceCollection.AddSingleton<IDeviceAdapter>(mockIDeviceAdapter.Object);
        serviceCollection.AddSingleton<IDeviceProxyFactory, DeviceProxyFactory>();
        serviceCollection.AddSingleton<ILocationManager, LocationManager>();
        serviceCollection.AddSingleton<IDeviceManager, DeviceManager>();
        serviceCollection.AddSingleton<IDrillManager, DrillManager>();
        serviceCollection.AddSingleton<ITransferPlanManager, TransferPlanManager>();
        serviceCollection.AddSingleton<IPanelAgvManager<TransferSiloAgv>, TransferSiloAgvManager>();
        serviceCollection.AddSingleton<IPanelAgvManager<BackPanelAgv>, BackPanelAgvManager>();
        serviceCollection.AddSingleton<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>, PanelSiloForkManager>();
        serviceCollection.AddSingleton<IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask>, PanelSiloShelfManager>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var deviceFactory = serviceProvider.GetRequiredService<IDeviceProxyFactory>();
        var deviceManager = serviceProvider.GetRequiredService<IDeviceManager>();
        var drillManager = serviceProvider.GetRequiredService<IDrillManager>();
        var transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        var centralFlags = serviceProvider.GetRequiredService<CentralFlags>();
        var panelSiloForkManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>>();

        var locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        await locationManager.Refresh();

        #endregion build serviceCollection

        #region build Fork

        var panelFork = CreateDeviceProxy<PanelSiloFork>(deviceFactory, "PanelFork", "PanelFork", DeviceKind.PanelSiloFork);
        await deviceManager.AddOrUpdateDevice(panelFork);
        Assert.NotNull(panelFork);

        var forkPanelList = Panel.HasSilo.HasPanelForSpindleFirst("abc", 18 * 6, 18, ProductStatus.EmptyPayload);
        //forkPanelList.UpdatePanelInfo(3, forkPanelList.GetLayers(3), ProductStatus.Finished_PIN, "TO20240926000048");

        for (int i = 1; i <= 6; i++)
        {
            var forkSchedule = new ForkScheduleTask
            {
                Id = DateTime.Now.Ticks,
                RequestDeviceKind = DeviceKind.PanelSiloFork,
                CallerDeviceId = panelFork.DeviceId,
                RouteCode = i <= 3 ? "a" : "c",
                Code = Guid.NewGuid().ToString(),
                TaskId = Guid.NewGuid().ToString(),
                ScheduledTaskStatus = ScheduledTaskStatus.Created,
                CreateTime = DateTime.Now,
                ItemCode = "",
                LocationCode = panelFork.DeviceId + i.ToString().PadLeft(3, '0'),
                InteractionSequence = i == 3 ? InteractionSequence.UnloadOnly : InteractionSequence.LoadOnly,
                EventRequest = new DeviceEventReportRequest
                {
                    RequestDeviceKind = DeviceKind.PanelSiloFork,
                    DeviceId = panelFork.DeviceId,
                    EventId = panelFork.DeviceId + $"#" + i,
                    RequestInteractionBehavior = InteractionBehavior.Make(DeviceKind.PanelSiloFork, i == 3 ? InteractionBehavior.REAR_UNLOAD_SILO_ONLY : InteractionBehavior.REAR_LOAD_SILO_ONLY),
                    RequestMaterialKind = MaterialKind.PanelSilo,
                    RequestInteractionDirection = InteractionPosition.Rear,
                    PayloadPanels = forkPanelList.GetPanelList(i),
                },
            };
            forkSchedule.RequestJson = JsonSerializer.Serialize(forkSchedule.EventRequest);
            list.Add(forkSchedule);
            forkList.Add(forkSchedule);
        }

        await panelFork.SetPanels(forkPanelList);

        #endregion build Fork

        #region build drill241

        var drill241 = CreateDeviceProxy<CNC84Drill>(deviceFactory, "D5-2849-241", "CNC84Drill", DeviceKind.CNC84Drill);
        var drill241PanelList = Panel.HasSilo.HasPanelForSpindleFirst("abc", 3 * 2, 2, ProductStatus.Finished_DRILL);
        drill241PanelList.UpdatePanelInfo(1, new int[] { 1 }, ProductStatus.Finished_DRILL, "TO20240926000048");
        drill241PanelList.UpdatePanelInfo(2, new int[] { 1 }, ProductStatus.Finished_DRILL, "TO20240926000048");
        drill241PanelList.UpdatePanelInfo(3, new int[] { 1 }, ProductStatus.Finished_DRILL, "TO20240926000048");
        await deviceManager.AddOrUpdateDevice(drill241);
        Assert.NotNull(drill241);

        var drillSchedule241 = new DrillScheduleTask
        {
            Id = DateTime.Now.Ticks,
            RequestDeviceKind = DeviceKind.CNC84Drill,
            CallerDeviceId = drill241.DeviceId,
            RouteCode = "A",
            Code = Guid.NewGuid().ToString(),
            TaskId = Guid.NewGuid().ToString(),
            ScheduledTaskStatus = ScheduledTaskStatus.Created,
            CreateTime = DateTime.Now,
            ItemCode = "TO20240926000048",
            LocationCode = drill241.DeviceId,
            InteractionSequence = InteractionSequence.LoadThenUnload,
            EventRequest = new DeviceEventReportRequest
            {
                RequestDeviceKind = DeviceKind.CNC84Drill,
                DeviceId = drill241.DeviceId,
                EventId = drill241.DeviceId + $"#1",
                RequestInteractionBehavior = InteractionBehavior.Make(DeviceKind.CNC84Drill, InteractionBehavior.REAR_LOAD_PANEL_ONLY),
                RequestMaterialKind = MaterialKind.Panel,
                RequestInteractionDirection = InteractionPosition.Rear,
                PayloadPanels = drill241PanelList,
                Params = new Dictionary<string, object?>
                {
                    {"RawSpindleNum", 6 },
                    {"ClinkerSpindleNum", 6 }
                }
            },
        };
        drillSchedule241.RequestJson = JsonSerializer.Serialize(drillSchedule241.EventRequest);
        list.Add(drillSchedule241);
        await drill241.SetPanels(drill241PanelList);

        #endregion build drill241

        await deviceManager.RefreshAgvRouteCodes();
        await deviceManager.RefreshDrillRouteCodes();

        bool found = panelSiloForkManager.TryFindEmptyPayloadLocation(1, "P1", out var emptyPayloadLocation);
        Assert.True(found);
        Assert.NotNull(emptyPayloadLocation);
    }
}
