namespace VegaIot.External.AgvEntity.STD;

public class StdBatteryResponseEntity
{

    public string AGV_ID { get; set; }

    public double battery { get; set; }

    public bool IsIDLE { get; set; }

    public StdBatteryResponseEntity()
    {
        AGV_ID = string.Empty;
        battery = 0.0;
        IsIDLE = false;
    }
}
