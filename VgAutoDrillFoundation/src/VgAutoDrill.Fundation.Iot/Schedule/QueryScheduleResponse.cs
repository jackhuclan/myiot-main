namespace VgAutoDrill.Fundation.Iot.Schedule;

public class QueryScheduleResponse
{
    public virtual string? Code { get; set; }
    public virtual string? RequireDeviceId { get; set; }
    public virtual string? TaskId { get; set; }
    public virtual string? ItemCode { get; set; }
    public virtual ScheduledTaskStatus? ScheduledTaskStatus { get; set; }
    public virtual string? StatusName { get; set; }
}
