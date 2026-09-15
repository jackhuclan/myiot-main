namespace VegaIot.External.AgvEntity.STD;

public class STDBindPodAndMatRequestEntity : STDBaseEntity
{
    #region -- 属性 --
    /// <summary>
    /// 物料批次号，按“|”隔开，最多10Lot
    /// </summary>
    public String materialLot { get; set; }
    /// <summary>
    /// 物料编号，按“|”隔开
    /// </summary>
    public String materialCode { get; set; }
    /// <summary>
    /// 批次系统数量，按“|”隔开
    /// </summary>
    public String sysLotNum { get; set; }
    /// <summary>
    /// 批次本托盘数量，按“|”隔开
    /// </summary>
    public String podLotNum { get; set; }
    /// <summary>
    /// "1"：绑定， "0"：解绑
    /// </summary>
    public String indBind { get; set; }
    #endregion

}
