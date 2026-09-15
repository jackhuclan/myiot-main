namespace VgAutoDrill.Central.Core;

public class DeviceServiceInvocationReplayerOptions
{
    /// <summary>
    /// 最大重试次数
    /// </summary>
    public int MaxRetries { get; set; } = 10;
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

    //必须启用，否则取消调度时，无法通知钻机
    //public bool Enabled { get; set; } = false;
}
