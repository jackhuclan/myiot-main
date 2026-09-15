using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Fundation.Event;

public abstract class AbstractEventHandler<TDevice> : DeviceShare<TDevice>, IEventHandler
    where TDevice : Device
{
    public AbstractEventHandler(IServiceProvider serviceProvider, TDevice device) : base(serviceProvider, device) { }

    public abstract void AddWatchingEvents();
}
