using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Event;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Devices.Fork;

public class PanelForkEventHandler : AbstractEventHandler<PanelFork>
{
    private readonly ILogger<PanelForkEventHandler> _logger;

    public PanelForkEventHandler(ILogger<PanelForkEventHandler> logger,
        IServiceProvider serviceProvider,
        PanelFork panelFork)
        : base(serviceProvider, panelFork)
    {
        _logger = logger;
    }

    public override void AddWatchingEvents()
    {
        foreach (var location in InteractingDevice.Locations.Values)
        {
            WatchingProperties.Properties("MqttConnected",
                    $"TranscationId{location.Position}",
                    $"PanelsCount{location.Position}",
                    $"IsReady{location.Position}")
                .When(proteryties =>
                {
                    return proteryties.Property("MqttConnected").NewValue.ToBool()
                    && string.IsNullOrWhiteSpace(proteryties.Property($"TranscationId{location.Position}").NewValue.ToStr())
                    && proteryties.Property($"PanelsCount{location.Position}").NewValue.ToInt() == location.LayerLimit
                    && proteryties.Property($"IsReady{location.Position}").NewValue.ToBool();
                })
                .TriggerAlways(async () =>
                {
                    try
                    {
                        var result = await location.StartNewSchedule();
                        location.TransactionMessage = result?.Message!;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"{location.Position} 号料架 呼叫AGV异常 ： {ex.Message} ");
                        location.TransactionMessage = $"呼叫AGV异常 ： {ex.Message}";
                    }
                });
        }
    }
}
