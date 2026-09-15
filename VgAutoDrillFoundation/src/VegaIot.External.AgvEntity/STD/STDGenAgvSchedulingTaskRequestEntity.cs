namespace VegaIot.External.AgvEntity.STD;

/// <summary>
/// 生成调度AGV任务单调度请求信息, RCS通过请求参数, 生成调度AGV任务单。
/// </summary>
/// <remarks>
///     wbCode和positionCodePath至少填写其中一项，以确定任务中的位置信息。
///     若任务中需要指定多个位置信息，如起点和终点信息等，请使用positionCodePath。
/// </remarks>
public class STDGenAgvSchedulingTaskRequestEntity : STDBaseEntity
{
    #region -- 属性 --
    /// <summary>
    /// 令牌号, 由调度系统颁发。
    /// </summary>
    public String tokenCode { get; set; }
    /// <summary>
    /// 任务类型，与在RCS-2000端配置的主任务类型编号一致。
    /// 必填
    /// </summary>
    /// <remarks>
    ///     任务类别：
    ///         F01：上满料
    ///         F02：下满料
    ///         F03：上空托盘
    ///         F04：下空托盘
    ///         F05：转仓（含点到点、点到区域、区域到点）
    /// </remarks>
    public String taskTyp { get; set; }
    /// <summary>
    /// 容器类型（叉车专用）叉车项目必传
    /// </summary>
    public String ctnrTyp { get; set; }
    /// <summary>
    /// 容器编号（叉车专用）
    /// </summary>
    public String ctnrCode { get; set; }
    /// <summary>
    /// 工作位，一般为机台或工作台位置，与RCS-2000端配置的位置名称一致, 工作位名称为字母\数字\或组合, 不超过32位。
    /// </summary>
    public String wbCode { get; set; }
    /// <summary>
    /// 接驳工位
    /// 必填
    /// </summary>
    public String userCallCode { get; set; }
    /// <summary>
    /// AGV途经的用户呼叫号集合，在任务类型中未配置线路时设置，否则不需要设置。待现场地图部署、配置完成后可获取
    /// </summary>
    /// <remarks>
    ///      位置信息解释:
    ///          "FF": 代表呼叫号, 不超过32位字符
    ///          "FF01${01}":代表唯一物料,  FF01为物料批次号, ${01}代表物料批次类型.
    ///          "FF02${02}":代表策略,  FF02为策略编号, ${02}代表策略类型
    ///          "FF03${03}":代表货架,  FF03为货架
    /// </remarks>
    public String[] userCallCodePath { get; set; }
    /// <summary>
    /// 货架编号，不指定货架可以为空
    /// </summary>
    public String? podCode { get; set; }
    /// <summary>
    /// 方向 ，不指定方向可以为空
    /// </summary>
    /// <remarks>
    ///     ”180”: ”左”,
    ///     ”0”  : ”右”,
    ///     ”90” : ”上”,
    ///     ”-90”: ”下”
    /// </remarks>
    public String podDir { get; set; }
    /// <summary>
    /// 货架类型, 传空时表示随机找个货架
    /// </summary>
    /// <remarks>
    ///     找空货架传参方式如下：
    ///         1: 代表不关心货架类型, 找到空货架即可.
    ///         2: 代表从工作位获取关联货架类型, 如果未配置, 只找空货架.
    ///         货架类型编号: 只找该货架类型的空货架.
    ///         P1 小托盘
    ///         P2 大托盘
    ///         P3 小30°L架托盘
    ///         P4 大30°L架托盘
    ///         P5 65° L架托盘
    ///         P6 钻孔料架
    ///         P7 钻孔小托盘
    ///         P8 铜粉料架
    /// </remarks>
    public String podTyp { get; set; }
    /// <summary>
    /// 任务单号, 选填, 不填系统自动生成，UUID小于等于64位
    /// </summary>
    public String taskCode { get; set; }
    /// <summary>
    /// 自定义字段
    /// {
    ///     carryPro 运送工序
    ///     materialCode 物料号
    ///     public string materialLot  物料批次
    ///     batchNum 批次数量
    ///     toolCode 工具编号
    ///     planNum 计划数量
    ///     realNum 实际数量
    ///     errorInfo 错误信息
    /// }
    /// </summary>
    public GenAgvSchedulingTaskData data { get; set; }

    public string? pnlStatus { get; set; }
    #endregion

}
public class GenAgvSchedulingTaskData
{
    /// <summary>
    /// 运送工序
    /// </summary>
    public String carryPro { get; set; }
    /// <summary>
    /// 物料号
    /// </summary>
    public String materialCode { get; set; }
    /// <summary>
    /// 物料批次
    /// </summary>
    public String? materialLot { get; set; }
    /// <summary>
    /// 批次数量
    /// </summary>
    public String batchNum { get; set; }
    /// <summary>
    /// 工具编号
    /// </summary>
    public String toolCode { get; set; }
    /// <summary>
    /// 计划数量
    /// </summary>
    public String planNum { get; set; }
    /// <summary>
    /// 实际数量
    /// </summary>
    public String realNum { get; set; }
    /// <summary>
    /// 错误信息
    /// </summary>
    public String errorInfo { get; set; }
}
