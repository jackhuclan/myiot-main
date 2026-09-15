namespace VgAutoDrill.Fundation.Iot.Configuration;

/// <summary>
/// 
/// </summary>
public class ClickHouseOptions
{
    /// <summary>
    /// 
    /// </summary>
    public const string Options = "ClickHouseOptions";
    /// <summary>
    /// 
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public bool Compression { get; set; } = true;
    /// <summary>
    /// 
    /// </summary>
    public bool Session { get; set; } = false;
    /// <summary>
    /// 
    /// </summary>
    public bool CustomDecimals { get; set; } = true;
    public bool Enabled { get; set; } = true;
    public bool EventReportEnabled { get; set; } = true;
    public bool PropertiesReportEnabled { get; set; } = true;
    public bool ServiceReportEnabled { get; set; } = true;
    public bool StatusReportEnabled { get; set; } = true;
    public bool AlarmReportEnabled { get; set; } = true;
}
