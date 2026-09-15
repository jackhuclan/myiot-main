namespace VegaIot.External.StdAgv;

public class StdAgvSchedulerOptions
{
    /// <summary>
    /// 呼叫std agv间隔时间，单位为秒
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

    public bool IsCustomPanelCount = false;
    public string GenAgvSchedulingTaskUrl { get; set; } = string.Empty;
    public string SyncMapDatasUrl { get; set; } = string.Empty;
    public string ContinueTaskUrl { get; set; } = string.Empty;
    public string CancelTaskUrl { get; set; } = string.Empty;
    public string QueryTaskStatusUrl { get; set; } = string.Empty;
    public string QueryAgvStatusUrl { get; set; } = string.Empty;
    public string StockInfoQueryUrl { get; set; } = string.Empty;
    public string BindPodAndMatUrl { get; set; } = string.Empty;
    public string BindBoxAndFoldUrl { get; set; } = string.Empty;
    public string MaterialDistributionNotifyUrl { get; set; } = string.Empty;
    public string ManualClinkerArea { get; set; } = string.Empty;
    public string ManualRawArea { get; set; } = string.Empty;

    public string QueryAreaEmptyPosUrl { get; set; } = string.Empty;
    public string StdTyp { get; set; } = "F05";

    public string BindLineSideStockUrl { get; set; } = string.Empty;
    /// <summary>
    /// 是否启用监控海康任务
    /// </summary>
    public bool EnableStdAgvTaskMonitor = false;

    public string QueryCheckLotUrl { get; set; } = string.Empty;

    public string QueryCheckEmptySiloUrl { get; set; } = string.Empty;

}
