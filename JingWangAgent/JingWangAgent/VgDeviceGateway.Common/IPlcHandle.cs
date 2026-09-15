namespace VgDeviceGateway.Devices.Common
{
    public interface IPlcHandle
    {
        public bool Start();

        public bool Stop();
    }
}
