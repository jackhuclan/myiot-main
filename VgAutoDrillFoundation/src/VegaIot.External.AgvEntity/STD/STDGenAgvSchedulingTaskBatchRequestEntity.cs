namespace VegaIot.External.AgvEntity.STD;

/// <summary>
/// 传入一批任务信息，按照这些信息生成多个任务
/// </summary>
public class STDGenAgvSchedulingTaskBatchRequestEntity : STDBaseEntity
{
    #region -- 属性 --
    /// <summary>
    /// 任务组编号，每次请求唯一不重复
    /// 必填 
    /// </summary>
    public String taskGroupCode { get; set; }
    /// <summary>
    /// 呼叫站点
    /// 必填 
    /// </summary>
    public String wbCode { get; set; }
    /// <summary>
    /// 任务类型，与在RCS-2000端配置的主任务类型编号一致
    /// 必填 
    /// </summary>
    public String taskTyp { get; set; }
    /// <summary>
    /// 任务集合
    /// </summary>
    public List<TaskGroup> taskGroups { get; set; }
    #endregion

}
/// <summary>
/// 任务信息
/// </summary>
public class TaskGroup
{
    /// <summary>
    /// 路径集合，(批次，终点)
    /// 必填 
    /// </summary>
    public String[] userCallCodePath { get; set; }
    /// <summary>
    /// 优先级，从（1~5）级，最大优先级最高
    /// </summary>
    public String priority { get; set; }
    /// <summary>
    /// 任务单编号，可不传，RCS系统随机生成
    /// </summary>
    public String taskCode { get; set; }
    /// <summary>
    /// 批次(lot)号
    /// 必填 
    /// </summary>
    public String materialLot { get; set; }
    /// <summary>
    /// 货架/托盘编号
    /// </summary>
    public String podCode { get; set; }
}
