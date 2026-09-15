namespace VegaIot.External.AgvEntity.Hik;

public class QueryTaskStatusRequest
{
    /// <summary>
    /// 请求编码
    /// </summary>
    public string reqCode { get; set; }

    /// <summary>
    /// 请求时间
    /// </summary>
    public string? reqTime { get; set; } = DateTime.Now.ToString("yyyy-MM-ddHH:mm:ss");

    /// <summary>
    ///客户端编号，如 PDA，HCWMS 等
    /// </summary>
    public string? clientCode { get; set; }

    /// <summary>
    /// 令牌号
    /// </summary>
    public string? tokenCode { get; set; }

    /// <summary>
    /// 任务号
    /// </summary>
    public string[] taskCodes { get; set; }

    /// <summary>
    /// AGV code
    /// </summary>
    public string? agvCode { get; set; }
}
