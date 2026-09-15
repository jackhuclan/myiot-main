namespace VegaIot.External.AgvEntity.AJW;
public class AJWBatteryRequestEntity
{
    public AJWBatteryRequestEntity()
    {
        AGV_ID = string.Empty;
        battery = 0;
    }
    /// <summary>
    ///  车辆ID
    /// </summary>
    public string AGV_ID { get; set; }

    /// <summary>
    /// 电量
    /// </summary>
    public double battery { get; set; }
}
