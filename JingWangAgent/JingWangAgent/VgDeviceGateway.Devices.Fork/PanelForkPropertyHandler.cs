using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Property;

namespace VgDeviceGateway.Devices.Fork;

public class PanelForkPropertyHandler : AbstractPropertyHandler<PanelFork>
{
    public PanelForkPropertyHandler(ILogger<PanelForkPropertyHandler> logger,
        IServiceProvider serviceProvider,
        PanelFork panelFork)
        : base(serviceProvider, panelFork)
    {
    }

    public override void AddWatchingProperties()
    {
        var watchProperty = WatchingProperties
             .AddProperty("MqttConnected", false)
             .AddProperty("TranscationId", "");

        foreach (var location in InteractingDevice.Locations.Values)
        {
            watchProperty.AddProperty($"TranscationId{location.Position}", string.Empty);
            watchProperty.AddProperty($"TransactionMessage{location.Position}", string.Empty);
            watchProperty.AddProperty($"PanelsCount{location.Position}", 0);
            watchProperty.AddProperty($"IsReady{location.Position}", false);
        }
    }

    public override void CollectPropertyValues()
    {
        var realProperties = new Dictionary<string, object?>();
        realProperties["MqttConnected"] = InteractingDevice.MqttClientWrapper.IsConnected;

        foreach (var location in InteractingDevice.Locations.Values)
        {
            realProperties[$"TranscationId{location.Position}"] = location.TranscationId;
            realProperties[$"TransactionMessage{location.Position}"] = location.TransactionMessage;
            realProperties[$"PanelsCount{location.Position}"] = location.Panels.Count();
            realProperties[$"IsReady{location.Position}"] = location.IsReady;
        }

        WatchingProperties.SetValues(realProperties);
    }
}
