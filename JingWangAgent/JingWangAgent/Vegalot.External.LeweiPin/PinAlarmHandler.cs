using VgAutoDrill.Fundation.Alarm;

namespace Vegalot.External.LeweiPin
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
