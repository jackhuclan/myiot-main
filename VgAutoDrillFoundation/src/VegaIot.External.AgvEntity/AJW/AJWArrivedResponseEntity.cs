namespace VegaIot.External.AgvEntity.AJW;

public class AJWArrivedResponseEntity
{

    public AJWArrivedResponseEntity()
    {
        AGV_ID = string.Empty;
        Result = false;
    }
    /// <summary>
    ///  车辆ID
    /// </summary>
    public string AGV_ID { get; set; }

    /// <summary>
    /// 是否收到
    /// </summary>
    public bool Result { get; set; }
}
