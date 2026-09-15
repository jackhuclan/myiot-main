using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Event;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgDeviceGateway.Devices.Drill.EventHandler;

namespace VgDeviceGateway.Devices.Drill
{
    public class DefaultDrillEventHandler : AbstractEventHandler<DefaultDrill>
    {
        private readonly ILogger<DefaultDrillEventHandler> logger;
        public IDrillEventHandler drillEventHandler;
        private readonly int agvOperationTypes;
        private readonly IObjectFactory factory;

        public DefaultDrillEventHandler(ILogger<DefaultDrillEventHandler> logger,
            IServiceProvider serviceProvider,
            IObjectFactory factory,
            DefaultDrill device)
            : base(serviceProvider, device)
        {
            this.logger = logger;
            this.factory = factory;
            agvOperationTypes = device.DeviceDescriptor.Extra["AgvOperationTypes"].ToInt();
            drillEventHandler = InteractionFactory.CreatetDrillEventHandler(factory, InteractingDevice, agvOperationTypes);
        }

        public override void AddWatchingEvents()
        {
            drillEventHandler.AddWatchingEvents();
        }
    }
}
