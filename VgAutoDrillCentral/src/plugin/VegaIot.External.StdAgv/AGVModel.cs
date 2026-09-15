using VegaIot.External.AgvEntity.STD;

namespace VegaIot.External.StdAgv;

public class BindSiloStockEntity : STDBaseEntity
{
    public string mapDataCode { get; set; }
    /// <summary>
    /// 托盘编号
    /// </summary>
    public string? podCode { get; set; }
    /// <summary>
    /// 1:绑定 0:解绑
    /// </summary>
    public string indBind { get; set; }
}

public class QueryEmptyPos : STDBaseEntity
{
    public string targetPosArea { get; set; }
}

public class QueryEmptyPosRes : STDResponse
{
    public new List<string> data { get; set; } = new List<string>();
}

public class QueryCheckLotReq : STDBaseEntity
{
    public string targetPosArea { get; set; }
    public string lot { get; set; }
    public string pnlStatus { get; set; }
}

public class QueryCheckLotRes : STDResponse
{
    public new bool data { get; set; } = false;
}
