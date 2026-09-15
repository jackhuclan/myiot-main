using VgAutoDrill.Central.Core.AutoMapper;
using VgAutoDrill.Central.Core.Schedule;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.Core.Mes.Model;

public class DrillScheduleTask : ScheduleTaskWithRequest
{
    [IgnoreMap]
    public int ClinkerSpindleNum
    {
        get
        {
            return EventRequest != null && EventRequest.Params.ContainsKey("ClinkerSpindleNum") ? EventRequest.Params["ClinkerSpindleNum"].ToInt() : 0;
        }
    }

    //本次申请的生料数量 RawSpindleNum
    [IgnoreMap]
    public int RawSpindleNum
    {
        get
        {
            return EventRequest != null && EventRequest.Params.ContainsKey("RawSpindleNum") ? EventRequest.Params["RawSpindleNum"].ToInt() : 0;
        }
    }

    //已有的生料数量
    [IgnoreMap]
    public int ExistRawNum
    {
        get
        {
            return EventRequest != null && EventRequest.Params.ContainsKey("ExistRawNum") ? EventRequest.Params["ExistRawNum"].ToInt() : 0;
        }
    }

    //当前使用的轴数
    [IgnoreMap]
    public int SpindleUseNum
    {
        get
        {
            return EventRequest != null && EventRequest.Params.ContainsKey("SpindleUseNum") ? EventRequest.Params["SpindleUseNum"].ToInt() : 0;
        }
    }

    [IgnoreMap]
    public bool IsFirst
    {
        get
        {
            return EventRequest != null && EventRequest.PayloadPanels.ContainsFirst();
        }
    }

    private ScheduleRequirement _requirement;

    [IgnoreMap]
    public override ScheduleRequirement Requirement
    {
        get
        {
            if (_requirement == null)
                _requirement = new ScheduleRequirement();

            _requirement.CallerDeviceId = CallerDeviceId!;
            _requirement.RequireUndrilledItemCode = ItemCode ?? string.Empty;
            _requirement.RequireDrilledItemCode = EventRequest!.PayloadPanels.DrilledItemCodes.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x)) ?? string.Empty;
            _requirement.RequireDrilledItemQty = ClinkerSpindleNum;
            _requirement.RequireUndrilledItemQty = RawSpindleNum;
            _requirement.MaterialKind = Behavior.MaterialKind;
            _requirement.IsFirst = EventRequest.PayloadPanels.ContainsFirst();
            _requirement.RouteCode = string.IsNullOrEmpty(this.RouteCode) ? string.Empty : this.RouteCode.ToLower();
            _requirement.OriginalSchedule = this;
            _requirement.InteractionSequence = this.InteractionSequence ?? VgAutoDrill.Fundation.Iot.Models.InteractionSequence.None;
            _requirement.InteractionSequenceName = InteractionSequence != null && this.InteractionSequence.HasValue ? this.InteractionSequence.ToString() : string.Empty;
            _requirement.RequestInteractionBehaviorName = this.RequestInteractionBehaviorName;
            _requirement.ScheduledTaskStatusName = this.ScheduledTaskStatus.ToString();
            _requirement.Behavior = this.Behavior;
            _requirement.HasUndrilled = this.EventRequest != null && this.EventRequest.PayloadPanels.UndrilledPanels.Count > 0;
            _requirement.PayloadUndrilledPanelsCount = this.EventRequest != null ? this.EventRequest!.PayloadPanels.UndrilledPanels.Count : 0;
            _requirement.HasDrilled = this.EventRequest != null && this.EventRequest.PayloadPanels.DrilledPanels.Count > 0;
            _requirement.PayloadDrilledPanelsCount = this.EventRequest != null ? this.EventRequest!.PayloadPanels.DrilledPanels.Count : 0;
            _requirement.HasFirst = this.EventRequest != null && this.EventRequest!.PayloadPanels.FirstPanels.Count > 0;
            _requirement.PayloadFirstPanelsCount = this.EventRequest != null ? this.EventRequest!.PayloadPanels.FirstPanels.Count : 0;
            _requirement.RequiredDeviceKind = this.RequestDeviceKind ?? Fundation.Iot.Models.DeviceKind.Unknown;
            return _requirement;
        }
    }
}
