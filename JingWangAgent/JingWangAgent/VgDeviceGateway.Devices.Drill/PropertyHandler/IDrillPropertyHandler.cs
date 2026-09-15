namespace VgDeviceGateway.Devices.Drill.PropertyHandler
{
    public interface IDrillPropertyHandler
    {
        public Dictionary<string, object?> DrillInformation();

        public Dictionary<string, object?> OtherInformation();

        public void AddWatchingProperties();
    }
}
