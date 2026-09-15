namespace VgAutoDrill.Fundation.Iot;

public class PeriodicTimerExecutorFactoryOptions
{
    /// <summary>
    /// 是否启用，默认启用
    /// </summary>
    public bool Enabled { get; set; } = true;
    /// <summary>
    /// 默认时钟
    /// </summary>
    public List<string> Intervals { get; set; } = new List<string>() { "100ms", "5s", "10s", "20s", "30s", "1m" };
}
