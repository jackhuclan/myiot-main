namespace VgAutoDrill.Central.Core.Manager;

public class AlarmLogPersistOptions
{
    public int BatchSize { get; set; } = 2000;
    public int FlushTimeout { get; set; } = 10;
}
