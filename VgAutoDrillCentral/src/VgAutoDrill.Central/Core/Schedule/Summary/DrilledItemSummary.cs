namespace VgAutoDrill.Central.Core.Schedule.Summary;

/// <summary>
/// 需要的/负载的 熟料汇总；对钻机来说是需要的，对agv和fork来说是负载的
/// </summary>
public class DrilledItemSummary : ItemSummary
{
    public List<string> RouteCodes { get; set; } = new();
    public List<long> ScheduleIds { get; set; } = new();
    public List<string> DeviceIds { get; set; } = new();
}
