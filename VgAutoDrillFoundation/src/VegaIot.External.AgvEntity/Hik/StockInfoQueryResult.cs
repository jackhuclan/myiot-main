namespace VegaIot.External.AgvEntity.Hik;

/// <summary>
/// 查询区域中的库存信息回复的信息
/// </summary>
public class StockInfoQueryResult
{
    /// <summary>
    /// 物料号
    /// </summary>
    public string? product { get; set; }

    /// <summary>
    /// 托盘号
    /// </summary>
    public string? podCode { get; set; }

    /// <summary>
    /// 所在位置
    /// </summary>
    public string? address { get; set; }

    /// <summary>
    /// 所在位置区域
    /// </summary>
    public string? addressName { get; set; }

    /// <summary>
    /// 批次
    /// </summary>
    public string? lot { get; set; }

    /// <summary>
    /// 本托盘数量
    /// </summary>
    public string? qty { get; set; }

    /// <summary>
    /// 系统数量
    /// </summary>
    public string? sysQty { get; set; }

    /// <summary>
    /// 差数
    /// </summary>
    public string? difQty { get; set; }
}
