using VgAutoDrill.Fundation.Event;
using VgDeviceGateway.Devices.Agv.EventHandler;

namespace VgDeviceGateway.Devices.Agv
{
    public class DefaultAgvEventHandler : AbstractEventHandler<DefaultAgv>
    {
        private IAgvEventHandler agvEventHandler;

        public DefaultAgvEventHandler(
            IServiceProvider serviceProvider,
            DefaultAgv defaultagv)
            : base(serviceProvider, defaultagv)
        {
            agvEventHandler = InteractionAgvFactory.CreateAgvEventHandler(InteractingDevice);
        }

        public override void AddWatchingEvents()
        {
            agvEventHandler.AddWatchingEventsLocal();
        }
    }
}
