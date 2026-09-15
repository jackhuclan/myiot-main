using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Property;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgDeviceGateway.Devices.Drill.PropertyHandler;

namespace VgDeviceGateway.Devices.Drill
{
    public class DefaultDrillPropertyHandler : AbstractPropertyHandler<DefaultDrill>
    {
        private readonly ILogger<DefaultDrillPropertyHandler> logger;
        private readonly int agvOperationTypes;
        private readonly IObjectFactory factory;
        private IDrillPropertyHandler drillPropertyHandler;

        public DefaultDrillPropertyHandler(ILogger<DefaultDrillPropertyHandler> logger,
            IServiceProvider serviceProvider,
             IObjectFactory factory,
            DefaultDrill device)
            : base(serviceProvider, device)
        {
            this.logger = logger;
            this.factory = factory;
            agvOperationTypes = device.DeviceDescriptor.Extra["AgvOperationTypes"].ToInt();
            drillPropertyHandler = InteractionFactory.CreatetDrillPropertyHandler(factory, InteractingDevice, agvOperationTypes);

            PeriodicTimers["5s"]!.OnTick += async () =>
            {
                 if (DeviceDescriptor.AutoMode)
                {
                    var mergeInf = WatchingProperties.GetValues();
                    await DataExporter.DevicePropertiesReport(
                    new DevicePropertiesReportRequest()
                    {
                        DeviceId = DeviceDescriptor.DeviceId,
                        ProductId = DeviceDescriptor.ProductId,
                        Params = mergeInf
                    });
                }
            };
        }

        public override void AddWatchingProperties()
        {
            drillPropertyHandler.AddWatchingProperties();
        }

        public override void CollectPropertyValues()
        {
            var mergeProperties = RetrieveDataViaCncAndOther();
            SetWatchablePropertiesValue(mergeProperties);
        }

        private Dictionary<string, object?> RetrieveDataViaCncAndOther()
        {
            var mergeProperties = new Dictionary<string, object?>();
            var drillProperties = drillPropertyHandler.DrillInformation();
            var plcProperties = drillPropertyHandler.OtherInformation();

            foreach (var item in drillProperties)
            {
                mergeProperties.Add(item.Key, item.Value);
            }

            foreach (var item in plcProperties)
            {
                mergeProperties.Add(item.Key, item.Value);
            }

            return mergeProperties;
        }

        private void SetWatchablePropertiesValue(Dictionary<string, object?> values)
        {
            WatchingProperties.SetValues(values);
        }
    }
}
