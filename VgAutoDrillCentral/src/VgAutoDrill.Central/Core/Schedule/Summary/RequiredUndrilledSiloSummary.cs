namespace VgAutoDrill.Central.Core.Schedule.Summary;

/// <summary>
/// 需要的生料料仓汇总；对钻机来说是需要的，对agv和fork来说是负载的
/// </summary>
public class RequiredUndrilledSiloSummary : ItemSummary
{
    public int SiloCount { get; set; }
    public List<long> ScheduleIds { get; set; } = new();
    public List<string> DeviceIds { get; set; } = new();
    public List<string> LocatioCodes { get; set; } = new();
}
