namespace VgDeviceGateway.Devices.Drill.EventHandler
{
    public interface IDrillEventHandler
    {
        public void AddWatchingEvents();
        public Task LoadFileFromCenter();
    }
}
