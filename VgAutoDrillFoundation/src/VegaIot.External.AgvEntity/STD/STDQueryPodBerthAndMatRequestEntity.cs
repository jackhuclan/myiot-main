namespace VegaIot.External.AgvEntity.STD;
/// <summary>
/// 货架储位与物料批次关系
/// </summary>
public class STDQueryPodBerthAndMatRequestEntity : STDBaseEntity
{
    #region -- 属性 --
    /// <summary>
    /// 令牌号, 由调度系统颁发。
    /// </summary>
    public String tokenCode { get; set; }
    /// <summary>
    /// 货架编号
    /// </summary>
    public String podCode { get; set; }
    /// <summary>
    /// 物料批次
    /// </summary>
    public String materialLot { get; set; }
    /// <summary>
    /// 位置编号，地图位置的别名，能任意命名(字母+数字)，但要唯一，由RCS-2000 界面配置。
    /// </summary>
    public String positionCode { get; set; }
    /// <summary>
    /// 区域编号
    /// </summary>
    public String areaCode { get; set; }
    /// <summary>
    /// 地图简称
    /// </summary>
    public String mapShortName { get; set; }
    #endregion
}
