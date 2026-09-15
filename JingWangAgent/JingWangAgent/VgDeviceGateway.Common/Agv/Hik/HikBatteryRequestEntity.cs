namespace VgDeviceGateway.Devices.Common.Agv.Hik;

public class HikBatteryRequestEntity
{
    public string AGV_ID { get; set; }

    public double battery { get; set; }

    public HikBatteryRequestEntity()
    {
        AGV_ID = string.Empty;
        battery = 0.0;
    }
}
