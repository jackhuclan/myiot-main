namespace VegaIot.External.AgvEntity.STD;
/// <summary>
/// 查询区域中的空储位信息
/// </summary>
public class STDQueryAreaEmptyPosRequestEntity : STDBaseEntity
{
    #region -- 属性 --
    /// <summary>
    /// 目标区域，支持区域编号或者策略编号，格式：区域编号${04},策略编号${02}
    /// 必填
    /// </summary>
    public String targetPosArea { get; set; }
    #endregion
}
