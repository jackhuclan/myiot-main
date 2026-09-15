using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Property;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.UnPin
{
    public class UnPinPropertyHandler : AbstractPropertyHandler<UnPin>
    {
        private ILogger<UnPinPropertyHandler> _logger;

        public UnPinPropertyHandler(ILogger<UnPinPropertyHandler> logger,
            IServiceProvider serviceProvider,
            UnPin unPin)
            : base(serviceProvider, unPin)
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
        }

        public override void AddWatchingProperties()
        {
            var watchProperty = WatchingProperties
                 .AddProperty("MqttConnected", false)
                 .AddProperty("TranscationChange", false)
                 .AddProperty("TranscationId", "")
                 .AddProperty("PlcItemNo", "")
                 .AddProperty("HandleLeftAskUpload", 0)
                 .AddProperty("HandleLeftPanelFinish", 0)
                 .AddProperty("HandleLeftAskDownLoad", 0)
                 .AddProperty("HandleRightAskUpload", 0)
                 .AddProperty("HandleRightPanelFinish", 0)
                 .AddProperty("HandleRightAskDownLoad", 0);

            foreach (var property in InteractingDevice.WatchShelfProperty.Values)
            {
                watchProperty.AddProperty($"IsReady{property.Position}", property.IsReady);
                watchProperty.AddProperty($"IsCanCallAgv{property.Position}", property.IsCanCallAgv);
                watchProperty.AddProperty($"IsAgvWorking{property.Position}", property.IsAgvWorking);
                watchProperty.AddProperty($"HandlAskUpload{property.Position}", property.UpinCallAgvUploadSilo);
                watchProperty.AddProperty($"HandleAskDownLoad{property.Position}", property.UpinCallAgvDownLoadSilo);
            }
        }

        public override void CollectPropertyValues()
        {
            var realProperties = new Dictionary<string, object?>();

            realProperties["MqttConnected"] = InteractingDevice.MqttClientWrapper.IsConnected;

            if (InteractingDevice.modbusIpMaster != null && InteractingDevice.Connector.IsConnected)
            {
                try
                {
                    var triggerSignal = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 40, 40);
                    realProperties["HandleLeftAskUpload"] = triggerSignal[0];
                    realProperties["HandleLeftPanelFinish"] = triggerSignal[4];
                    realProperties["HandleLeftAskDownLoad"] = triggerSignal[2];
                    realProperties["HandleRightAskUpload"] = triggerSignal[10];
                    realProperties["HandleRightPanelFinish"] = triggerSignal[14];
                    realProperties["HandleRightAskDownLoad"] = triggerSignal[12];
                    realProperties["PlcItemNo"] = triggerSignal.Skip(20).Take(20).ToArray().UshortToStrings().Replace("\0", "").Replace("\r", "");
                }
                catch (Exception ex)
                {
                    InteractingDevice.Connector.IsConnected = false;
                    throw ex;
                }

                //realProperties["HandleLeftAskUpload"] = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 40, 1)[0];
                //realProperties["HandleLeftPanelFinish"] = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 44, 1)[0];
                //realProperties["HandleLeftAskDownLoad"] = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 42, 1)[0];
                //realProperties["HandleRightAskUpload"] = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 50, 1)[0];
                //realProperties["HandleRightPanelFinish"] = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 54, 1)[0];
                //realProperties["HandleRightAskDownLoad"] = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 52, 1)[0];
            }

            foreach (var property in InteractingDevice.WatchShelfProperty.Values)
            {
                realProperties[$"IsReady{property.Position}"] = property.IsReady;
                realProperties[$"IsCanCallAgv{property.Position}"] = property.IsCanCallAgv;
                realProperties[$"IsAgvWorking{property.Position}"] = property.IsAgvWorking;
                realProperties[$"HandlAskUpload{property.Position}"] = property.UpinCallAgvUploadSilo;
                realProperties[$"HandleAskDownLoad{property.Position}"] = property.UpinCallAgvDownLoadSilo;
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
                    InteractingDevice.isReady = deviceServiceInvokeRequest.Params["IsReady"].ToBool();
                }
            }

            return base.WriteProperties(deviceServiceInvokeRequest!);
        }
    }
}
