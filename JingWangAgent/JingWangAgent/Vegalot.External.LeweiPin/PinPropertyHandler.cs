using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Property;
using VgAutoDrill.Fundation.Utils;

namespace Vegalot.External.LeweiPin
{
    public class PinPropertyHandler : AbstractPropertyHandler<Pin>
    {
        private bool change = false;
        private ILogger<PinPropertyHandler> _logger;

        public PinPropertyHandler(ILogger<PinPropertyHandler> logger,
            IServiceProvider serviceProvider,
            Pin siloShelf)
            : base(serviceProvider, siloShelf)
        {
            _logger = logger;

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

            PeriodicTimers[InteractingDevice.DeviceDescriptor.Extra["SyncLocationInterval"].ToStr()]!.OnTick += async () =>
            {
                if (InteractingDevice.IsCanLoopSync)
                {
                    foreach (var location in InteractingDevice.WatchShelfProperty)
                    {
                        await InteractingDevice.PayloadPanels.RaiseCollectionChangedEvent(location.Value.LocationCode);
                    }
                }
            };
        }

        public override void AddWatchingProperties()
        {
            var watchProperty = WatchingProperties
                 .AddProperty("MqttConnected", false)
                 .AddProperty("TranscationChange", false)
                 .AddProperty("TranscationId", "")
                 .AddProperty("HandleLeftAskUpload", 0)
                 .AddProperty("HandleLeftPanelFinish", 0)
                 .AddProperty("HandleLeftAskDownLoad", 0)
                 .AddProperty("HandleRightAskUpload", 0)
                 .AddProperty("HandleRightPanelFinish", 0)
                 .AddProperty("HandleRightAskDownLoad", 0)
                 ;

            foreach (var property in InteractingDevice.WatchShelfProperty.Values)
            {
                watchProperty.AddProperty($"IsReady{property.Position}", property.IsReady);
                watchProperty.AddProperty($"IsCanCallAgv{property.Position}", property.IsCanCallAgv);
                watchProperty.AddProperty($"IsAgvWorking{property.Position}", property.IsAgvWorking);
                watchProperty.AddProperty($"HandlAskUpload{property.Position}", property.PinCallAgvUploadSilo);
                watchProperty.AddProperty($"HandleAskDownLoad{property.Position}", property.PinCallAgvDownLoadSilo);
            }
        }

        public override void CollectPropertyValues()
        {
            var realProperties = new Dictionary<string, object?>();

            realProperties["MqttConnected"] = InteractingDevice.MqttClientWrapper.IsConnected;

            foreach (var property in InteractingDevice.WatchShelfProperty.Values)
            {
                realProperties[$"IsReady{property.Position}"] = property.IsReady;
                realProperties[$"IsCanCallAgv{property.Position}"] = property.IsCanCallAgv;
                realProperties[$"IsAgvWorking{property.Position}"] = property.IsAgvWorking;
                realProperties[$"HandlAskUpload{property.Position}"] = property.PinCallAgvUploadSilo;
                realProperties[$"HandleAskDownLoad{property.Position}"] = property.PinCallAgvDownLoadSilo;
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
                    //InteractingDevice.isReady = deviceServiceInvokeRequest.Params["IsReady"].ToBool();
                }
            }

            return base.WriteProperties(deviceServiceInvokeRequest!);
        }
    }
}
