namespace VegaIot.External.AgvEntity.STD;
/// <summary>
/// 绑定货架信息
/// </summary>
public class BindSiloBoxEntity : STDBaseEntity
{
    /// <summary>
    /// 托盘编号
    /// </summary>
    public String? podCode { get; set; }
    /// <summary>
    /// 1:绑定 0:解绑
    /// </summary>
    public String indBind { get; set; }
    /// <summary>
    /// 托盘物料信息
    /// </summary>
    public List<PodInfoEntity>? podList { get; set; }
}

/// <summary>
/// 托盘信息
/// </summary>
public class PodInfoEntity
{
    /// <summary>
    /// 层号
    /// </summary>
    public Int32 boxLayerNo { get; set; }
    /// <summary>
    /// 批次号
    /// </summary>
    public String? lotNo { get; set; }
    /// <summary>
    /// 铝片码
    /// </summary>
    public String? foldQrCode { get; set; }
    /// <summary>
    /// PLN 码
    /// </summary>
    public List<String>? PanelList { get; set; }
    /// <summary>
    /// 板料状态 1-生料,2-熟料
    /// </summary>
    public string pnlStatus { get; set; } = "0";
}
