namespace VegaIot.External.AgvEntity.STD;
/// <summary>
/// 更新批次系统数量
/// </summary>
public class STDUpdateStockSysNumRequestEntity : STDBaseEntity
{
    #region -- 属性 --
    /// <summary>
    /// 批次号
    /// </summary>
    public String lot { get; set; }
    /// <summary>
    /// 批次系统数量
    /// </summary>
    public String sysLotNum { get; set; }
    #endregion

}
