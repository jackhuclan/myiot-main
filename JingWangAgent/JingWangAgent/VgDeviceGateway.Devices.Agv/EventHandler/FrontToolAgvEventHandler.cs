using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;
using static VgAutoDrill.Fundation.Iot.Events;

namespace VgDeviceGateway.Devices.Agv.EventHandler
{
    public class FrontToolAgvEventHandler : DeviceShare<DefaultAgv>, IAgvEventHandler
    {
        private readonly ILogger<FrontToolAgvEventHandler> logger;

        public FrontToolAgvEventHandler(ILogger<FrontToolAgvEventHandler> logger, IServiceProvider serviceProvider, DefaultAgv device) : base(serviceProvider, device)
        {
            this.logger = logger;
        }

        public void AddWatchingEventsLocal()
        {
            WatchingProperties.Property("IsAuto")
            .When(properties => properties.NewValue.ToBool() && properties.IsValueChanged)
            .TriggerAlways(async () =>
            {
                await DataExporter.DeviceEventReport(
                      new DeviceEventReportRequest()
                      {
                          EventId = AGV.AGV_UPDATE_PLCSILOINFO_EVENT,
                          EventName = AGV.AGV_UPDATE_PLCSILOINFO_NAME,
                          DeviceId = InteractingDevice.DeviceId,
                          ProductId = InteractingDevice.ProductId,
                          ClientId = InteractingDevice.ClientId,
                          PayloadPanels = InteractingDevice.PayloadPanels
                      });
            });

            WatchingProperties.Property("WarningCode")
                .When(properties => properties.NewValue.ToInt() > 0 && properties.IsValueChanged)
                .TriggerAlways(async () =>
                {
                    await DataExporter.DeviceEventReport(
                         new DeviceEventReportRequest()
                         {
                             EventId = AGV.AGV_UPDATE_PLCSILOINFO_EVENT,
                             EventName = AGV.AGV_UPDATE_PLCSILOINFO_NAME,
                             DeviceId = InteractingDevice.DeviceId,
                             ProductId = InteractingDevice.ProductId,
                             ClientId = InteractingDevice.ClientId,
                             PayloadPanels = InteractingDevice.PayloadPanels,
                             Params = new Dictionary<string, object?> {
                            {
                                     "WarningCode", WatchingProperties.Property("WarningCode").NewValue.ToFloat()}
                            }
                         });
                });

            WatchingProperties.Property("AgvChassisErrorMessage")
                .When(properties => properties.NewValue.ToStr() != "" && properties.IsValueChanged)
                .TriggerAlways(async () =>
                {
                    var errormsg = WatchingProperties.Property("AgvChassisErrorMessage").NewValue.ToStr();
                    logger.LogDebug(errormsg);
                    InteractingDevice.ReportingProcess(errormsg);
                    WatchingProperties.Property("AgvChassisErrorMessage").SetValue("");
                });

            AgvStatusReportEvent();
            LoadMaterialOkReportEvent();
            UnLoadMaterialOkReportEvent();
        }

        private void AgvStatusReportEvent()
        {
            WatchingProperties.Property("IsArrived")
              .When(properties => properties.NewValue.ToBool() && properties.IsValueChanged)
              .TriggerAlways(async () =>
              {
                  await DataExporter.DeviceEventReport(
                        new DeviceEventReportRequest()
                        {
                            EventId = AGV.AGV_MOVE_ISARRIVED_EVENT,
                            EventName = AGV.AGV_MOVE_ISARRIVED_NAME,
                            DeviceId = InteractingDevice.DeviceId,
                            ProductId = InteractingDevice.ProductId,
                            ClientId = InteractingDevice.ClientId,
                            PayloadPanels = InteractingDevice.PayloadPanels
                        });
              });

            WatchingProperties.Property("IsMoving")
              .When(properties => properties.NewValue.ToBool() && properties.IsValueChanged)
              .TriggerAlways(async () =>
              {
                  await DataExporter.DeviceEventReport(
                        new DeviceEventReportRequest()
                        {
                            EventId = AGV.AGV_MOVE_ISMOVING_EVENT,
                            EventName = AGV.AGV_MOVE_ISMOVING_NAME,
                            DeviceId = InteractingDevice.DeviceId,
                            ProductId = InteractingDevice.ProductId,
                            ClientId = InteractingDevice.ClientId,
                            PayloadPanels = InteractingDevice.PayloadPanels
                        });
              });

            WatchingProperties.Property("AgvStatus")
              .When(properties => properties.NewValue.ToStr() == StdAgvStatus.CHARGING && properties.IsValueChanged)
              .TriggerAlways(async () =>
              {
                  InteractingDevice.Status = DeviceStatus.Charging;
                  await DataExporter.DeviceEventReport(
                        new DeviceEventReportRequest()
                        {
                            EventId = AGV.AGV_CHARGE_EVENT,
                            EventName = AGV.AGV_CHARGE_NAME,
                            DeviceId = InteractingDevice.DeviceId,
                            ProductId = InteractingDevice.ProductId,
                            ClientId = InteractingDevice.ClientId,
                            PayloadPanels = InteractingDevice.PayloadPanels
                        });
              });
        }

        private void UnLoadMaterialOkReportEvent()
        {
            WatchingProperties.Property("PrepareUnloadOk")
              .When(properties => properties.NewValue.ToBool() && properties.IsValueChanged)
              .TriggerAlways(async () =>
              {
                  await DataExporter.DeviceEventReport(
                        new DeviceEventReportRequest()
                        {
                            EventId = AGV.AGV_PREPAREUNLOADOK_EVENT,
                            EventName = AGV.AGV_PREPAREUNLOADOK_NAME,
                            DeviceId = InteractingDevice.DeviceId,
                            ProductId = InteractingDevice.ProductId,
                            ClientId = InteractingDevice.ClientId,
                            PayloadPanels = InteractingDevice.PayloadPanels
                        });
              });

            WatchingProperties.Property("InvokeUnloadOk")
              .When(properties => properties.NewValue.ToBool() && properties.IsValueChanged)
              .TriggerAlways(async () =>
              {
                  await DataExporter.DeviceEventReport(
                        new DeviceEventReportRequest()
                        {
                            EventId = AGV.AGV_INVOKEUNLOADOK_EVENT,
                            EventName = AGV.AGV_INVOKEUNLOADOK_NAME,
                            DeviceId = InteractingDevice.DeviceId,
                            ProductId = InteractingDevice.ProductId,
                            ClientId = InteractingDevice.ClientId,
                            PayloadPanels = InteractingDevice.PayloadPanels
                        });
              });

            WatchingProperties.Property("CompleteUnloadOk")
              .When(properties => properties.NewValue.ToBool() && properties.IsValueChanged)
              .TriggerAlways(async () =>
              {
                  await DataExporter.DeviceEventReport(
                        new DeviceEventReportRequest()
                        {
                            EventId = AGV.AGV_COMPLETEUNLOADOK_EVENT,
                            EventName = AGV.AGV_COMPLETEUNLOADOK_NAME,
                            DeviceId = InteractingDevice.DeviceId,
                            ProductId = InteractingDevice.ProductId,
                            ClientId = InteractingDevice.ClientId,
                            PayloadPanels = InteractingDevice.PayloadPanels
                        });
              });
        }

        private void LoadMaterialOkReportEvent()
        {
            WatchingProperties.Property("PrepareLoadOk")
              .When(properties => properties.NewValue.ToBool() && properties.IsValueChanged)
              .TriggerAlways(async () =>
              {
                  await DataExporter.DeviceEventReport(
                         new DeviceEventReportRequest()
                         {
                             EventId = AGV.AGV_PREPARELOADOK_EVENT,
                             EventName = AGV.AGV_PREPARELOADOK_NAME,
                             DeviceId = InteractingDevice.DeviceId,
                             ProductId = InteractingDevice.ProductId,
                             ClientId = InteractingDevice.ClientId,
                             PayloadPanels = InteractingDevice.PayloadPanels
                         });
              });

            WatchingProperties.Property("InvokeLoadOk")
              .When(properties => properties.NewValue.ToBool() && properties.IsValueChanged)
              .TriggerAlways(async () =>
              {
                  await DataExporter.DeviceEventReport(
                         new DeviceEventReportRequest()
                         {
                             EventId = AGV.AGV_INVOKELOADOK_EVENT,
                             EventName = AGV.AGV_INVOKELOADOK_NAME,
                             DeviceId = InteractingDevice.DeviceId,
                             ProductId = InteractingDevice.ProductId,
                             ClientId = InteractingDevice.ClientId,
                             PayloadPanels = InteractingDevice.PayloadPanels
                         });
              });

            WatchingProperties.Property("CompleteLoadOk")
              .When(properties => properties.NewValue.ToBool() && properties.IsValueChanged)
              .TriggerAlways(async () =>
              {
                  await DataExporter.DeviceEventReport(
                        new DeviceEventReportRequest()
                        {
                            EventId = AGV.AGV_COMPLETELOADOK_EVENT,
                            EventName = AGV.AGV_COMPLETELOADOK_NAME,
                            DeviceId = InteractingDevice.DeviceId,
                            ProductId = InteractingDevice.ProductId,
                            ClientId = InteractingDevice.ClientId,
                            PayloadPanels = InteractingDevice.PayloadPanels
                        });
              });
        }
    }
}
