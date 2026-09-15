using VgAutoDrill.Central.Core.AutoMapper;

namespace VgAutoDrill.Central.Core.Mes.Model;

public class AlarmLog
{
    public long SyncId { get; set; } = DateTime.Now.Ticks;
    public string AlarmCode { get; set; }
    public string AlarmName { get; set; }
    [IgnoreMap]
    public string Level { get; set; }
    [IgnoreMap]
    public string DeviceId { get; set; }
    [IgnoreMap]
    public string LocationCode { get; set; }
    public bool IsHandled { get; set; }

    public DateTime AlarmTime { get; set; }
    public DateTime? HandleTime { get; set; }

    [IgnoreMap]
    public bool IsSynced { get; set; } = false;
}
