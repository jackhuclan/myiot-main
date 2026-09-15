using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.State;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgDeviceGateway.Devices.Drill.StateHandler;

namespace VgDeviceGateway.Devices.Drill
{
    public class DefaultDrillStateHandler : AbstractStateHandler<DefaultDrill>
    {
        private readonly int agvOperationTypes;
        private readonly IObjectFactory factory;
        private IDrillStateHandler abstartDrillStateHandler;

        public DefaultDrillStateHandler(IServiceProvider serviceProvider,
            IObjectFactory factory,
            DefaultDrill device)
            : base(serviceProvider, device)
        {
            agvOperationTypes = device.DeviceDescriptor.Extra["AgvOperationTypes"].ToInt();
            this.factory = factory;
            abstartDrillStateHandler = InteractionFactory.CreatetDrillStateHandler(factory, InteractingDevice, agvOperationTypes);
        }

        public override void AddWatchingStates()
        {
            if (abstartDrillStateHandler == null)
            {
                return;
            }
            WatchingProperties.Properties(abstartDrillStateHandler.ReadyProperties.ToArray())
                .When(properties =>
                {
                    return abstartDrillStateHandler.SetReadyStateCondition(properties);
                })
               .TriggerAlways(async () =>
               {
                   var request = MakeStatusRequest(DeviceStatus.Ready);
                   InteractingDevice.Status = DeviceStatus.Ready;
                   if (DeviceDescriptor.AutoMode)
                   {
                       await DataExporter.DeviceStatusReport(request);
                   }
               });

            WatchingProperties.Properties(abstartDrillStateHandler.WorkingProperties.ToArray())
                .When(properties =>
                {
                    return abstartDrillStateHandler.SetWorkingStateCondition(properties);
                })
                .TriggerAlways(async () =>
                {
                    var request = MakeStatusRequest(DeviceStatus.Working);
                    InteractingDevice.Status = DeviceStatus.Working;
                    if (DeviceDescriptor.AutoMode)
                    {
                        await DataExporter.DeviceStatusReport(request);
                    }
                });

            WatchingProperties.Properties(abstartDrillStateHandler.ExceptionProperties.ToArray())
                .When(properties =>
                {
                    return abstartDrillStateHandler.SetExceptionStateCondition(properties);
                })
                .TriggerAlways(async () =>
                {
                    var request = MakeStatusRequest(DeviceStatus.Exception);
                    InteractingDevice.Status = DeviceStatus.Exception;
                    if (DeviceDescriptor.AutoMode)
                    {
                        await DataExporter.DeviceStatusReport(request);
                    }
                });
        }

        private DeviceStatusReportRequest MakeStatusRequest(DeviceStatus newStatus)
        {
            var request = new DeviceStatusReportRequest
            {
                DeviceId = DeviceDescriptor.DeviceId,
                ProductId = DeviceDescriptor.ProductId,
                NewStatus = newStatus,
                OldStatus = InteractingDevice.Status,
                Params = new Dictionary<string, object?>
                {
                    { "CallAgvMessage",InteractingDevice.AgvIsWorkOnBuffer?"agv在给buffer上下料":InteractingDevice.TransactionId },
                    { "IsLoadingOrUnLoading",InteractingDevice.AgvIsWorkOnBuffer},
                    { "IsWarning",InteractingDevice.AgvIsWorkOnBuffer?false:InteractingDevice.CallAgvMessageColor},
                    { "Percentage",InteractingDevice.percentage},
                    { "ExistRawPanel",InteractingDevice.existRawPanel},
                    { "SuggestPanelInteractionSequence",InteractingDevice.suggestInteractionSequence},
                    { "IsAvailbleForAgv",InteractingDevice.isAvailbleForAgv}
                }
            };
            request.PayloadPanels = PayloadPanels;

            return request;
        }
    }
}
