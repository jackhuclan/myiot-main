using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Channel;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.State;

namespace Vegalot.External.LeweiPin
{
    public class PinStateHandler : AbstractStateHandler<Pin>
    {
        private readonly IMessageChannel dataExporter;
        private readonly ILogger<PinStateHandler> logger;

        public PinStateHandler(ILogger<PinStateHandler> logger,
            IServiceProvider serviceProvider,
            Pin siloShelf
            )
            : base(serviceProvider, siloShelf)
        {
            this.logger = logger;
        }

        public override void AddWatchingStates()
        {
            //WatchingProperties.Properties("IsReady")
            //      .When(properties => properties.Property("IsReady").IsValueChanged)
            //      .TriggerAlways(async () =>
            //      {
            //          var isReady = WatchingProperties.Property("IsReady").NewValue.ToBool();

            //          var request = GetStatusRequest(InteractingDevice.Status);
            //          InteractingDevice.Status = isReady ? DeviceStatus.Ready : DeviceStatus.Online;
            //          logger.LogInformation($"上报状态 ready:{isReady}  online:{!isReady} ");
            //          await DataExporter.DeviceStatusReport(request);
            //      });
        }

        private DeviceStatusReportRequest GetStatusRequest(DeviceStatus newStatus)
        {
            var request = new DeviceStatusReportRequest
            {
                DeviceId = DeviceDescriptor.DeviceId,
                ProductId = DeviceDescriptor.ProductId,
                NewStatus = newStatus,
                OldStatus = InteractingDevice.Status
            };
            request.PayloadPanels = PayloadPanels;
            return request;
        }
    }
}
