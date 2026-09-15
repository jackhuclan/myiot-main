namespace VegaIot.External.AgvEntity.STD;

/// <summary>
/// 取消任务
/// </summary>
public class STDCancelTaskRequestEntity : STDBaseEntity
{
    #region -- 属性 --
    public string cancelType { get; set; } = string.Empty;

    public int taskId { get; set; }
    ///// <summary>
    ///// 令牌号, 由调度系统颁发。由RCS-2000告知上层系统
    ///// </summary>
    //public String tokenCode { get; set; }
    ///// <summary>
    ///// 取消类型
    ///// </summary>
    ///// <remarks>
    /////     0表示：取消后货架直接放地上
    /////     1表示：AGV仍然背着货架， 根据回库区域执行回库指令， 只有潜伏车支持。
    /////     默认的取消模式为0
    /////     forcecancel = 1时有意义，
    ///// </remarks>
    //public String forceCancel { get; set; }
    ///// <summary>
    ///// 回库区域编号
    ///// </summary>
    ///// <remarks>
    /////如果为空，采用货架配置的库区。
    ///// </remarks>
    //public String matterArea { get; set; }
    ///// <summary>
    ///// 取消该AGV正在执行的任务单
    ///// </summary>
    //public String agvCode { get; set; }
    ///// <summary>
    ///// 任务单编号, 取消该任务单
    ///// </summary>
    //public String taskCode { get; set; }
    #endregion
}

public class STDCancelTaskResponseEntity : STDBaseResponse<STDCancelTaskResponseEntity>
{
}
