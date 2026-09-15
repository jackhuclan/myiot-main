namespace VgDeviceGateway.Devices.Common.Agv.Hik;

public class HikBatteryResponseEntity
{
    public string AGV_ID { get; set; }

    public double battery { get; set; }

    public bool IsIDLE { get; set; }

    public HikBatteryResponseEntity()
    {
        AGV_ID = string.Empty;
        battery = 0.0;
        IsIDLE = false;
    }
}
