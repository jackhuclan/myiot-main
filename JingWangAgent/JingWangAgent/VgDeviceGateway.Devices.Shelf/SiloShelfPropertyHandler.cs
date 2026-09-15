using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Property;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Devices.Shelf;

public class SiloShelfPropertyHandler : AbstractPropertyHandler<SiloShelf>
{
    public SiloShelfPropertyHandler(ILogger<SiloShelfPropertyHandler> logger,
        IServiceProvider serviceProvider,
        SiloShelf siloShelf)
        : base(serviceProvider, siloShelf)
    {
        PeriodicTimers["20s"]!.OnTick += async () =>
        {
            var values = WatchingProperties.GetValues();
            await InteractingDevice.DataExporter.DevicePropertiesReport(
            new DevicePropertiesReportRequest()
            {
                DeviceId = InteractingDevice.DeviceDescriptor.DeviceId,
                ProductId = InteractingDevice.DeviceDescriptor.ProductId,
                Params = values,
            });
        };

        PeriodicTimers[InteractingDevice.DeviceDescriptor.Extra["SyncLocationInterval"].ToStr()]!.OnTick += () =>
        {
            if (InteractingDevice.IsCanLoopSync)
            {
                foreach (var location in InteractingDevice.Locations)
                {
                    location.Value?.SavePanels();
                }
            }

            return Task.CompletedTask;
        };
    }

    public override void AddWatchingProperties()
    {
        var watchProperty = WatchingProperties
             .AddProperty("MqttConnected", false)
             .AddProperty("TranscationChange", false)
             .AddProperty("TranscationId", "");

        foreach (var property in InteractingDevice.Locations.Values)
        {
            watchProperty.AddProperty($"IsReady{property.Position}", property.IsReady);
            watchProperty.AddProperty($"IsCanCallAgv{property.Position}", property.IsCanCallAgv);
            watchProperty.AddProperty($"IsAgvWorking{property.Position}", property.IsAgvWorking);
        }
    }

    public override void CollectPropertyValues()
    {
        var realProperties = new Dictionary<string, object?>();

        realProperties["MqttConnected"] = MqttClientWrapper.IsConnected;
        realProperties["TranscationChange"] = InteractingDevice.Locations.Values.Any(x => !string.IsNullOrWhiteSpace(x.TransactionMessage));

        foreach (var property in InteractingDevice.Locations.Values)
        {
            realProperties[$"IsReady{property.Position}"] = property.IsReady;
            realProperties[$"IsCanCallAgv{property.Position}"] = property.IsCanCallAgv;
            realProperties[$"IsAgvWorking{property.Position}"] = property.IsAgvWorking;
        }

        WatchingProperties.SetValues(realProperties);
    }

    public override Task<DeviceServiceInvokeResponse> WriteProperties(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        if (deviceServiceInvokeRequest != null && deviceServiceInvokeRequest.Params != null)
        {
            WatchingProperties.SetValues(deviceServiceInvokeRequest.Params);
            if (deviceServiceInvokeRequest.Params.ContainsKey("IsReady"))
            {
            }
        }

        return base.WriteProperties(deviceServiceInvokeRequest!);
    }
}
