using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Fundation.Event;

public interface IEventHandler : IDeviceShare
{
    void AddWatchingEvents();
}
