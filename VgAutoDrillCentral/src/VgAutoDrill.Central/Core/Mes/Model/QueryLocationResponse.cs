namespace VgAutoDrill.Central.Core.Mes.Model;

public class QueryLocationResponse
{
    /// <summary>
    /// 库位编号
    /// </summary>
    public string? Code { get; set; }

    public string? SiloCode { get; set; }

    public string? DeviceId { get; set; }
    public string? PositionCode { get; set; }
    /// <summary>
    /// 大车内点
    /// </summary>
    public string? FeedAGVInnerPoint { get; set; }
    /// <summary>
    /// 大车外点
    /// </summary>
    public string? FeedAGVOutputPoint { get; set; }

    /// <summary>
    /// 大车休息点
    /// </summary>
    public string? FeedAGVRestPoint { get; set; }

    /// <summary>
    /// 小车内点
    /// </summary>
    public string? TransAGVInnerPoint { get; set; }

    /// <summary>
    /// 小车外点
    /// </summary>
    public string? TransAGVOutputPoint { get; set; }

    /// <summary>
    /// 小车休息点
    /// </summary>
    public string? TransAGVRestPoint { get; set; }

}
