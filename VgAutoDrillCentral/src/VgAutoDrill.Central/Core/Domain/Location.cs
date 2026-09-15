using System.Diagnostics;
using System.Text.Json.Serialization;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Schedule;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Extensions;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Iot.Transportation;
using Panel = VgAutoDrill.Fundation.Iot.Models.Panel;

namespace VgAutoDrill.Central.Core.Domain;

/// <summary>
/// 库位
/// </summary>
[DebuggerDisplay("LocationCode={Code}")]
public class Location : IEquatable<Location>
{
    private volatile ScheduleTaskWithRequest? _schedule;
    private volatile bool _scheduleChanged = false;
    private volatile bool _locked = false;
    private volatile PanelList _panels = new();
    private DateTime _lastUpdated = DateTime.Now;
    private TimeSpan _lastInterval = TimeSpan.Zero;
    private DateTime _locationTasklessStartTime = DateTime.Now;
    private DateTime _locationTasklessEndTime = DateTime.Now;

    public event Func<ScheduleTaskWithRequest, ScheduleTaskWithRequest, Task> OnScheduleChanged;

    public event Func<Location, Task> OnPanelChanged;

    private volatile int _doingWork = 0;

    public Location()
    { }

    /// <summary>
    /// 库位编号
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 料仓号
    /// </summary>
    public string SiloCode => OriginPanels.SiloCode;

    public string DeviceId { get; set; }
    public string? PositionCode { get; set; }

    public int Index { get; set; }

    /// <summary>
    /// 大车内点
    /// </summary>
    public string? FeedAGVInnerPoint { get; set; }

    /// <summary>
    /// 大车外点
    /// </summary>
    public string? FeedAGVOutputPoint { get; set; }

    /// <summary>
    /// 大车休息点
    /// </summary>
    public string? FeedAGVRestPoint { get; set; }

    /// <summary>
    /// 小车内点
    /// </summary>
    public string? TransAGVInnerPoint { get; set; }

    /// <summary>
    /// 小车外点
    /// </summary>
    public string? TransAGVOutputPoint { get; set; }

    /// <summary>
    /// 小车休息点
    /// </summary>
    public string? TransAGVRestPoint { get; set; }

    public int Status { get; set; }

    public DeviceKind? LocationDeviceKind { get; set; }

    /// <summary>
    /// 是否是放料箱的库位
    /// </summary>
    public bool IsRackLocation => IsForkLocation || IsWIPLocation;

    /// <summary>
    /// 是否是中转位库位
    /// </summary>
    public bool IsForkLocation => LocationDeviceKind == DeviceKind.PanelSiloFork;

    /// <summary>
    /// 是否是wip(公共缓存区)库位
    /// </summary>
    public bool IsWIPLocation => LocationDeviceKind == DeviceKind.PublicPanelSiloWIP;

    /// <summary>
    /// 当前库位所在分区
    /// </summary>
    [JsonIgnore]
    public Mes.Model.Partition? Partition { get; set; }

    /// <summary>
    /// 分区编码
    /// </summary>
    public string PartitionCode { get; set; }

    /// <summary>
    /// 当前库位所属设备
    /// </summary>
    [JsonIgnore]
    public DeviceProxy? HostDevice { get; set; }

    public bool HasHostDevice => HostDevice != null;
    public bool HasSchedule => Schedule != null;
    public bool IsNotStarted => Schedule != null && Schedule.IsNotStarted;
    public ScheduledTaskStatus? LocationScheduledTaskStatus => Schedule?.ScheduledTaskStatus;

    /// <summary>
    /// 服务于哪个需求
    /// </summary>
    [JsonIgnore]
    public ScheduleTaskWithRequest? ServingForSchedule { get; set; }

    public long? ServingForScheduleId => ServingForSchedule?.Id;
    public long? ScheduleId => Schedule?.Id;
    public string? ScheduleCode => Schedule?.Code;
    public DeviceKind? ScheduleRequestDeviceKind => Schedule?.RequestDeviceKind;

    /// <summary>
    /// 最近调度距离上次调度的时间间隔
    /// </summary>
    public TimeSpan ScheduleDuration => _lastInterval;

    /// <summary>
    /// 最近调度开始的时间点
    /// </summary>
    public DateTime? ScheduleWaitingStartTime => _schedule?.CreateTime;

    /// <summary>
    /// 最近调度结束的时间点
    /// </summary>
    public DateTime? ScheduleWaitingEndTime => _schedule?.AllocateTime;

    /// <summary>
    /// 库位上的调度等待被执行的时长，等待时长或者空闲时长
    /// </summary>
    public TimeSpan? ScheduleWaitingDuration
    {
        get
        {
            bool condition1 = !ScheduleWaitingEndTime.HasValue && ScheduleWaitingStartTime.HasValue;
            bool condition2 = ScheduleWaitingEndTime.HasValue && ScheduleWaitingStartTime.HasValue
                && ScheduleWaitingEndTime.Value < ScheduleWaitingStartTime.Value;
            bool condition3 = ScheduleWaitingEndTime.HasValue && ScheduleWaitingStartTime.HasValue
                && ScheduleWaitingEndTime.Value > ScheduleWaitingStartTime.Value;

            if (condition1 || condition2)
            {
                return DateTime.Now - ScheduleWaitingStartTime!.Value;
            }

            if (condition3)
            {
                return ScheduleWaitingEndTime!.Value - ScheduleWaitingStartTime!.Value;
            }

            return null;
        }
    }

    /// <summary>
    /// 最近调度开始调度的时间点
    /// </summary>
    public DateTime? ScheduleRunningStartTime => _schedule?.RunningTime;

    /// <summary>
    /// 最近调度结束调度的时间点
    /// </summary>
    public DateTime? ScheduleRunningEndTime => _schedule?.CompletedTime ?? _schedule?.CanceledTime;

    /// <summary>
    /// 库位上的调度执行调度的时长
    /// </summary>
    public TimeSpan? ScheduleRunningDuration
    {
        get
        {
            bool condition1 = !ScheduleRunningEndTime.HasValue && ScheduleRunningStartTime.HasValue;
            bool condition2 = ScheduleRunningEndTime.HasValue && ScheduleRunningStartTime.HasValue
                && ScheduleRunningEndTime.Value < ScheduleRunningStartTime.Value;
            bool condition3 = ScheduleRunningEndTime.HasValue && ScheduleRunningStartTime.HasValue
                && ScheduleRunningEndTime.Value > ScheduleRunningStartTime.Value;

            if (condition1 || condition2)
            {
                return DateTime.Now - ScheduleRunningStartTime!.Value;
            }

            if (condition3)
            {
                return ScheduleRunningEndTime!.Value - ScheduleRunningStartTime!.Value;
            }

            return null;
        }
    }

    /// <summary>
    /// 库位无任务的起始时间
    /// </summary>
    public DateTime LocationTasklessStartTime => _locationTasklessStartTime;

    /// <summary>
    /// 库位无任务的起始时间
    /// </summary>
    public DateTime LocationTasklessEndTime => _locationTasklessEndTime;

    /// <summary>
    /// 库位无任务持续时长
    /// </summary>
    public TimeSpan TasklessDuration
    {
        get
        {
            if (LocationTasklessEndTime < LocationTasklessStartTime)
            {
                return DateTime.Now - _locationTasklessStartTime;
            }

            return _locationTasklessEndTime - _locationTasklessStartTime;
        }
    }

    /// <summary>
    /// 当前库位发出的调度
    /// </summary>
    [JsonIgnore]
    public ScheduleTaskWithRequest? Schedule
    {
        get { return _schedule; }
        set
        {
            if ((_schedule != null && value == null)
                || (_schedule == null && value != null)
                || (_schedule != null && value != null && _schedule.Id != value.Id))
            {
                var oldTask = _schedule;
                _schedule = value;
                _scheduleChanged = true;

                _lastInterval = DateTime.Now - _lastUpdated;
                _lastUpdated = DateTime.Now;

                if (_schedule != null)
                {
                    _schedule.OnStatusChanged += (ScheduleTask arg) =>
                    {
                        if (arg.IsCompleted)
                        {
                            _locationTasklessStartTime = DateTime.Now;
                        }

                        return Task.CompletedTask;
                    };

                    _locationTasklessEndTime = DateTime.Now;

                    if (_schedule.EventRequest != null)
                        SetPanels(_schedule.EventRequest.PayloadPanels, true);
                }

                OnScheduleChanged?.Invoke(oldTask, value);
            }
            else
            {
                _scheduleChanged = false;
            }
        }
    }

    public bool IsPartCompleted => Schedule != null && Schedule.ScheduledTaskStatus == ScheduledTaskStatus.PartCompleted;
    public bool IsUrgent => Schedule != null && Schedule.IsUrgent == 1;
    public int Percentage => HostDevice == null || !IsDrillLocation ? 0 : ((Drill)HostDevice).Percentage;

    public bool HasPanelChangedEvent => OnPanelChanged != null;

    /// <summary>
    /// 存在未完成的调度
    /// </summary>
    public bool HasNotStartedSchedule => Schedule != null && Schedule.IsNotStarted;

    /// <summary>
    /// 存在未分配agv的调度
    /// </summary>
    public bool HasNotAllocatedSchedule => Schedule != null && Schedule.IsNotStarted && string.IsNullOrEmpty(Schedule.AllocatedAgv);

    /// <summary>
    /// 当前库位发出的调度所请求的物料种类
    /// </summary>
    public MaterialKind RequestMaterialKind => Schedule?.Behavior.MaterialKind ?? MaterialKind.Unspecified;

    public DeviceKind RequestDeviceKind => HostDevice?.DeviceKind ?? DeviceKind.Unknown;
    public TransportationKind SiloKind { get; private set; }

    /// <summary>
    /// 当前库位料仓最大板料数
    /// </summary>
    public int PanelLimit => _panels.Count;

    /// <summary>
    /// 当前料仓所载料仓的板料
    /// </summary>
    public PanelList Panels => _panels;

    /// <summary>
    /// 板料快照
    /// </summary>
    public PanelListSnapshot PanelSnapshot => _panels.PanelSnapshot;

    [JsonIgnore]
    public PanelList OriginPanels => _panels;

    public int Position => _panels.Position;

    /// <summary>
    /// 空层数
    /// </summary>
    public int EmptySiloBoxCount => Panels.CountEmptySiloBoxPanels;

    /// <summary>
    /// 钻完孔的板数
    /// </summary>
    public int DrilledPanelsCount => Panels.CountDrilledPanels();

    /// <summary>
    /// 生料板数
    /// </summary>
    public int UndrilledPanelsCount => Panels.CountUndrilledPanels();

    /// <summary>
    /// 当前料仓所载料仓的首件板料数量
    /// </summary>
    public int FirstPanelsCount => Panels.CountFirstPanels();

    /// <summary>
    /// 所有料号
    /// </summary>
    public IReadOnlyList<string> ItemCodes => Panels.ItemCodes;

    /// <summary>
    /// 获取生料料号列表
    /// </summary>
    public IReadOnlyList<string> UndrilledItemCodes => Panels.UndrilledItemCodes;

    /// <summary>
    /// 获取正在钻孔的料号列表
    /// </summary>
    public IReadOnlyList<string> DrillingItemCodes => Panels.DrillingItemCodes;

    /// <summary>
    /// 获取熟料料号列表
    /// </summary>
    public IReadOnlyList<string> DrilledItemCodes => Panels.DrilledItemCodes;

    /// <summary>
    /// 获取首件料号列表
    /// </summary>
    public IReadOnlyList<string> FirstItemCodes => Panels.FirstItemCodes;

    /// <summary>
    /// 是否混料
    /// </summary>
    public bool HasMixedItems => DrilledItemCodes.Any();

    /// <summary>
    /// 库位有无料仓，有料仓用HasSilo
    /// </summary>
    public bool IsEmptyPayload => Panels.IsEmptyPayload;

    /// <summary>
    /// 库位有无料仓，有料仓用HasSilo
    /// </summary>
    public bool HasNothing => IsEmptyPayload;

    /// <summary>
    /// 库位有无料仓，有料仓用HasSilo
    /// </summary>
    public bool IsNoPayload => IsEmptyPayload;

    /// <summary>
    /// 库位有无料仓，有料仓用HasSilo
    /// </summary>
    public bool IsNoSilo => IsEmptyPayload;

    /// <summary>
    /// 库位有料仓
    /// </summary>
    public bool HasSilo => !string.IsNullOrEmpty(SiloCode) && Code != SiloCode && Panels.Any();

    public bool IsEmptySiloBox => HasSilo && Panels.IsEmptySiloBox;
    public bool IsDrillLocation => DeviceKindExtensions.IsDrill(RequestDeviceKind);
    public bool IsPanelAgvLocation => DeviceKindExtensions.IsPanelAGV(RequestDeviceKind);

    public ScheduleRequirement? Requirement => Schedule?.Requirement;

    /// <summary>
    /// 包含首件板
    /// </summary>
    public bool ContainsFirst => HasSilo && Panels.ContainsFirst();

    /// <summary>
    /// 当前库位能够上料仓
    /// </summary>
    public bool CanLoadPanelSilo => Panels.Any()
        && Schedule != null
        && Schedule.Behavior.InteractionSequence == InteractionSequence.LoadOnly
        && Schedule.Behavior.MaterialKind == Fundation.Iot.Models.MaterialKind.PanelSilo
        && Schedule.ScheduledTaskStatus == ScheduledTaskStatus.Created
        && IsEmptyPayload;

    /// <summary>
    /// 该库位上的料仓能够上板
    /// </summary>
    public bool CanLoadPanel => HasSilo
        && Panels.Any()
        && Schedule != null
        && (Schedule.Behavior.InteractionSequence == InteractionSequence.LoadOnly
        || Schedule.Behavior.InteractionSequence == InteractionSequence.LoadThenUnload
        || Schedule.Behavior.InteractionSequence == InteractionSequence.UnloadThenLoad)
        && Schedule.Behavior.MaterialKind == MaterialKind.Panel
        && (Schedule.ScheduledTaskStatus == ScheduledTaskStatus.Created || Schedule.ScheduledTaskStatus == ScheduledTaskStatus.PartCompleted)
        && !IsEmptyPayload;

    /// <summary>
    /// 当前库位能够下料仓
    /// </summary>
    public bool CanUnloadPanelSilo => HasSilo
        && Panels.Any()
        && Schedule != null
        && Schedule.Behavior.InteractionSequence == InteractionSequence.UnloadOnly
        && Schedule.Behavior.MaterialKind == MaterialKind.PanelSilo
        && (Schedule.ScheduledTaskStatus == ScheduledTaskStatus.Created)
        && !IsEmptyPayload;

    /// <summary>
    /// 该库位上的料仓能够下板
    /// </summary>
    public bool CanUnloadPanel => (HasSilo || IsDrillLocation)
        && Panels.Any()
        && Schedule != null
        && (Schedule.Behavior.InteractionSequence == InteractionSequence.UnloadOnly
        || Schedule.Behavior.InteractionSequence == InteractionSequence.LoadThenUnload
        || Schedule.Behavior.InteractionSequence == InteractionSequence.UnloadThenLoad)
        && Schedule.Behavior.MaterialKind == MaterialKind.Panel
        && Schedule.IsNotStarted
        && !IsEmptyPayload;

    /// <summary>
    /// 该库位上的料仓能够下熟料
    /// </summary>
    public bool CanUnloadDrilledPanel => CanUnloadPanel && Panels.CountDrilledPanels() > 0;

    /// <summary>
    /// 该库位上的料仓能够下生料
    /// </summary>
    public bool CanUnloadUndrilledPanel => CanUnloadPanel && Panels.CountUndrilledPanels() > 0;

    /// <summary>
    /// 该库位上的料仓能够上生料
    /// </summary>
    public bool CanLoadUndrilledPanel => CanLoadPanel && Schedule != null && !string.IsNullOrEmpty(Schedule.ItemCode);

    /// <summary>
    /// 该库位上的料仓能够上熟料
    /// </summary>
    public bool CanLoadDrilledPanel => CanLoadPanel && Panels.CountEmptySiloBoxPanels > 0;

    /// <summary>
    /// 当前库位能否匹配agv工艺路线
    /// </summary>
    /// <param name="panelAgv"></param>
    /// <returns></returns>
    public bool CanMatchAgvRoutes(PanelAgv panelAgv)
    {
        if (!panelAgv.RouteCodes.Any())
            return true;

        return panelAgv.RouteCodes.Any() && Schedule != null
            && !string.IsNullOrEmpty(Schedule.RouteCode)
            && panelAgv.RouteCodes.Contains(Schedule.RouteCode.ToLower());
    }

    /// <summary>
    /// 正在给此库位执行任务的agv
    /// </summary>
    public PanelAgv? AllocatedAgv { get; set; }

    public IReadOnlyList<string> RouteCodes
    {
        get
        {
            if (IsDrillLocation && Requirement != null)
            {
                return new List<string>() { Requirement.RouteCode.ToLower() };
            }

            return Partition == null ? new List<string>() : Partition.RouteCodes;
        }
    }

    /// <summary>
    /// 即将被拖走
    /// </summary>
    public bool IsEmptyPayloadSoon => AllocatedAgv != null && CanUnloadPanelSilo;

    /// <summary>
    /// 即将被运来一个空料仓
    /// </summary>
    public bool IsEmptySiloBoxSoon => AllocatedAgv != null && CanLoadPanelSilo;

    /// <summary>
    /// 当前库位上面为空
    /// </summary>
    public bool IsEmptyPayloadNow => Panels.IsEmptyPayload;

    /// <summary>
    /// 当前库位上面为空料仓（有料仓，但是没有板料）
    /// </summary>
    public bool IsEmptySiloBoxNow => HasSilo && Panels.IsEmptySiloBox;

    public bool ScheduleChanged => _scheduleChanged;

    /// <summary>
    /// 是否已经预约(库位上调度结束后，让此库位马上可用)
    /// </summary>
    public bool Appointed => Schedule != null && !Schedule.IsCompleted && Schedule.Appointed;

    /// <summary>
    /// 库位是否可用
    /// </summary>
    public bool Available => !Appointed
                            && Panels.Any()
                            && Schedule != null
                            && Schedule.IsNotStarted;

    public string? AppointedMessage => Schedule?.AppointedMessage;

    public int PanelCount() => Panels.Count();

    public int PanelCount(IReadOnlyList<ProductStatus> productStatuses) => Panels.PanelCount(productStatuses);

    public int PanelCount(Predicate<Panel> predicate) => Panels.PanelCount(predicate);

    /// <summary>
    /// 是否包含所给物料状态
    /// </summary>
    /// <param name="productStatuses"></param>
    /// <returns></returns>
    public bool ContainProductStatuses(IReadOnlyList<ProductStatus> productStatuses) => Panels.ContainProductStatuses(productStatuses);

    /// <summary>
    /// 不包含所给物料状态
    /// </summary>
    /// <param name="productStatuses"></param>
    /// <returns></returns>
    public bool NotContainProductStatuses(IReadOnlyList<ProductStatus> productStatuses) => Panels.ContainProductStatuses(productStatuses);

    /// <summary>
    /// 包含所给物料状态
    /// </summary>
    /// <param name="productStatus"></param>
    /// <returns></returns>
    public bool ContainProductStatus(ProductStatus productStatus) => Panels.ContainProductStatus(productStatus);

    /// <summary>
    /// 不包含所给物料状态
    /// </summary>
    /// <param name="productStatus"></param>
    /// <returns></returns>
    public bool NotContainProductStatus(ProductStatus productStatus) => Panels.ContainProductStatus(productStatus);

    /// <summary>
    /// 包含熟料板，不包括首件
    /// </summary>
    /// <returns></returns>
    public bool ContainsDrilled => Panels.ContainsDrilled();

    /// <summary>
    /// 包含生料板
    /// </summary>
    /// <returns></returns>
    public bool ContainsUndrilled => Panels.ContainsUndrilled();

    public IReadOnlyList<Panel> UndrilledPanels => Panels.UndrilledPanels;
    public IReadOnlyList<Panel> DrilledPanels => Panels.DrilledPanels;
    public IReadOnlyList<Panel> FirstPanels => Panels.FirstPanels;

    /// <summary>
    /// 对钻机需求与库位上的板料进行计算，得出一个上生料分值
    /// </summary>
    /// <param name="requirement"></param>
    /// <returns></returns>
    public int LoadPanelScore(ScheduleRequirement requirement) => CanAcceptUndrilledItems(requirement) ?
                    (PanelLimit - Panels.CountUndrilledPanels(requirement.RequireUndrilledItemCode)) * PanelLimit : 0;

    /// <summary>
    /// 对钻机需求与库位上的板料进行计算，得出一个下熟料分值
    /// </summary>
    /// <param name="requirement"></param>
    /// <returns></returns>
    public int UnloadPanelScore(ScheduleRequirement requirement) => CanAcceptDrilledItems(requirement) ?
                    Panels.CountDrilledPanels(requirement.RequireDrilledItemCode) +
                    Math.Min(requirement.RequireDrilledItemQty, Panels.CountEmptySiloBoxPanels) : 0;

    /// <summary>
    /// 对钻机需求与库位上的板料进行计算，得出一个分值
    /// </summary>
    /// <param name="requirement"></param>
    /// <returns></returns>
    public int Match(ScheduleRequirement requirement) => LoadPanelScore(requirement) + UnloadPanelScore(requirement);

    /// <summary>
    /// 当前库位能否满足钻机的调度需求
    /// </summary>
    /// <param name="requirement"></param>
    /// <returns></returns>
    public bool CanMatch(ScheduleRequirement requirement)
    {
        switch (requirement.InteractionSequence)
        {
            case InteractionSequence.LoadOnly:
                return CanAcceptUndrilledItems(requirement);

            case InteractionSequence.LoadThenUnload:
            case InteractionSequence.UnloadThenLoad:
                if (CanAcceptUndrilledItems(requirement))
                    return true;

                if (CanAcceptDrilledItems(requirement))
                    return true;

                return false;

            case InteractionSequence.UnloadOnly:
                return CanAcceptDrilledItems(requirement);

            default:
                return false;
        }
    }

    /// <summary>
    /// 是否包含所给生料料号的板料
    /// </summary>
    /// <param name="undrilledItemCodes">生料料号</param>
    /// <returns></returns>
    public bool ContainsUndrilledItemCodes(IReadOnlyList<string?> undrilledItemCodes)
    {
        return UndrilledItemCodes.Any() && UndrilledItemCodes.Intersect(undrilledItemCodes).Any();
    }

    /// <summary>
    /// 是否包含所给生料料号的板料
    /// </summary>
    /// <param name="undrilledItemCode">生料料号</param>
    /// <returns></returns>
    public bool ContainsUndrilledItemCode(string undrilledItemCode)
    {
        return UndrilledItemCodes.Any() && UndrilledItemCodes.Contains(undrilledItemCode);
    }

    /// <summary>
    /// 是否包含所给料号的板料
    /// </summary>
    /// <param name="itemCode">料号</param>
    /// <returns></returns>
    public bool ContainsItemCode(string itemCode)
    {
        return ItemCodes.Any() && ItemCodes.Contains(itemCode);
    }

    /// <summary>
    /// 是否包含所给熟料料号的板料
    /// </summary>
    /// <param name="drilledItemCodes">熟料料号</param>
    /// <returns></returns>
    public bool ContainsDrilledItemCodes(IReadOnlyList<string?> drilledItemCodes)
    {
        return drilledItemCodes.Any() && DrilledItemCodes.Intersect(drilledItemCodes).Any();
    }

    /// <summary>
    /// 是否包含所给熟料料号的板料
    /// </summary>
    /// <param name="drilledItemCode">熟料料号</param>
    /// <returns></returns>
    public bool ContainsDrilledItemCode(string drilledItemCode)
    {
        return DrilledItemCodes.Any() && DrilledItemCodes.Contains(drilledItemCode);
    }

    /// <summary>
    /// 该库位能接受钻机需求上熟料
    /// </summary>
    /// <param name="requirement"></param>
    /// <returns></returns>
    public bool CanAcceptDrilledItems(ScheduleRequirement requirement) => HasSilo
                                    && !string.IsNullOrWhiteSpace(requirement.RequireDrilledItemCode)
                                    && Panels.CountEmptySiloBoxPanels >= requirement.RequireDrilledItemQty
                                    && (!Panels.ContainsOtherDrilledItemCodes(requirement.RequireDrilledItemCode) || IsEmptySiloBox);

    /// <summary>
    /// 该库位能接受钻机需求上生料
    /// </summary>
    /// <param name="requirement"></param>
    /// <returns></returns>
    public bool CanAcceptUndrilledItems(ScheduleRequirement requirement) => HasSilo
                    && !string.IsNullOrWhiteSpace(requirement.RequireUndrilledItemCode)
                    && Panels.ContainsUndrilledItemCode(requirement.RequireUndrilledItemCode);

    /// <summary>
    /// 该库位能接受钻机需求上生料
    /// </summary>
    /// <param name="undrilledItemCode">生料料号</param>
    /// <returns></returns>
    public bool CanAcceptUndrilledItems(string undrilledItemCode) => HasSilo
                    && !string.IsNullOrWhiteSpace(undrilledItemCode)
                    && Panels.ContainsUndrilledItemCode(undrilledItemCode);

    /// <summary>
    /// 设置库位板料，参数<paramref name="forceRaiseChangedEvent"/>是否强制发生板料变化事件
    /// </summary>
    /// <param name="panelList">板料</param>
    /// <param name="forceRaiseChangedEvent">强制调用板料变化事件</param>
    public void SetPanels(PanelList panelList, bool forceRaiseChangedEvent = false)
    {
        if (Interlocked.CompareExchange(ref _doingWork, 1, 0) == 0)
        {
            var snapshot = _panels.PanelSnapshot;
            _panels.Clear();
            _panels.AddRange(panelList);
            SiloKind = _panels.CalculateTransportationKind();

            var snapshot2 = _panels.PanelSnapshot;
            Interlocked.Exchange(ref _doingWork, 0);

            if ((!snapshot.Equals(snapshot2) || forceRaiseChangedEvent)
                && this.OnPanelChanged != null)
            {
                this.OnPanelChanged.Invoke(this);
            }
        }
    }

    /// <summary>
    /// 设置库位为空位置
    /// </summary>
    public void SetNoPayload()
    {
        _panels.SetNoPayload();
    }

    /// <summary>
    /// 初始化该库位为空位置，并且层数为<paramref name="layerLimit"/>
    /// </summary>
    /// <param name="layerLimit"></param>
    public void InitializeWith(int layerLimit)
    {
        _panels = Panel.NoSilo.PanelForSingleSpindle(Index, 0, layerLimit);
    }

    /// <summary>
    /// 初始化该库位为空位置，并且层数为<paramref name="layerLimit"/>
    /// </summary>
    /// <param name="layerLimit"></param>
    /// <param name="siloCode"></param>
    public void InitializeWith(int layerLimit, string siloCode)
    {
        _panels = Panel.HasSilo.NoPanelForSingleSpindle(siloCode, Index, 0, layerLimit);
    }

    public bool Equals(Location? other)
    {
        return this.Code == other?.Code;
    }

    public override int GetHashCode() => this.Code.GetHashCode();

    public override bool Equals(object? obj) => obj != null && obj is Location other && this.Equals(other);

    public bool Appoint(string message = "")
    {
        if (Schedule != null && Schedule.IsNotStarted)
        {
            Schedule.Appointed = true;
            Schedule.AppointedMessage = message;
            return true;
        }

        return false;
    }

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

    /// <summary>
    /// 主动触发库位的OnPanelChanged事件
    /// </summary>
    public void PanelChange()
    {
        this.OnPanelChanged?.Invoke(this);
    }
}
