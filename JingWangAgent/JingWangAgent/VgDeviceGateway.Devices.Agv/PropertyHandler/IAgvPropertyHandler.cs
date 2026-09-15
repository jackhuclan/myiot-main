namespace VgDeviceGateway.Devices.Agv.PropertyHandler
{
    public interface IAgvPropertyHandler
    {
        public Dictionary<string, object?> PlcInformation();

        public Task<Dictionary<string, object?>> AgvChassisInformation();

        public void AddWatchingProperties();
    }
}
