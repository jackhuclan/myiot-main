namespace VgAutoDrill.Central.Core.Handler.Alarm;

public class DeviceAlarmListenerOptions
{
    /// <summary>
    /// 
    /// </summary>
    public const string Options = "DeviceAlarmListenerOptions";
    /// <summary>
    /// 预处理项数
    /// </summary>
    public int PrefetchCount { get; set; } = 100;
    /// <summary>
    /// 预处理间隔多少秒
    /// </summary>
    public int PrefetchInterval { get; set; } = 5;
    /// <summary>
    /// 预处理多少天以内的数据
    /// </summary>
    public int PrefetchDays { get; set; } = 7;

    public bool Enabled { get; set; } = false;
}
