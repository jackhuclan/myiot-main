using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Central.Core.Mysql;
using VgAutoDrill.Central.Core.Schedule;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Infrastructure.Redis;

namespace UnitTest.VgAutoDrill.Central
{
    public class AgvScheduleStrategyTest
    {
        private ILoggerFactory loggerFactory;
        private ILoggerFactory GetLoggerFactory()
        {
            ServiceCollection services = new ServiceCollection();
            if (loggerFactory == null)
                loggerFactory = new LoggerFactory();
            services.AddSingleton(loggerFactory);

            return loggerFactory;
        }

        [Fact]
        public async void WhenAllDrilled_GetAvailableAgvDevice_LoadSiloOnly_ReturnAgv()
        {
            var deviceHolder = new Mock<IDeviceHolder>();
            var scheduleAdapter = Mock.Of<IScheduleTaskAdapter>();
            var sysConfigManager = Mock.Of<ISysConfigManager>();
            var deviceService = Mock.Of<IDeviceService>();
            var taskScheduleOptions = Mock.Of<IOptions<MysqlTaskSchedulerOptions>>();
            var scheduleTaskDeliverPolicyFactory = Mock.Of<IScheduleTaskDeliverPolicyFactory>();
            var loggerFactory = GetLoggerFactory();
            var deviceProxy = new Mock<DeviceProxy>();
            var redis = Mock.Of<IRedisClient>();

            deviceProxy.Setup(x => x.Status).Returns(DeviceStatus.Ready);
            var descriptor = new DeviceDescriptor
            {
                ProductId = Events.Products.AGV,
                DeviceId = "MockAgv01",
                DeviceKind = DeviceKind.BackPanelAgv,
                OutputCapabilities = new List<ProductStatus>
                            {
                                ProductStatus.PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_1
                            }
            };
            deviceProxy.Setup(x => x.Descriptor).Returns(descriptor);
            deviceProxy.Setup(x => x.PayloadPanels).Returns(new List<Panel> { new Panel { PanelCode = "1", ProductStatus = ProductStatus.PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_1 }, });
            deviceProxy.Setup(x => x.Properties).Returns(new Dictionary<string, object> {
                { "TargetDevice", "" },
                { "IsFullSilo", true },
            });

            deviceHolder
                .Setup(x => x.GetOnlineAgvs())
                .Returns(new List<DeviceProxy>
                {
                    deviceProxy.Object
                });

            var agvScheduleStrategy = new TaskScheduleStrategy(deviceHolder.Object,
                scheduleAdapter,
                deviceService,
                redis,
                sysConfigManager,
                taskScheduleOptions,
                scheduleTaskDeliverPolicyFactory,
                loggerFactory);

            var request = new DeviceEventReportRequest
            {
                RequestInteractionBehavior = InteractionBehavior.Make(DeviceKind.PanelSiloShelf, InteractionBehavior.REAR_LOAD_SILO_ONLY),
                RequestInputProductStatus = ProductStatus.PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_1,
            };

            var agv = agvScheduleStrategy.GetAvailableAgvDevices(request);
            Assert.NotEmpty(agv);
        }

        [Fact]
        public async void WhenNotAllDrilled_GetAvailableAgvDevice_LoadSiloOnly_ReturnEmptyList()
        {
            var deviceHolder = new Mock<IDeviceHolder>();
            var scheduleAdapter = Mock.Of<IScheduleTaskAdapter>();
            var deviceService = Mock.Of<IDeviceService>();
            var taskScheduleOptions = new MockOptions<MysqlTaskSchedulerOptions>().Mock(new MysqlTaskSchedulerOptions
            {
            });
            var itemCode = "item01";
            var scheduleTaskDeliverPolicyFactory = Mock.Of<IScheduleTaskDeliverPolicyFactory>();
            var loggerFactory = GetLoggerFactory();
            var redis = Mock.Of<IRedisClient>();
            var sysConfigManager = Mock.Of<ISysConfigManager>();
            var deviceProxy = new Mock<DeviceProxy>();
            deviceProxy.Setup(x => x.Status).Returns(DeviceStatus.Ready);
            var descriptor = new DeviceDescriptor
            {
                ProductId = Events.Products.AGV,
                DeviceId = "MockAgv01",
                DeviceKind = DeviceKind.BackPanelAgv,
                OutputCapabilities = new List<ProductStatus>
                            {
                                ProductStatus.PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_1
                            }
            };
            deviceProxy.Setup(x => x.Descriptor).Returns(descriptor);
            deviceProxy.Setup(x => x.PayloadPanels).Returns(
                new List<Panel>
                { 
                    //ÒÑ×ê¿×
                    new Panel { PanelCode = "1", ItemCode=itemCode, ProductStatus = ProductStatus.PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_1 },
                    //Î´×ê¿×
                    new Panel { PanelCode = "1", ItemCode=itemCode,ProductStatus = ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_1 },
                });
            deviceProxy.Setup(x => x.Properties).Returns(new Dictionary<string, object> { { "TargetDevice", "" } });
            deviceHolder
                .Setup(x => x.GetOnlineAgvs())
                .Returns(new List<DeviceProxy>
                {
                                deviceProxy.Object
                });

            var agvScheduleStrategy = new TaskScheduleStrategy(deviceHolder.Object,
                scheduleAdapter,
                deviceService,
                redis,
                sysConfigManager,
                taskScheduleOptions.Object,
                scheduleTaskDeliverPolicyFactory,
                loggerFactory);

            var request = new DeviceEventReportRequest
            {
                RequestInteractionBehavior = InteractionBehavior.Make(DeviceKind.PanelSiloShelf, InteractionBehavior.FRONT_LOAD_SILO_ONLY),
                RequestInputProductStatus = ProductStatus.PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_1,
            };
            request.Params[ScheduleConstants.PARAMS_TASK_ITEM_CODE] = itemCode;

            var agv = agvScheduleStrategy.GetAvailableAgvDevices(request);
            Assert.Empty(agv);
        }

        [Fact]
        public async void WhenNoneDrilled_GetAvailableAgvDevice_LoadSiloOnly_ReturnEmptyList()
        {
            var deviceHolder = new Mock<IDeviceHolder>();
            var redis = Mock.Of<IRedisClient>();
            var sysConfigManager = Mock.Of<ISysConfigManager>();
            var scheduleAdapter = Mock.Of<IScheduleTaskAdapter>();
            var deviceService = Mock.Of<IDeviceService>();
            var taskScheduleOptions = Mock.Of<IOptions<MysqlTaskSchedulerOptions>>();
            var scheduleTaskDeliverPolicyFactory = Mock.Of<IScheduleTaskDeliverPolicyFactory>();
            var loggerFactory = GetLoggerFactory();
            var deviceProxy = new Mock<DeviceProxy>();
            deviceProxy.Setup(x => x.Status).Returns(DeviceStatus.Ready);
            var descriptor = new DeviceDescriptor
            {
                ProductId = Events.Products.AGV,
                DeviceId = "MockAgv01",
                DeviceKind = DeviceKind.BackPanelAgv,
                OutputCapabilities = new List<ProductStatus>
                            {
                                ProductStatus.PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_1
                            }
            };
            deviceProxy.Setup(x => x.Descriptor).Returns(descriptor);
            deviceProxy.Setup(x => x.PayloadPanels).Returns(
                new List<Panel>
                {
                });
            deviceProxy.Setup(x => x.Properties).Returns(new Dictionary<string, object> { { "TargetDevice", "" } });

            deviceHolder
                .Setup(x => x.GetOnlineAgvs())
                .Returns(new List<DeviceProxy>
                {
                    deviceProxy.Object
                });

            var agvScheduleStrategy = new TaskScheduleStrategy(deviceHolder.Object,
                scheduleAdapter,
                deviceService,
                redis,
                sysConfigManager,
                taskScheduleOptions,
                scheduleTaskDeliverPolicyFactory,
                loggerFactory);

            var request = new DeviceEventReportRequest
            {
                RequestInteractionBehavior = InteractionBehavior.Make(DeviceKind.PanelSiloShelf, InteractionBehavior.REAR_LOAD_SILO_ONLY),
                RequestInputProductStatus = ProductStatus.PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_1,
            };

            var agv = agvScheduleStrategy.GetAvailableAgvDevices(request);
            Assert.Empty(agv);
        }

        [Fact]
        public async void WhenHaveNewCutter_GetAvailableAgvDevice_LoadCutterSiloOnly_ReturnAgv()
        {
            var deviceHolder = new Mock<IDeviceHolder>();
            var scheduleAdapter = Mock.Of<IScheduleTaskAdapter>();
            var sysConfigManager = Mock.Of<ISysConfigManager>();
            var deviceService = Mock.Of<IDeviceService>();
            var taskScheduleOptions = Mock.Of<IOptions<MysqlTaskSchedulerOptions>>();
            var scheduleTaskDeliverPolicyFactory = Mock.Of<IScheduleTaskDeliverPolicyFactory>();
            var loggerFactory = GetLoggerFactory();
            var deviceProxy = new Mock<DeviceProxy>();
            var redis = Mock.Of<IRedisClient>();

            deviceProxy.Setup(x => x.Status).Returns(DeviceStatus.Ready);
            var descriptor = new DeviceDescriptor
            {
                ProductId = Events.Products.AGV,
                DeviceId = "MockAgv01",
                DeviceKind = DeviceKind.FrontToolAgv
            };
            deviceProxy.Setup(x => x.Descriptor).Returns(descriptor);
            var trays = CutterTray.InitializeCutterSilo(6, 3, 2);
            trays[0].Status = CutterTrayStatus.New;
            trays[1].Status = CutterTrayStatus.New;
            trays[2].Status = CutterTrayStatus.New;
            trays[3].Status = CutterTrayStatus.New;
            trays[4].Status = CutterTrayStatus.New;
            trays[5].Status = CutterTrayStatus.New;
            deviceProxy.Setup(x => x.PayloadCutterTrays).Returns(trays);
            deviceProxy.Setup(x => x.Properties).Returns(new Dictionary<string, object> {
                { "TargetDevice", "" },
            });

            deviceHolder
                .Setup(x => x.GetOnlineAgvs())
                .Returns(new List<DeviceProxy>
                {
                    deviceProxy.Object
                });

            var agvScheduleStrategy = new TaskScheduleStrategy(deviceHolder.Object,
                scheduleAdapter,
                deviceService,
                redis,
                sysConfigManager,
                taskScheduleOptions,
                scheduleTaskDeliverPolicyFactory,
                loggerFactory);

            var request = new DeviceEventReportRequest
            {
                RequestInteractionBehavior = InteractionBehavior.Make(DeviceKind.CNC84Drill, InteractionBehavior.FRONT_LOAD_CUTTER_THEN_UNLOAD_CUTTER),
            };

            var agv = agvScheduleStrategy.GetAvailableAgvDevices(request);
            Assert.NotEmpty(agv);
        }
        [Fact]
        public async void WhenNoNewCutter_GetAvailableAgvDevice_LoadCutterSiloOnly_ReturnEmptyList()
        {
            var deviceHolder = new Mock<IDeviceHolder>();
            var scheduleAdapter = Mock.Of<IScheduleTaskAdapter>();
            var sysConfigManager = Mock.Of<ISysConfigManager>();
            var deviceService = Mock.Of<IDeviceService>();
            var taskScheduleOptions = Mock.Of<IOptions<MysqlTaskSchedulerOptions>>();
            var scheduleTaskDeliverPolicyFactory = Mock.Of<IScheduleTaskDeliverPolicyFactory>();
            var loggerFactory = GetLoggerFactory();
            var deviceProxy = new Mock<DeviceProxy>();
            var redis = Mock.Of<IRedisClient>();

            deviceProxy.Setup(x => x.Status).Returns(DeviceStatus.Ready);
            var descriptor = new DeviceDescriptor
            {
                ProductId = Events.Products.AGV,
                DeviceId = "MockAgv01",
                DeviceKind = DeviceKind.FrontToolAgv
            };
            deviceProxy.Setup(x => x.Descriptor).Returns(descriptor);
            var trays = CutterTray.InitializeCutterSilo(6, 3, 2);
            trays[0].Status = CutterTrayStatus.Old;
            trays[1].Status = CutterTrayStatus.Old;
            trays[2].Status = CutterTrayStatus.Old;
            trays[3].Status = CutterTrayStatus.Old;
            trays[4].Status = CutterTrayStatus.Old;
            trays[5].Status = CutterTrayStatus.Old;
            deviceProxy.Setup(x => x.PayloadCutterTrays).Returns(trays);
            deviceProxy.Setup(x => x.Properties).Returns(new Dictionary<string, object> {
                { "TargetDevice", "" },
            });

            deviceHolder
                .Setup(x => x.GetOnlineAgvs())
                .Returns(new List<DeviceProxy>
                {
                    deviceProxy.Object
                });

            var agvScheduleStrategy = new TaskScheduleStrategy(deviceHolder.Object,
                scheduleAdapter,
                deviceService,
                redis,
                sysConfigManager,
                taskScheduleOptions,
                scheduleTaskDeliverPolicyFactory,
                loggerFactory);

            var request = new DeviceEventReportRequest
            {
                RequestInteractionBehavior = InteractionBehavior.Make(DeviceKind.CNC84Drill, InteractionBehavior.FRONT_LOAD_CUTTER_THEN_UNLOAD_CUTTER),
            };

            var agv = agvScheduleStrategy.GetAvailableAgvDevices(request);
            Assert.Empty(agv);
        }
    }
}
