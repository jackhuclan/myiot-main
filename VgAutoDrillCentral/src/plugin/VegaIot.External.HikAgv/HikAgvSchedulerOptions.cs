namespace VegaIot.External.HikAgv;

public class HikAgvSchedulerOptions
{
    /// <summary>
    /// 呼叫hik agv间隔时间，单位为秒
    /// </summary>
    public int CallInterval { get; set; } = 2;

    /// <summary>
    /// 线边仓
    /// </summary>
    public string LineStore { get; set; } = string.Empty;

    /// <summary>
    /// 空料仓区
    /// </summary>
    public string EmptyStore { get; set; } = string.Empty;

    /// <summary>
    /// 熟料仓区
    /// </summary>
    public string ClinkerStore { get; set; } = string.Empty;

    /// <summary>
    /// 首件区
    /// </summary>
    public string FirstStore { get; set; } = string.Empty;

    public int MonitorAgvStatusInterval { get; set; } = 5;
    public int MonitorTaskStatusInterval { get; set; } = 2;

    public bool IsCustomPanelCount { get; set; } = true;
    public string GenAgvSchedulingTaskUrl { get; set; } = string.Empty;
    public string SyncMapDatasUrl { get; set; } = string.Empty;
    public string ContinueTaskUrl { get; set; } = string.Empty;
    public string CancelTaskUrl { get; set; } = string.Empty;
    public string QueryTaskStatusUrl { get; set; } = string.Empty;
    public string QueryAgvStatusUrl { get; set; } = string.Empty;
    public string StockInfoQueryUrl { get; set; } = string.Empty;
    public string MaterialDistributionNotifyUrl { get; set; } = string.Empty;

    public string HikTyp { get; set; } = "WJ02";

    /// <summary>
    /// 是否启用监控海康任务
    /// </summary>
    public bool EnableHikAgvTaskMonitor = false;
}
