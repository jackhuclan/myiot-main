namespace VgAutoDrill.Fundation.Iot.Configuration;

/// <summary>
/// 中控平台配置
/// </summary>
public class CentralWebOptions
{
    /// <summary>
    /// 中控平台配置节点名称
    /// </summary>
    public const string Options = "CentralWebOptions";
    /// <summary>
    /// 上下行消息通道，http/mqtt，默认http
    /// </summary>
    public string Channel { get; set; } = "http";
    /// <summary>
    /// 中控平台设备配置API接口地址
    /// </summary>
    public string DeviceConfig { get; set; } = string.Empty;
    /// <summary>
    /// 中控平台设备属性读取API接口地址
    /// </summary>
    public string PropertiesRead { get; set; } = string.Empty;
    /// <summary>
    /// 中控平台更改设备属性API接口地址
    /// </summary>
    public string PropertiesWrite { get; set; } = string.Empty;
    /// <summary>
    /// 服务准备API接口
    /// </summary>
    public string ServicePrepare { get; set; } = string.Empty;
    /// <summary>
    /// 服务调用API接口
    /// </summary>
    public string ServiceInvoke { get; set; } = string.Empty;
    /// <summary>
    /// 服务调用API接口,version 2
    /// </summary>
    public string ServiceInvokeV2 { get; set; } = string.Empty;
    /// <summary>
    /// 服务完成API接口
    /// </summary>
    public string ServiceComplete { get; set; } = string.Empty;
    /// <summary>
    /// 事件上报API接口
    /// </summary>
    public string EventReport { get; set; } = string.Empty;
    /// <summary>
    /// 属性上报API接口
    /// </summary>
    public string PropertiesReport { get; set; } = string.Empty;
    /// <summary>
    /// 元数据上报API接口
    /// </summary>
    public string MetadataReport { get; set; } = string.Empty;
    /// <summary>
    /// 状态数据上报API接口
    /// </summary>
    public string StatusReport { get; set; } = string.Empty;
    /// <summary>
    /// 板料变化上报API接口
    /// </summary>
    public string PanelReport { get; set; } = string.Empty;
    /// <summary>
    /// 刀具变化上报API接口
    /// </summary>
    public string CutterReport { get; set; } = string.Empty;
    /// <summary>
    /// 告警数据上报API接口
    /// </summary>
    public string AlarmReport { get; set; } = string.Empty;
    /// <summary>
    /// 查询设备是否在线
    /// </summary>
    public string DeviceOnline { get; set; } = string.Empty;
    /// <summary>
    /// 获取板料编号
    /// </summary>
    public string GetNextPanelNumber { get; set; } = string.Empty;
    /// <summary>
    /// 告知中控调度开始
    /// </summary>
    public string ScheduleStart { get; set; } = string.Empty;
    /// <summary>
    /// 告知中控调度取消
    /// </summary>
    public string ScheduleCancel { get; set; } = string.Empty;
    /// <summary>
    /// 库位调度信息
    /// </summary>
    public string ScheduleLocation { get; set; } = string.Empty;
    /// <summary>
    /// 告知中控调度细节
    /// </summary>
    public string ProcessReportSchedule { get; set; } = string.Empty;
    /// <summary>
    /// 告知中控调度失败接口
    /// </summary>
    public string ScheduleFail { get; set; } = string.Empty;
    /// <summary>
    /// 告知中控调度完成
    /// </summary>
    public string ScheduleComplete { get; set; } = string.Empty;
    /// <summary>
    /// 完成钻孔任务，即将停用；请使用FinishTask
    /// </summary>
    [Obsolete("完成钻孔任务，即将停用；请使用 FinishTask")]
    public string DrillTaskComplete { get; set; } = string.Empty;
    /// <summary>
    /// 开始任务
    /// </summary>
    public string BeginTask { get; set; } = string.Empty;
    /// <summary>
    /// 完成任务
    /// </summary>
    public string FinishTask { get; set; } = string.Empty;
    /// <summary>
    /// 获取物料列表
    /// </summary>
    public string GetItemCode { get; set; } = string.Empty;
    /// <summary>
    /// App上线/下线等api接口,example: http://desktop-2vl2bdl:8001/v1/central/gateway
    /// </summary>
    public string Gateway { get; set; } = string.Empty;
}
