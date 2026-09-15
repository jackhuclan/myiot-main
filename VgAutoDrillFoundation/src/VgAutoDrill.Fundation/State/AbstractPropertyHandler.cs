using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Fundation.State;

public abstract class AbstractStateHandler<TDevice> : DeviceShare<TDevice>, IStateHandler
    where TDevice : Device
{
    public AbstractStateHandler(IServiceProvider serviceProvider, TDevice device) : base(serviceProvider, device) { }

    public abstract void AddWatchingStates();
}
