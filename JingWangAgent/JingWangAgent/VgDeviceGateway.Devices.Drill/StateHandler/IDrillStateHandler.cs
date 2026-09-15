using VgAutoDrill.Fundation.Iot;

namespace VgDeviceGateway.Devices.Drill.StateHandler
{
    public interface IDrillStateHandler
    {
        List<string> ReadyProperties => SetReadyProperties();
        List<string> WorkingProperties => SetReadyProperties();
        List<string> ExceptionProperties => SetExceptionProperties();

        public List<string> SetReadyProperties();

        public List<string> SetWorkProperties();

        public List<string> SetExceptionProperties();

        public bool SetExceptionStateCondition(WatchableProperties properties);

        public bool SetWorkingStateCondition(WatchableProperties properties);

        public bool SetReadyStateCondition(WatchableProperties properties);
    }
}
