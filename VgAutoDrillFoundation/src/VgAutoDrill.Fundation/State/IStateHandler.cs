using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Fundation.State;

public interface IStateHandler : IDeviceShare
{
    void AddWatchingStates();
}
