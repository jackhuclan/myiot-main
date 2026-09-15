using VgAutoDrill.Fundation.Alarm;

namespace Vegalot.External.LeweiUnPin
{
    public class UnPinAlarmHandler : AbstractAlarmHandler<UnPin>
    {
        public UnPinAlarmHandler(IServiceProvider serviceProvider, UnPin device)
            : base(serviceProvider, device)
        {
        }

        public override void AddWatchingAlarms()
        {
        }
    }
}
