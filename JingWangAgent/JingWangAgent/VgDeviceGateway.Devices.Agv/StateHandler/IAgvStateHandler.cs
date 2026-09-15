using VgAutoDrill.Fundation.Iot;

namespace VgDeviceGateway.Devices.Agv.StateHandler
{
    public interface IAgvStateHandler
    {
        List<string> OnlineProperties => SetPropertiesOnline();

        public List<string> SetPropertiesOnline();

        Predicate<WatchableProperties> StateOnlineCondition { get; }

        List<string> ReadyProperties => SetPropertiesReady();

        public List<string> SetPropertiesReady();

        public bool SetStateConditionReady(WatchableProperties properties);

        List<string> WorkingProperties => SetPropertiesWorking();

        public List<string> SetPropertiesWorking();

        public bool SetStateConditionWorking(WatchableProperties properties);

        List<string> LowBatteryProperties => SetPropertiesLowBattery();

        public List<string> SetPropertiesLowBattery();

        public bool SetStateConditionLowBattery(WatchableProperties properties);

        List<string> ChargingProperties => SetPropertiesCharging();

        public List<string> SetPropertiesCharging();

        public bool SetStateConditionCharging(WatchableProperties properties);

        List<string> ExceptionProperties => SetPropertiesException();

        public List<string> SetPropertiesException();

        public bool SetStateConditionException(WatchableProperties properties);

        public void UpdatePLcSiloInfo();

        public void UpdatePLcError();

        public void UpdatePLcAgvStatus(DeviceStatus newStatus);
    }
}
