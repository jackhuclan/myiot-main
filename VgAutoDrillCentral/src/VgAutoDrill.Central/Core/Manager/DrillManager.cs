using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Model.CentralModels;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Schedule;
using VgAutoDrill.Central.Core.Schedule.Summary;

namespace VgAutoDrill.Central.Core.Manager;

public class DrillManager : IDrillManager
{
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly IDeviceManager _deviceManager;
    private readonly ILocationManager _locationManager;
    private readonly ILogger<DrillManager> _logger;
    private readonly IWorkOrderTaskAdapter _workOrderTaskAdapter;
    private readonly IDeviceAdapter _deviceAdapter;
    private List<WorkOrderTask> _pendingWorkOrders = new();

    public DrillManager(IServiceProvider serviceProvider)
    {
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
        _deviceManager = serviceProvider.GetRequiredService<IDeviceManager>();
        _locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        _workOrderTaskAdapter = serviceProvider.GetRequiredService<IWorkOrderTaskAdapter>();
        _deviceAdapter = serviceProvider.GetRequiredService<IDeviceAdapter>();

        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<DrillManager>();
    }

    public IReadOnlyList<Drill> Drills => _deviceManager.Drills;
    public IReadOnlyList<Location> Locations => Drills.Where(x => x.Location != null).Select(x => x.Location!).ToList();

    public IReadOnlyList<Drill> RouteDrills(IReadOnlyList<string> routeCodes) => Drills.Where(x => x.RouteCodes.Intersect(routeCodes).Any()).ToList();

    public IReadOnlyList<Location> OnlineLocations => _locationManager.OnlineLocations.Where(x => x.IsDrillLocation).ToList().AsReadOnly()!;
    public IReadOnlyList<Location> AvailableLocations => _locationManager.AvailableLocations.Where(x => x.IsDrillLocation).ToList().AsReadOnly()!;

    /// <summary>
    /// 待生产工单数
    /// </summary>
    public IReadOnlyList<WorkOrderTask> PendingWorkOrders => _pendingWorkOrders.ToList();

    /// <summary>
    /// 分区库位
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <returns></returns>
    public IReadOnlyList<Location> PartitionLocations(string partitionCode)
        => string.IsNullOrEmpty(partitionCode) ? OnlineLocations : OnlineLocations.Where(x => x.PartitionCode == partitionCode)
                                                            .ToList()
                                                            .AsReadOnly();

    /// <summary>
    /// 可用分区库位
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <returns></returns>
    public IReadOnlyList<Location> PartitionAvailableLocations(string partitionCode)
        => string.IsNullOrEmpty(partitionCode) ? AvailableLocations : AvailableLocations.Where(x => x.PartitionCode == partitionCode)
                                                            .ToList()
                                                            .AsReadOnly();

    /// <summary>
    /// 可用工艺路线库位
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <returns></returns>
    public IReadOnlyList<Location> RouteAvailableLocations(IReadOnlyList<string> routeCodes)
        => AvailableLocations.Where(x => x.RouteCodes.Intersect(routeCodes).Any())
                                                            .OrderByDescending(x => x.Percentage)
                                                            .ToList()
                                                            .AsReadOnly();

    /// <summary>
    /// 可用的库位，发出了调度任务，没有被预约的库位
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <param name="routeCodes">工艺路线</param>
    /// <returns></returns>
    public IReadOnlyList<Location> PartitionAvailableLocations(string partitionCode, IReadOnlyList<string> routeCodes)
    {
        Predicate<Location> routeCodeMatched = x => !routeCodes.Any() || (routeCodes.Any() && x.RouteCodes.Intersect(routeCodes).Any());
        Predicate<Location> partitionCodeMatched = x => string.IsNullOrEmpty(partitionCode) || string.Compare(x.PartitionCode, partitionCode, StringComparison.OrdinalIgnoreCase) == 0;

        return AvailableLocations
            .Where(x => partitionCodeMatched(x) && routeCodeMatched(x))
            .ToList().AsReadOnly();
    }

    /// <summary>
    /// 存在未完成的调度
    /// </summary>
    public IReadOnlyList<Location> NotStartedLocations => AvailableLocations.Where(x => x != null && x.HasNotStartedSchedule).ToList().AsReadOnly()!;

    public IReadOnlyList<Location> PartitionNotStartedLocations(string partitionCode) => PartitionAvailableLocations(partitionCode)
        .Where(x => x != null && x.HasNotStartedSchedule).ToList().AsReadOnly()!;

    public IReadOnlyList<Location> RouteNotStartedLocations(IReadOnlyList<string> routeCodes) => RouteAvailableLocations(routeCodes)
        .Where(x => x != null && x.HasNotStartedSchedule).ToList().AsReadOnly()!;

    public IReadOnlyList<Location> PartitionNotStartedLocations(string partitionCode, IReadOnlyList<string> routeCodes) => RouteAvailableLocations(routeCodes)
        .Where(x => x != null && x.HasNotStartedSchedule).ToList().AsReadOnly()!;

    /// <summary>
    /// 存在未分配agv的调度
    /// </summary>
    public IReadOnlyList<Location> NotAllocatedLocations => AvailableLocations.Where(x => x != null && x.HasNotAllocatedSchedule).ToList().AsReadOnly()!;

    public IReadOnlyList<Location> PartCompletedLocations => AvailableLocations.Where(s => s.Schedule != null
            && s.Schedule.ScheduledTaskStatus.HasValue
            && s.Schedule.ScheduledTaskStatus == Fundation.Iot.Schedule.ScheduledTaskStatus.PartCompleted)
            .OrderByDescending(s => s.Schedule!.Percentage)
            .ThenBy(s => s.Schedule!.Id)
            .ToList()
            .AsReadOnly();

    public IReadOnlyList<DrillScheduleTask> NotStartedSchedules => _scheduleTaskManager.NotStartedDrillSchedules;

    #region 钻机上的板料汇总

    /// <summary>
    /// 钻机上的板料汇总
    /// </summary>
    public IReadOnlyList<ItemSummary> PayloadPanelSummaries => Locations
        .SelectMany(x => x.Panels.Where(x => !string.IsNullOrWhiteSpace(x.ItemCode)))
        .GroupBy(x => x.ItemCode)
        .Select(x => new ItemSummary()
        {
            ItemCode = x.Key,
            ItemCount = x.Count(),
        }).ToList().AsReadOnly();

    /// <summary>
    /// 所给可用分区上的钻机的板料汇总
    /// </summary>
    /// <param name="partitionCode"></param>
    /// <returns></returns>
    public IReadOnlyList<ItemSummary> PartitionAvailablePayloadPanelSummaries(string partitionCode) => PartitionAvailableLocations(partitionCode)
        .SelectMany(x => x.Panels.Where(x => !string.IsNullOrWhiteSpace(x.ItemCode)))
        .GroupBy(x => x.ItemCode)
        .Select(x => new ItemSummary()
        {
            ItemCode = x.Key,
            ItemCount = x.Count(),
        }).ToList().AsReadOnly();

    /// <summary>
    /// 所给分区上的钻机的板料汇总
    /// </summary>
    /// <param name="partitionCode"></param>
    /// <returns></returns>
    public IReadOnlyList<ItemSummary> PartitionPayloadPanelSummaries(string partitionCode) => PartitionLocations(partitionCode)
        .SelectMany(x => x.Panels.Where(x => !string.IsNullOrWhiteSpace(x.ItemCode)))
        .GroupBy(x => x.ItemCode)
        .Select(x => new ItemSummary()
        {
            ItemCode = x.Key,
            ItemCount = x.Count(),
        }).ToList().AsReadOnly();

    #endregion 钻机上的板料汇总

    #region 钻机呼叫的需求

    /// <summary>
    /// 所有钻机的所有需求
    /// </summary>
    public IReadOnlyList<ScheduleRequirement> Requirements => AvailableLocations
        .Where(x => x.Requirement != null)
        .Select(x => x.Requirement)
        .ToList()
        .AsReadOnly()!;

    public IReadOnlyList<ScheduleRequirement> RouteRequirements(IReadOnlyList<string> routeCodes)
        => RouteAvailableLocations(routeCodes)
        .Where(x => x.Requirement != null)
        .Select(x => x.Requirement)
        .ToList()
        .AsReadOnly()!;

    public IReadOnlyList<ScheduleRequirement> PartitionRequirements(string partitionCode)
        => PartitionAvailableLocations(partitionCode)
        .Where(x => x.Requirement != null)
        .Select(x => x.Requirement)
        .ToList()
        .AsReadOnly()!;

    public IReadOnlyList<ScheduleRequirement> PartitionRouteRequirements(string partitionCode, IReadOnlyList<string> routeCodes)
        => PartitionAvailableLocations(partitionCode, routeCodes)
        .Where(x => x.Requirement != null)
        .Select(x => x.Requirement)
        .ToList()
        .AsReadOnly()!;

    public IReadOnlyList<ScheduleRequirement> OrderedRequirements(IReadOnlyList<string> routeCodes) => Requirements
         .Where(x => x.OriginalSchedule != null && !x.OriginalSchedule.Appointed
            && x.OriginalSchedule.IsNotStarted
            && !string.IsNullOrEmpty(x.OriginalSchedule.RouteCode)
            && routeCodes.Contains(x.OriginalSchedule.RouteCode.ToLower()))
        .OrderBy(x => x.OriginalSchedule.ScheduledTaskStatus == Fundation.Iot.Schedule.ScheduledTaskStatus.PartCompleted ? 0 : 1)
        .OrderByDescending(x => x.OriginalSchedule.IsUrgent.HasValue ? x.OriginalSchedule.IsUrgent.Value : 0)
        //.ThenByDescending(x => x.OriginalSchedule.Percentage)
        .ThenBy(x => x.OriginalSchedule.Id)
        .Select(x => x)
        .ToList();

    #endregion 钻机呼叫的需求

    #region 对钻机需要的生料板料进行summary

    /// <summary>
    /// 所有钻机的生料需求汇总
    /// </summary>
    public IReadOnlyList<UndrilledItemSummary> RequiredUndrilledItemSummaries => Requirements
        .Where(x => !string.IsNullOrWhiteSpace(x.RequireUndrilledItemCode))
        .GroupBy(x => x.RequireUndrilledItemCode)
        .Select(x => new UndrilledItemSummary()
        {
            ItemCode = x.Key!,
            ItemCount = x.Sum(i => i.RequireUndrilledItemQty),
            RouteCodes = x.Select(i => i.OriginalSchedule.RouteCode?.ToLower() ?? string.Empty).Distinct().OrderBy(x => x).ToList(),
            ScheduleIds = x.Select(i => i.OriginalSchedule.Id).Distinct().OrderBy(x => x).ToList(),
            DeviceIds = x.Select(i => i.OriginalSchedule.CallerDeviceId ?? string.Empty).Distinct().OrderBy(x => x).ToList(),
        }).ToList().AsReadOnly();

    /// <summary>
    /// 工艺路线上所有钻机的生料需求汇总
    /// </summary>
    /// <param name="routeCodes"></param>
    /// <returns></returns>
    public IReadOnlyList<UndrilledItemSummary> RouteRequiredUndrilledItemSummaries(IReadOnlyList<string> routeCodes) => RouteRequirements(routeCodes)
        .Where(x => !string.IsNullOrWhiteSpace(x.RequireUndrilledItemCode))
        .GroupBy(x => x.RequireUndrilledItemCode)
        .Select(x => new UndrilledItemSummary()
        {
            ItemCode = x.Key!,
            ItemCount = x.Sum(i => i.RequireUndrilledItemQty),
            RouteCodes = x.Select(i => i.OriginalSchedule.RouteCode?.ToLower() ?? string.Empty).Distinct().OrderBy(x => x).ToList(),
            ScheduleIds = x.Select(i => i.OriginalSchedule.Id).Distinct().OrderBy(x => x).ToList(),
            DeviceIds = x.Select(i => i.OriginalSchedule.CallerDeviceId ?? string.Empty).Distinct().OrderBy(x => x).ToList(),
        }).ToList().AsReadOnly();

    public IReadOnlyList<UndrilledItemSummary> PartitionRequiredUndrilledItemSummaries(string partitionCode)
        => PartitionRequirements(partitionCode)
        .Where(x => !string.IsNullOrWhiteSpace(x.RequireUndrilledItemCode))
        .GroupBy(x => x.RequireUndrilledItemCode)
        .Select(x => new UndrilledItemSummary()
        {
            ItemCode = x.Key!,
            ItemCount = x.Sum(i => i.RequireUndrilledItemQty),
            RouteCodes = x.Select(i => i.OriginalSchedule.RouteCode?.ToLower() ?? string.Empty).Distinct().OrderBy(x => x).ToList(),
            ScheduleIds = x.Select(i => i.OriginalSchedule.Id).Distinct().OrderBy(x => x).ToList(),
            DeviceIds = x.Select(i => i.OriginalSchedule.CallerDeviceId ?? string.Empty).Distinct().OrderBy(x => x).ToList(),
        }).ToList().AsReadOnly();

    public IReadOnlyList<UndrilledItemSummary> PartitionRouteRequiredUndrilledItemSummaries(string partitionCode, IReadOnlyList<string> routeCodes)
        => PartitionRouteRequirements(partitionCode, routeCodes)
        .Where(x => !string.IsNullOrWhiteSpace(x.RequireUndrilledItemCode))
        .GroupBy(x => x.RequireUndrilledItemCode)
        .Select(x => new UndrilledItemSummary()
        {
            ItemCode = x.Key!,
            ItemCount = x.Sum(i => i.RequireUndrilledItemQty),
            RouteCodes = x.Select(i => i.OriginalSchedule.RouteCode?.ToLower() ?? string.Empty).Distinct().OrderBy(x => x).ToList(),
            ScheduleIds = x.Select(i => i.OriginalSchedule.Id).Distinct().OrderBy(x => x).ToList(),
            DeviceIds = x.Select(i => i.OriginalSchedule.CallerDeviceId ?? string.Empty).Distinct().OrderBy(x => x).ToList(),
        }).ToList().AsReadOnly();

    #endregion 对钻机需要的生料板料进行summary

    #region 对钻机需要的熟料板料进行summary

    /// <summary>
    /// 所有钻机的要下的熟料汇总
    /// </summary>
    public IReadOnlyList<DrilledItemSummary> RequiredDrilledItemSummaries => Requirements
        .Where(x => !string.IsNullOrWhiteSpace(x.RequireDrilledItemCode))
        .GroupBy(x => x.RequireDrilledItemCode!)
        .Select(x => new DrilledItemSummary()
        {
            ItemCode = x.Key,
            ItemCount = x.Sum(i => i.RequireDrilledItemQty),
            RouteCodes = x.Select(i => i.OriginalSchedule.RouteCode?.ToLower() ?? string.Empty).Distinct().OrderBy(x => x).ToList(),
            ScheduleIds = x.Select(i => i.OriginalSchedule.Id).Distinct().OrderBy(x => x).ToList(),
            DeviceIds = x.Select(i => i.OriginalSchedule.CallerDeviceId ?? string.Empty).Distinct().OrderBy(x => x).ToList(),
        }).ToList().AsReadOnly();

    /// <summary>
    /// 工艺路线上所有钻机要下的熟料汇总
    /// </summary>
    /// <param name="routeCodes"></param>
    /// <returns></returns>
    public IReadOnlyList<DrilledItemSummary> RouteRequiredDrilledItemSummaries(IReadOnlyList<string> routeCodes) => RouteRequirements(routeCodes)
        .Where(x => !string.IsNullOrWhiteSpace(x.RequireDrilledItemCode))
        .GroupBy(x => x.RequireDrilledItemCode!)
        .Select(x => new DrilledItemSummary()
        {
            ItemCode = x.Key,
            ItemCount = x.Sum(i => i.RequireDrilledItemQty),
            RouteCodes = x.Select(i => i.OriginalSchedule.RouteCode?.ToLower() ?? string.Empty).Distinct().OrderBy(x => x).ToList(),
            ScheduleIds = x.Select(i => i.OriginalSchedule.Id).Distinct().OrderBy(x => x).ToList(),
            DeviceIds = x.Select(i => i.OriginalSchedule.CallerDeviceId ?? string.Empty).Distinct().OrderBy(x => x).ToList(),
        }).ToList().AsReadOnly();

    public IReadOnlyList<DrilledItemSummary> PartitionRequiredDrilledItemSummaries(string partitionCode)
        => PartitionRequirements(partitionCode)
        .Where(x => !string.IsNullOrWhiteSpace(x.RequireDrilledItemCode))
        .GroupBy(x => x.RequireDrilledItemCode)
        .Select(x => new DrilledItemSummary()
        {
            ItemCode = x.Key!,
            ItemCount = x.Sum(i => i.RequireDrilledItemQty),
            RouteCodes = x.Select(i => i.OriginalSchedule.RouteCode?.ToLower() ?? string.Empty).Distinct().OrderBy(x => x).ToList(),
            ScheduleIds = x.Select(i => i.OriginalSchedule.Id).Distinct().OrderBy(x => x).ToList(),
            DeviceIds = x.Select(i => i.OriginalSchedule.CallerDeviceId ?? string.Empty).Distinct().OrderBy(x => x).ToList(),
        }).ToList().AsReadOnly();

    public IReadOnlyList<DrilledItemSummary> PartitionRouteRequiredDrilledItemSummaries(string partitionCode, IReadOnlyList<string> routeCodes)
        => PartitionRouteRequirements(partitionCode, routeCodes)
        .Where(x => !string.IsNullOrWhiteSpace(x.RequireDrilledItemCode))
        .GroupBy(x => x.RequireDrilledItemCode)
        .Select(x => new DrilledItemSummary()
        {
            ItemCode = x.Key!,
            ItemCount = x.Sum(i => i.RequireDrilledItemQty),
            RouteCodes = x.Select(i => i.OriginalSchedule.RouteCode?.ToLower() ?? string.Empty).Distinct().OrderBy(x => x).ToList(),
            ScheduleIds = x.Select(i => i.OriginalSchedule.Id).Distinct().OrderBy(x => x).ToList(),
            DeviceIds = x.Select(i => i.OriginalSchedule.CallerDeviceId ?? string.Empty).Distinct().OrderBy(x => x).ToList(),
        }).ToList().AsReadOnly();

    #endregion 对钻机需要的熟料板料进行summary

    #region 对钻机需要的首件板料进行summary

    /// <summary>
    /// 所有钻机的要下的首件汇总
    /// </summary>
    public IReadOnlyList<FirstItemSummary> FirstDrilledItemSummaries => Requirements
        .Where(x => !string.IsNullOrWhiteSpace(x.RequireDrilledItemCode) && x.IsFirst)
        .GroupBy(x => x.RequireDrilledItemCode!)
        .Select(x => new FirstItemSummary()
        {
            ItemCode = x.Key,
            ItemCount = x.Sum(i => i.RequireDrilledItemQty),
            RouteCodes = x.Select(i => i.OriginalSchedule.RouteCode?.ToLower() ?? string.Empty).Distinct().OrderBy(x => x).ToList(),
            ScheduleIds = x.Select(i => i.OriginalSchedule.Id).Distinct().OrderBy(x => x).ToList(),
            DeviceIds = x.Select(i => i.OriginalSchedule.CallerDeviceId ?? string.Empty).Distinct().OrderBy(x => x).ToList(),
        }).ToList().AsReadOnly();

    /// <summary>
    /// 工艺路线上所有钻机的要下的首件汇总
    /// </summary>
    public IReadOnlyList<FirstItemSummary> RouteFirstDrilledItemSummaries(IReadOnlyList<string> routeCodes) => RouteRequirements(routeCodes)
        .Where(x => !string.IsNullOrWhiteSpace(x.RequireDrilledItemCode) && x.IsFirst)
        .GroupBy(x => x.RequireDrilledItemCode!)
        .Select(x => new FirstItemSummary()
        {
            ItemCode = x.Key,
            ItemCount = x.Sum(i => i.RequireDrilledItemQty),
            RouteCodes = x.Select(i => i.OriginalSchedule.RouteCode?.ToLower() ?? string.Empty).Distinct().OrderBy(x => x).ToList(),
            ScheduleIds = x.Select(i => i.OriginalSchedule.Id).Distinct().OrderBy(x => x).ToList(),
            DeviceIds = x.Select(i => i.OriginalSchedule.CallerDeviceId ?? string.Empty).Distinct().OrderBy(x => x).ToList(),
        }).ToList().AsReadOnly();

    #endregion 对钻机需要的首件板料进行summary

    #region 对料号进行汇总

    /// <summary>
    /// 所有钻机的需求的生料料号
    /// </summary>
    public IReadOnlyList<string> RequiredUndrilledItemCodes => Requirements
        .Where(x => !string.IsNullOrEmpty(x.RequireUndrilledItemCode))
        .Select(s => s.RequireUndrilledItemCode)
        .Distinct()
        .ToList()
        .AsReadOnly()!;

    public IReadOnlyList<string> RouteRequiredUndrilledItemCodes(IReadOnlyList<string> routeCodes) => RouteRequirements(routeCodes)
        .Where(x => !string.IsNullOrEmpty(x.RequireUndrilledItemCode))
        .Select(s => s.RequireUndrilledItemCode)
        .Distinct()
        .ToList()
        .AsReadOnly()!;

    public IReadOnlyList<string> PartitionRequiredUndrilledItemCodes(string partitionCode)
        => PartitionRequirements(partitionCode)
        .Where(x => !string.IsNullOrEmpty(x.RequireUndrilledItemCode))
        .Select(s => s.RequireUndrilledItemCode)
        .Distinct()
        .ToList()
        .AsReadOnly()!;

    public IReadOnlyList<string> PartitionRouteRequiredUndrilledItemCodes(string partitionCode, IReadOnlyList<string> routeCodes)
        => PartitionRouteRequirements(partitionCode, routeCodes)
        .Where(x => !string.IsNullOrEmpty(x.RequireUndrilledItemCode))
        .Select(s => s.RequireUndrilledItemCode)
        .Distinct()
        .ToList()
        .AsReadOnly()!;

    /// <summary>
    /// 所有钻机的需求的熟料料号
    /// </summary>
    public IReadOnlyList<string> RequiredDrilledItemCodes
        => Requirements.Where(x => !string.IsNullOrWhiteSpace(x.RequireDrilledItemCode))
        .Select(x => x.RequireDrilledItemCode!)
        .Distinct()
        .ToList()
        .AsReadOnly();

    public IReadOnlyList<string> RouteRequiredDrilledItemCodes(IReadOnlyList<string> routeCodes)
        => RouteRequirements(routeCodes)
        .Where(x => !string.IsNullOrWhiteSpace(x.RequireDrilledItemCode))
        .Select(x => x.RequireDrilledItemCode!)
        .Distinct()
        .ToList()
        .AsReadOnly();

    public IReadOnlyList<string> PartitionRequiredDrilledItemCodes(string partitionCode)
        => PartitionRequirements(partitionCode)
        .Where(x => !string.IsNullOrWhiteSpace(x.RequireDrilledItemCode))
        .Select(x => x.RequireDrilledItemCode!)
        .Distinct()
        .ToList()
        .AsReadOnly();

    public IReadOnlyList<string> PartitionRouteRequiredDrilledItemCodes(string partitionCode, IReadOnlyList<string> routeCodes)
        => PartitionRouteRequirements(partitionCode, routeCodes)
        .Where(x => !string.IsNullOrWhiteSpace(x.RequireDrilledItemCode))
        .Select(x => x.RequireDrilledItemCode!)
        .Distinct()
        .ToList()
        .AsReadOnly();

    #endregion 对料号进行汇总

    /// <summary>
    /// 所有钻机上的板料汇总
    /// </summary>
    /// <returns></returns>
    public int PanelCount()
    {
        return _deviceManager.Drills.Select(x => x.Location)
             .Sum(s => s == null ? 0 : s.PanelCount());
    }

    /// <summary>
    /// 某个钻机上的板料汇总
    /// </summary>
    /// <param name="deviceId">钻机的设备id</param>
    /// <returns></returns>
    public int PanelCount(string deviceId)
    {
        return _deviceManager.Drills.Where(x => x.DeviceId == deviceId)
             .Select(x => x.Location)
             .Sum(s => s == null ? 0 : s.PanelCount());
    }

    /// <summary>
    /// 所有钻机上的某些状态的板料汇总
    /// </summary>
    /// <param name="productStatuses">物料状态</param>
    /// <returns></returns>
    public int PanelCount(IReadOnlyList<Fundation.Iot.Models.ProductStatus> productStatuses)
    {
        return _deviceManager.Drills.Select(x => x.Location)
             .Sum(s => s == null ? 0 : s.PanelCount(productStatuses));
    }

    /// <summary>
    /// 某个钻机上的某些状态的板料汇总
    /// </summary>
    /// <param name="deviceId">钻机的设备id</param>
    /// <param name="productStatuses">物料状态</param>
    /// <returns></returns>
    public int PanelCount(string deviceId, IReadOnlyList<Fundation.Iot.Models.ProductStatus> productStatuses)
    {
        return _deviceManager.Drills.Where(x => x.DeviceId == deviceId)
            .Select(x => x.Location)
            .Sum(s => s == null ? 0 : s.PanelCount(productStatuses));
    }

    /// <summary>
    /// 统计包含所给物料状态的料仓数量
    /// </summary>
    /// <param name="productStatuses">所给物料状态</param>
    /// <returns></returns>
    public int SiloCount(IReadOnlyList<Fundation.Iot.Models.ProductStatus> productStatuses)
    {
        return _deviceManager.Drills.Select(x => x.Location).Count(x => x != null && x.ContainProductStatuses(productStatuses));
    }

    /// <summary>
    /// 统计包含所给物料状态的钻机设备
    /// </summary>
    /// <param name="productStatuses">所给物料状态</param>
    /// <returns></returns>
    public int Count(IReadOnlyList<Fundation.Iot.Models.ProductStatus> productStatuses)
    {
        return _deviceManager.Drills.Select(x => x.Location).Count(x => x != null && x.ContainProductStatuses(productStatuses));
    }

    public bool TryFindArbitraryDrillRequirementByAgv(PanelAgv agv, out ScheduleRequirement? drillRequirement)
    {
        _logger.LogDebug($"DrillScheduleHandler:agv={agv.DeviceId} agv.RouteCodes={string.Join(",", agv.RouteCodes)} has silo, TryFindArbitraryDrillRequirementByAgv...");
        var requirements = OrderedRequirements(agv.RouteCodes);
        foreach (var scheduleRequirement in requirements)
        {
            scheduleRequirement.Score = agv.Location.Match(scheduleRequirement);
        }

        requirements.ToList().Sort();
        drillRequirement = requirements.FirstOrDefault();
        _logger.LogDebug($"DrillScheduleHandler:agv={agv.DeviceId} agv.RouteCodes={string.Join(",", agv.RouteCodes)} has silo, TryFindArbitraryDrillRequirementByAgv, requirements={requirements.Any()}");
        return drillRequirement != null;
    }

    public async Task Refresh()
    {
        if (Drills == null || Drills.Count == 0)
        {
            return;
        }

        var drillIds = Drills.Where(p => !string.IsNullOrEmpty(p.DeviceId)).Select(p => p.DeviceId.ToLower()).ToList();

        var data = await _workOrderTaskAdapter.GetPendingTask(drillIds);
        if (data != null && data.Count > 0)
        {
            _pendingWorkOrders = data;

            foreach (var drill in Drills)
            {
                drill.PendingWorkOrders = _pendingWorkOrders.Where(p => !string.IsNullOrEmpty(p.WorkStationCode)
                && p.WorkStationCode.ToLower() == drill.DeviceId.ToLower()).ToList();
            }
        }

        List<ReCordDrillRateFactorDto> addList = new List<ReCordDrillRateFactorDto>();
        foreach (var drill in Drills)
        {
            if (drill.PendingWorkOrders == null || drill.PendingWorkOrders.Count == 0)
            {
                DateTime time = DateTime.Now;
                DateTime oldTime = drill.WithoutPendingWorkOrdersTime == null ? time : drill.WithoutPendingWorkOrdersTime.ToDate();
                await _deviceAdapter.ReCordDrillRateFactor(drill.DeviceId, DrillRateFactorReason.WithoutPendingWorkOrders, oldTime, time);

                drill.WithoutPendingWorkOrdersTime = time;
            }
            else
            {
                drill.WithoutPendingWorkOrdersTime = null;
            }

            if (drill.DrillToolLifeExpiredStartTime != null)
            {
                _logger.LogDebug($"ReCordDrillRateFactor {drill.DeviceId} DrillToolLifeExpiredStartTime {drill.DrillToolLifeExpiredStartTime},DrillToolLifeExpiredEndTime {drill.DrillToolLifeExpiredEndTime} ");
                addList.Add(new ReCordDrillRateFactorDto
                {
                    DeviceId = drill.DeviceId,
                    Reason = DrillRateFactorReason.DrillToolLifeExpored,
                    StartTime = drill.DrillToolLifeExpiredStartTime,
                    EndTime = drill.DrillToolLifeExpiredEndTime,
                    LocationCode = drill.GetLocationCode(),
                    DrillShiftsStartTime = drill.DrillShiftsStartTime,
                });
            }

            if (drill.DrillCollectCleanBegin != null)
            {
                addList.Add(new ReCordDrillRateFactorDto
                {
                    DeviceId = drill.DeviceId,
                    Reason = DrillRateFactorReason.DrillCollectClear,
                    StartTime = drill.DrillCollectCleanBegin,
                    EndTime = drill.DrillCollectCleanEnd,
                    LocationCode = drill.GetLocationCode(),
                    DrillShiftsStartTime = drill.DrillShiftsStartTime,
                });
            }

            if (drill.DrillChangeNoBoardTime != null)
            {
                addList.Add(new ReCordDrillRateFactorDto
                {
                    DeviceId = drill.DeviceId,
                    Reason = DrillRateFactorReason.WithoutPendingPanel,
                    StartTime = drill.DrillChangeNoBoardTime,
                    EndTime = drill.DrillChangeExistBoardTime,
                    LocationCode = drill.GetLocationCode(),
                    DrillShiftsStartTime = drill.DrillShiftsStartTime,
                });
            }

            if (drill.BufferClinkerChangeExistTime != null)
            {
                addList.Add(new ReCordDrillRateFactorDto
                {
                    DeviceId = drill.DeviceId,
                    Reason = DrillRateFactorReason.BufferClinkerChangeExist,
                    StartTime = drill.BufferClinkerChangeExistTime,
                    EndTime = drill.BufferClinkerChangeNoBoardTime,
                    LocationCode = drill.GetLocationCode(),
                    DrillShiftsStartTime = drill.DrillShiftsStartTime,
                });
            }

            if (drill.BufferRawChangeNoBoardTime != null)
            {
                addList.Add(new ReCordDrillRateFactorDto
                {
                    DeviceId = drill.DeviceId,
                    Reason = DrillRateFactorReason.BufferRawChangeNoBoard,
                    StartTime = drill.BufferRawChangeNoBoardTime,
                    EndTime = drill.BufferRawChangeExistBoardTime,
                    LocationCode = drill.GetLocationCode(),
                    DrillShiftsStartTime = drill.DrillShiftsStartTime,
                });
            }

            if (drill.DrillChangeExistBoardTime != null)
            {
                addList.Add(new ReCordDrillRateFactorDto
                {
                    DeviceId = drill.DeviceId,
                    Reason = DrillRateFactorReason.DrillRawExistToRun,
                    StartTime = drill.DrillChangeExistBoardTime,
                    EndTime = drill.DrillRunStartTime,
                    LocationCode = drill.GetLocationCode(),
                    DrillShiftsStartTime = drill.DrillShiftsStartTime,
                });
            }

            if (drill.DrillRunStartTime != null)
            {
                _logger.LogDebug($"ReCordDrillRateFactor {drill.DeviceId} DrillRunStartTime {drill.DrillRunStartTime},DrillRunEndTime {drill.DrillRunEndTime} ");
                addList.Add(new ReCordDrillRateFactorDto
                {
                    DeviceId = drill.DeviceId,
                    Reason = DrillRateFactorReason.DrillRun,
                    StartTime = drill.DrillRunStartTime,
                    EndTime = drill.DrillRunEndTime,
                    LocationCode = drill.GetLocationCode(),
                    DrillShiftsStartTime = drill.DrillShiftsStartTime,
                });
            }

            if (drill.DrillRunEndTime != null)
            {
                addList.Add(new ReCordDrillRateFactorDto
                {
                    DeviceId = drill.DeviceId,
                    Reason = DrillRateFactorReason.DrillEndToStart,
                    StartTime = drill.DrillRunEndTime,
                    EndTime = drill.DrillRunStartTime,
                    LocationCode = drill.GetLocationCode(),
                    DrillShiftsStartTime = drill.DrillShiftsStartTime,
                });
            }

            if (drill.DrillErrorStartTime != null)
            {
                addList.Add(new ReCordDrillRateFactorDto
                {
                    DeviceId = drill.DeviceId,
                    Reason = DrillRateFactorReason.DrillAlarm,
                    StartTime = drill.DrillErrorStartTime,
                    EndTime = drill.DrillErrorEndTime,
                    LocationCode = drill.GetLocationCode(),
                    Memo = $"DrillEventId:{drill.DrillEventId} DrillScreenText:{drill.DrillScreenText}",
                    DrillShiftsStartTime = drill.DrillShiftsStartTime,
                });
            }

            if (drill.DrillAutomaticStartTime != null)
            {
                addList.Add(new ReCordDrillRateFactorDto
                {
                    DeviceId = drill.DeviceId,
                    Reason = DrillRateFactorReason.BufferAutomatic,
                    StartTime = drill.DrillAutomaticStartTime,
                    EndTime = drill.DrillAutomaticEndTime,
                    LocationCode = drill.GetLocationCode(),
                    DrillShiftsStartTime = drill.DrillShiftsStartTime,
                });
            }

            if (drill.DrillBoardDirectionStartTime != null)
            {
                addList.Add(new ReCordDrillRateFactorDto
                {
                    DeviceId = drill.DeviceId,
                    Reason = DrillRateFactorReason.DrillBoardDirection,
                    StartTime = drill.DrillBoardDirectionStartTime,
                    EndTime = drill.DrillBoardDirectionEndTime,
                    LocationCode = drill.GetLocationCode(),
                    DrillShiftsStartTime = drill.DrillShiftsStartTime,
                });
            }

            if (drill.DrillTestPinStartTime != null)
            {
                addList.Add(new ReCordDrillRateFactorDto
                {
                    DeviceId = drill.DeviceId,
                    Reason = DrillRateFactorReason.DrillTestPin,
                    StartTime = drill.DrillTestPinStartTime,
                    EndTime = drill.DrillTestPinEndTime,
                    LocationCode = drill.GetLocationCode(),
                    DrillShiftsStartTime = drill.DrillShiftsStartTime,
                });
            }

            if (drill.DrillToolEvaluationStartTime != null)
            {
                _logger.LogDebug($"ReCordDrillRateFactor {drill.DeviceId} DrillToolLifeExpiredStartTime {drill.DrillToolLifeExpiredStartTime},DrillToolLifeExpiredEndTime {drill.DrillToolLifeExpiredEndTime} ");
                addList.Add(new ReCordDrillRateFactorDto
                {
                    DeviceId = drill.DeviceId,
                    Reason = DrillRateFactorReason.DrillToolEvaluation,
                    StartTime = drill.DrillToolEvaluationStartTime,
                    EndTime = drill.DrillToolEvaluationEndTime,
                    LocationCode = drill.GetLocationCode(),
                    DrillShiftsStartTime = drill.DrillShiftsStartTime,
                });
            }

            if (drill.BufferManualStartTime != null)
            {
                addList.Add(new ReCordDrillRateFactorDto
                {
                    DeviceId = drill.DeviceId,
                    Reason = DrillRateFactorReason.BufferManual,
                    StartTime = drill.BufferManualStartTime,
                    EndTime = drill.BufferManualEndTime,
                    LocationCode = drill.GetLocationCode(),
                    DrillShiftsStartTime = drill.DrillShiftsStartTime,
                });
            }

            if (drill.DrillNoVacuumStartTime != null)
            {
                addList.Add(new ReCordDrillRateFactorDto
                {
                    DeviceId = drill.DeviceId,
                    Reason = DrillRateFactorReason.DrillNoVacuum,
                    StartTime = drill.DrillNoVacuumStartTime,
                    EndTime = drill.DrillNoVacuumEndTime,
                    LocationCode = drill.GetLocationCode(),
                    DrillShiftsStartTime = drill.DrillShiftsStartTime,
                });
            }

            if (drill.Location != null && drill.Location.ScheduleWaitingStartTime != null)
            {
                addList.Add(new ReCordDrillRateFactorDto
                {
                    DeviceId = drill.DeviceId,
                    Reason = DrillRateFactorReason.DrillScheduleWaitingForAGV,
                    StartTime = drill.Location.ScheduleWaitingStartTime,
                    EndTime = drill.Location.ScheduleWaitingEndTime,
                    LocationCode = drill.GetLocationCode(),
                    DrillShiftsStartTime = drill.DrillShiftsStartTime,
                });
            }

            if (drill.Location != null && drill.Location.ScheduleRunningStartTime != null)
            {
                addList.Add(new ReCordDrillRateFactorDto
                {
                    DeviceId = drill.DeviceId,
                    Reason = DrillRateFactorReason.DrillScheduleRunningToComplete,
                    StartTime = drill.Location.ScheduleRunningStartTime,
                    EndTime = drill.Location.ScheduleRunningEndTime,
                    LocationCode = drill.GetLocationCode(),
                    DrillShiftsStartTime = drill.DrillShiftsStartTime,
                });
            }
        }

        if (addList != null && addList.Count > 0)
        {
            List<ReCordDrillRateFactorDto> needRefreshDatas = new List<ReCordDrillRateFactorDto>();
            foreach (var i in addList)
            {
                if (!i.EndTime.HasValue || i.EndTime < i.StartTime)
                {
                    i.EndTime = i.StartTime;
                }

                if (i.DrillShiftsStartTime.HasValue && i.DrillShiftsStartTime > i.StartTime
                    && i.DrillShiftsStartTime < i.EndTime)
                {
                    needRefreshDatas.Add(new ReCordDrillRateFactorDto
                    {
                        DeviceId = i.DeviceId,
                        Reason = i.Reason,
                        StartTime = i.StartTime,
                        EndTime = i.DrillShiftsStartTime,
                        LocationCode = i.LocationCode,
                    });

                    i.StartTime = i.DrillShiftsStartTime;
                }
            }

            if (needRefreshDatas.Count > 0)
            {
                addList.AddRange(needRefreshDatas);
            }

            await _deviceAdapter.BulkReCordDrillRateFactor(addList);
        }
    }
}
