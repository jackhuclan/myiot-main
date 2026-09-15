namespace VegaIot.External.AgvEntity.STD;

public class StdBatteryRequestEntity
{
    public string AGV_ID { get; set; }

    public double battery { get; set; }

    public StdBatteryRequestEntity()
    {
        AGV_ID = string.Empty;
        battery = 0.0;
    }
}
