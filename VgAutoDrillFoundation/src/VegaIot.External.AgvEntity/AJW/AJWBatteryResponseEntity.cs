namespace VegaIot.External.AgvEntity.AJW;

public class AJWBatteryResponseEntity
{
    public AJWBatteryResponseEntity()
    {
        AGV_ID = string.Empty;
        battery = 0;
        IsIDLE = false;
    }
    /// <summary>
    ///  车辆ID
    /// </summary>
    public string AGV_ID { get; set; }

    /// <summary>
    /// 电量
    /// </summary>
    public double battery { get; set; }

    public bool IsIDLE { get; set; }
}
