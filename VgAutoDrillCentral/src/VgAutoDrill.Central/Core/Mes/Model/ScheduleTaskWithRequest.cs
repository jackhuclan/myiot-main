using System.Text.Json.Serialization;
using VgAutoDrill.Central.Core.AutoMapper;
using VgAutoDrill.Central.Core.Schedule;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Mes.Model;

public class ScheduleTaskWithRequest : ScheduleTask
{
    private volatile bool _locked = false;
    public virtual DeviceEventReportRequest? EventRequest { get; set; }

    [IgnoreMap]
    public virtual InteractionBehavior Behavior
    {
        get
        {
            if (EventRequest == null) return InteractionBehavior.Noop;
            else return (InteractionBehavior)EventRequest.RequestInteractionBehavior;
        }
    }

    [IgnoreMap]
    public bool? IsEmptyPayload => EventRequest?.PayloadPanels.IsEmptyPayload;

    [IgnoreMap]
    public bool? IsEmptySiloBox => EventRequest?.PayloadPanels.IsEmptySiloBox;

    /// <summary>
    /// 任务里面包含的熟料代码
    /// </summary>
    /// <returns></returns>
    [JsonIgnore]
    [IgnoreMap]
    public IReadOnlyList<string> DrilledItemCodes => EventRequest == null ? new List<string>() : EventRequest.PayloadPanels.DrilledItemCodes;

    /// <summary>
    /// 当前任务百分比
    /// </summary>
    [IgnoreMap]
    public int Percentage { get; set; }

    [IgnoreMap]
    public virtual ScheduleRequirement Requirement { get; }

    [IgnoreMap]
    public int DrillBoardPositionStatus { get; set; }

    [IgnoreMap]
    public int BufferRawMaterialLayerBoardStatus { get; set; }

    [IgnoreMap]
    public int BufferClinkerLayerBoardStatus { get; set; }

    /// <summary>
    /// 是否被临时锁住/占用,暂时不可用
    /// </summary>
    [IgnoreMap]
    public bool IsLocked => _locked;

    public bool TryLock()
    {
        if (!_locked)
        {
            _locked = true;
            return true;
        }

        return false;
    }

    public void ReleaseLock()
    {
        _locked = false;
    }
}
