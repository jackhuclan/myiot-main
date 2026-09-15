using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Fundation.Alarm;

public abstract class AbstractAlarmHandler<TDevice> : DeviceShare<TDevice>, IAlarmHandler
    where TDevice : Device
{
    public AbstractAlarmHandler(IServiceProvider serviceProvider, TDevice device) : base(serviceProvider, device) { }

    public abstract void AddWatchingAlarms();
}
