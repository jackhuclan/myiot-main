namespace VegaIot.External.AgvEntity.Hik;

/// <summary>
/// 查询区域中的库存信息回复的信息
/// </summary>
public class StockInfoQueryResponse
{
    public string? code { get; set; }
    public string? message { get; set; }

    public string? reqCode { get; set; }

    public List<StockInfo>? data { get; set; }
}

public class StockInfo
{
    public string? product { get; set; }
    public string? podCode { get; set; }
    public string? address { get; set; }
    public string? addressName { get; set; }
    public string? lot { get; set; }
    public string? qty { get; set; }
    public string? sysQty { get; set; }
    public string? difQty { get; set; }
}
