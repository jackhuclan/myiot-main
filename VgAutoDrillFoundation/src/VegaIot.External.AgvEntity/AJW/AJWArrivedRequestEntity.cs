namespace VegaIot.External.AgvEntity.AJW;

public class AJWArrivedRequestEntity
{
    public AJWArrivedRequestEntity()
    {
        AGV_ID = string.Empty;
        IsSuccess = false;
        position = string.Empty;
        X_Error = string.Empty;
        Z_Error = string.Empty;
        Exception = string.Empty;
        Status = string.Empty;
        TaskId = string.Empty;
    }
    /// <summary>
    ///  车辆ID
    /// </summary>
    public string AGV_ID { get; set; }

    /// <summary>
    /// 是否到位
    /// </summary>
    public bool IsSuccess { get; set; }
    /// <summary>
    /// 站点
    /// </summary>
    public string? position { get; set; }
    /// <summary>
    /// X偏差
    /// </summary>
    public string? X_Error { get; set; }
    /// <summary>
    /// Z偏差
    /// </summary>
    public string? Z_Error { get; set; }
    /// <summary>
    /// 异常信息
    /// </summary>
    public string? Exception { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// 任务Id
    /// </summary>
    public string TaskId { get; set; }
}
