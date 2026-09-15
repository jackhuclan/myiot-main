using VgAutoDrill.Fundation.Alarm;

namespace VgDeviceGateway.Devices.Pin
{
    public class PinAlarmHandler : AbstractAlarmHandler<Pin>
    {
        public PinAlarmHandler(IServiceProvider serviceProvider, Pin device)
            : base(serviceProvider, device)
        {
        }

        public override void AddWatchingAlarms()
        {
        }
    }
}
