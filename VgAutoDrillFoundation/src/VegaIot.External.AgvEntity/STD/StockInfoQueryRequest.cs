namespace VegaIot.External.AgvEntity.STD;

/// <summary>
/// 查询区域中的库存信息
/// </summary>
public class StockInfoQueryRequest
{
    /// <summary>
    /// 请求编号
    /// </summary>
    public string reqCode { get; set; }

    /// <summary>
    /// 请求时间截
    /// </summary>
    public string? reqTime { get; set; }

    /// <summary>
    /// 客户端编号
    /// </summary>
    public string? clientCode { get; set; }

    /// <summary>
    /// 目标区域，支持区域编号或者策略编号，格式：区域编号${04},策略编号${02}
    /// </summary>
    public string targetPosArea { get; set; }
}
