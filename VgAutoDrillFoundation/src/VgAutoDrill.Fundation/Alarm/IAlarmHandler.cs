using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Fundation.Alarm;

public interface IAlarmHandler : IDeviceShare
{
    void AddWatchingAlarms();
}
