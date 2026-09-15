using VgAutoDrill.Central.Core.Mes.Model;

namespace VgAutoDrill.Central.Core.Schedule;

internal class DrillRequirementMatchResult
{
    /// <summary>
    /// pin, unpin, drill 为主人
    /// </summary>
    public ScheduleTaskWithRequest MasterSchedule { get; set; }

    /// <summary>
    /// fork, shelf, transferRack中转位为 仆人
    /// </summary>
    public ScheduleTaskWithRequest? ServantSchedule { get; set; }

    /// <summary>
    /// 当匹配调度是叉齿时，对应插齿所在的叉齿分区
    /// </summary>
    public Partition? ForkPartition { get; set; }
}
