using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Channel;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.State;

namespace VgDeviceGateway.Devices.Shelf;

public class SiloShelfStateHandler : AbstractStateHandler<SiloShelf>
{
    private readonly IMessageChannel dataExporter;
    private readonly ILogger<SiloShelfStateHandler> logger;

    public SiloShelfStateHandler(ILogger<SiloShelfStateHandler> logger,
        IServiceProvider serviceProvider,
        SiloShelf siloShelf
        )
        : base(serviceProvider, siloShelf)
    {
        this.logger = logger;
    }

    public override void AddWatchingStates()
    {
        foreach (var shelf in InteractingDevice.Locations)
        {
            WatchingProperties.Properties($"IsReady{shelf.Key}")
                .When(properties => properties.Property($"IsReady{shelf.Key}").IsValueChanged)
                .TriggerAlways(async () =>
                {
                });
        }
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
