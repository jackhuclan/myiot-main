namespace VegaIot.External.AgvEntity.STD;

/// <summary>
/// 绑定货架信息
/// </summary>
public class BindSiloEntity : STDBaseEntity
{
    /// <summary>
    /// 物料批次号
    /// </summary>
    public String materialLot { get; set; }
    /// <summary>
    /// 货架信息
    /// </summary>
    public String? podCode { get; set; }
    /// <summary>
    /// 1:绑定 0:解绑
    /// </summary>
    public String indBind { get; set; }
}
