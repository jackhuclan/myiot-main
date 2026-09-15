namespace VegaIot.External.AgvEntity.STD;
/// <summary>
/// 位置禁用与启用
/// </summary>
public class STDLockPositionRequestEntity : STDBaseEntity
{
    #region -- 属性 --
    /// <summary>
    /// 令牌号, 由调度系统颁发。
    /// </summary>
    public String tokenCode { get; set; }
    /// <summary>
    /// 位置编号，地图位置的别名，能任意命名(字母+数字)，但要唯一，由RCS-2000 界面配置。
    /// </summary>
    public String positionCode { get; set; }
    /// <summary>
    /// "1"： 启用， "0"：禁用
    /// </summary>
    public String indBind { get; set; }
    #endregion
}
