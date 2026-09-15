namespace VegaIot.External.AgvEntity.STD;
/// <summary>
/// 继续执行任务
/// </summary>
public class STDContinueTaskRequestEntity : STDBaseEntity
{
    #region -- 属性 --
    /// <summary>
    /// 任务单号,选填, 不填系统自动生 成，必须为 32 位 UUID
    /// </summary>
    public String taskCode { get; set; }
    #endregion

}
