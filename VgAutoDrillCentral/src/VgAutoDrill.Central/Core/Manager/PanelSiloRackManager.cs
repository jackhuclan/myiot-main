using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Schedule;
using VgAutoDrill.Central.Core.Schedule.Summary;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Manager;

public abstract class PanelSiloRackManager<T, TScheduleTask> : IPanelSiloRackManager<T, TScheduleTask>
    where T : PanelSiloRack
    where TScheduleTask : ScheduleTaskWithRequest
{
    private readonly ILogger<PanelSiloRackManager<T, TScheduleTask>> _logger;
    private readonly IDrillManager _drillManager;
    private readonly IDeviceManager _deviceHolder;

    public PanelSiloRackManager(IServiceProvider serviceProvider)
    {
        _drillManager = serviceProvider.GetRequiredService<IDrillManager>();
        _deviceHolder = serviceProvider.GetRequiredService<IDeviceManager>();
        _logger = serviceProvider.GetRequiredService<ILogger<PanelSiloRackManager<T, TScheduleTask>>>();
    }

    public abstract IReadOnlyList<PanelSiloRack> DutyRacks { get; }
    public IReadOnlyList<PanelSiloRack> PanelSiloRacks => _deviceHolder.PanelSiloRacks ?? new List<PanelSiloRack>().AsReadOnly();

    public IReadOnlyList<Partition?> Partitions => DutyRacks.SelectMany(x => x.Locations).Select(x => x.Partition).Distinct()
        .Where(x => x != null).ToList().AsReadOnly() ?? new List<Partition?>().AsReadOnly();

    public IReadOnlyList<PanelSiloShelf> PanelSiloShelfs => _deviceHolder.PanelSiloShelfs ?? new List<PanelSiloShelf>().AsReadOnly();

    public IReadOnlyList<PanelSiloFork> PanelSiloForks => _deviceHolder.PanelSiloForks ?? new List<PanelSiloFork>().AsReadOnly();

    public abstract IReadOnlyList<TScheduleTask> NotStartedSchedules { get; }

    #region Locations

    /// <summary>
    /// 所有可用库位
    /// </summary>
    public IReadOnlyList<Location> AvailableLocations => DutyRacks.SelectMany(x => x.AvailableLocations)
        .Where(x => x != null).ToList().AsReadOnly()!;

    /// <summary>
    /// 可用的库位，发出了调度任务，没有被预约的库位
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <returns></returns>
    public IReadOnlyList<Location> PartitionAvailableLocations(string partitionCode) => DutyRacks.SelectMany(x => x.PartitionAvailableLocations(partitionCode))
        .Where(x => x != null).ToList().AsReadOnly()!;

    public IReadOnlyList<Location> PartitionAvailableLocations(string partitionCode, IReadOnlyList<string> routeCodes) => DutyRacks.SelectMany(x => x.PartitionAvailableLocations(partitionCode, routeCodes))
        .Where(x => x != null).ToList().AsReadOnly()!;

    /// <summary>
    /// 分区库位
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <returns></returns>
    public IReadOnlyList<Location> PartitionLocations(string partitionCode) => DutyRacks.SelectMany(x => x.PartitionLocations(partitionCode))
        .Where(x => x != null).ToList().AsReadOnly()!;

    /// <summary>
    /// 所有库位, 库位可能尚未发出调度记录，当前不可用
    /// </summary>
    public IReadOnlyList<Location> EnabledLocations => DutyRacks.SelectMany(x => x.Locations)
        .Where(x => x != null
            && x.Status == 1
        ).ToList().AsReadOnly()!;

    public IReadOnlyList<Location> PartitionEnabledLocations(string partitionCode) => EnabledLocations
            .Where(x => string.IsNullOrEmpty(partitionCode) || string.Compare(x.PartitionCode, partitionCode, StringComparison.OrdinalIgnoreCase) == 0)
            .ToList()
            .AsReadOnly();

    /// <summary>
    /// 存在未完成的库位
    /// </summary>
    public IReadOnlyList<Location> NotStartedLocations => AvailableLocations
        .Where(x => x != null && x.HasNotStartedSchedule).ToList().AsReadOnly()!;

    /// <summary>
    /// 存在未分配agv的库位
    /// </summary>
    public IReadOnlyList<Location> NotAllocatedLocations => AvailableLocations
        .Where(x => x != null && x.HasNotAllocatedSchedule).ToList().AsReadOnly()!;

    #endregion Locations

    #region Count

    /// <summary>
    /// 库位数
    /// </summary>
    public int AvailableLocationsCount() => AvailableLocations.Count();

    /// <summary>
    /// 统计包含所给物料状态的库位数量
    /// </summary>
    /// <param name="productStatuses">所给物料状态</param>
    /// <returns></returns>
    public int AvailableLocationsCount(IReadOnlyList<ProductStatus> productStatuses)
    {
        return AvailableLocations.Count(x => x.ContainProductStatuses(productStatuses));
    }

    /// <summary>
    /// 料仓数
    /// </summary>
    public int SiloCount() => AvailableLocations
                            .Select(loc => loc.HasSilo)
                            .Count();

    /// <summary>
    /// 当前空库位数
    /// </summary>
    public int EmptyPayloadNowCount => AvailableLocations.Where(loc => loc.IsEmptyPayloadNow).Count();

    public int PartitionEmptyPayloadNowCount(string partitionCode) => PartitionAvailableLocations(partitionCode).Where(loc => loc.IsEmptyPayloadNow).Count();

    /// <summary>
    /// 当前空仓数
    /// </summary>
    public int EmptySiloBoxNowCount => AvailableLocations.Where(loc => loc.IsEmptySiloBoxNow).Count();

    public int PartitionEmptySiloBoxNowCount(string partitionCode) => PartitionAvailableLocations(partitionCode).Where(loc => loc.IsEmptySiloBoxNow).Count();

    /// <summary>
    /// 即将空库位数
    /// </summary>
    public int EmptyPayloadSoonCount => AvailableLocations.Where(loc => loc.IsEmptyPayloadSoon).Count();

    public int PartitionEmptyPayloadSoonCount(string partitionCode) => PartitionAvailableLocations(partitionCode).Where(loc => loc.IsEmptyPayloadSoon).Count();

    /// <summary>
    /// 即将空仓数
    /// </summary>
    public int EmptySiloBoxSoonCount => AvailableLocations.Where(loc => loc.IsEmptySiloBoxSoon).Count();

    public int PartitionEmptySiloBoxSoonCount(string partitionCode) => PartitionAvailableLocations(partitionCode).Where(loc => loc.IsEmptySiloBoxSoon).Count();

    #endregion Count

    #region ItemSummaries

    public IReadOnlyList<DrilledItemSummary> DrilledItemSummaries => AvailableLocations
                                            .SelectMany(x => x.Panels.DrilledPanels)
                                            .GroupBy(x => x.ItemCode)
                                            .Select(x => new DrilledItemSummary
                                            {
                                                ItemCode = x.Key,
                                                ItemCount = x.Count(),
                                                RouteCodes = AvailableLocations
                                                            .Where(a => a.ContainsDrilledItemCode(x.Key))
                                                            .SelectMany(i => i.RouteCodes)
                                                            .Distinct()
                                                            .OrderBy(x => x)
                                                            .ToList(),
                                                ScheduleIds = AvailableLocations
                                                            .Where(a => a.ContainsDrilledItemCode(x.Key))
                                                             .Select(i => i.Schedule.Id)
                                                             .Distinct()
                                                             .OrderBy(x => x)
                                                             .ToList(),
                                                DeviceIds = AvailableLocations
                                                            .Where(a => a.ContainsDrilledItemCode(x.Key))
                                                            .Select(i => i.Code)
                                                            .Distinct()
                                                            .OrderBy(x => x)
                                                            .ToList(),
                                            })
                                            .ToList()
                                            .AsReadOnly();

    public IReadOnlyList<DrilledItemSummary> PartitionDrilledItemSummaries(string partitionCode) => PartitionAvailableLocations(partitionCode)
                                            .SelectMany(x => x.Panels.DrilledPanels)
                                            .GroupBy(x => x.ItemCode)
                                            .Select(x => new DrilledItemSummary
                                            {
                                                ItemCode = x.Key,
                                                ItemCount = x.Count(),
                                                RouteCodes = PartitionAvailableLocations(partitionCode)
                                                            .Where(a => a.ContainsDrilledItemCode(x.Key))
                                                            .SelectMany(i => i.RouteCodes)
                                                            .Distinct()
                                                            .OrderBy(x => x)
                                                            .ToList(),
                                                ScheduleIds = PartitionAvailableLocations(partitionCode)
                                                            .Where(a => a.ContainsDrilledItemCode(x.Key))
                                                             .Select(i => i.Schedule.Id)
                                                             .Distinct()
                                                             .OrderBy(x => x)
                                                             .ToList(),
                                                DeviceIds = PartitionAvailableLocations(partitionCode)
                                                            .Where(a => a.ContainsDrilledItemCode(x.Key))
                                                            .Select(i => i.Code)
                                                            .Distinct()
                                                            .OrderBy(x => x)
                                                            .ToList(),
                                            })
                                            .ToList()
                                            .AsReadOnly();

    public IReadOnlyList<UndrilledItemSummary> UndrilledItemSummaries => AvailableLocations
                                            .SelectMany(x => x.Panels.UndrilledPanels)
                                            .GroupBy(x => x.ItemCode)
                                            .Select(x => new UndrilledItemSummary
                                            {
                                                ItemCode = x.Key,
                                                ItemCount = x.Count(),
                                                RouteCodes = AvailableLocations
                                                            .Where(a => a.ContainsUndrilledItemCode(x.Key))
                                                            .SelectMany(i => i.RouteCodes)
                                                            .Distinct()
                                                            .OrderBy(x => x)
                                                            .ToList(),
                                                ScheduleIds = AvailableLocations
                                                            .Where(a => a.ContainsUndrilledItemCode(x.Key))
                                                             .Select(i => i.Schedule.Id)
                                                             .Distinct()
                                                             .OrderBy(x => x)
                                                             .ToList(),
                                                DeviceIds = AvailableLocations
                                                            .Where(a => a.ContainsUndrilledItemCode(x.Key))
                                                            .Select(i => i.Code)
                                                            .Distinct()
                                                            .OrderBy(x => x)
                                                            .ToList(),
                                            })
                                            .ToList()
                                            .AsReadOnly();

    public IReadOnlyList<UndrilledItemSummary> PartitionUndrilledItemSummaries(string partitionCode) => PartitionAvailableLocations(partitionCode)
                                            .SelectMany(x => x.Panels.UndrilledPanels)
                                            .GroupBy(x => x.ItemCode)
                                            .Select(x => new UndrilledItemSummary
                                            {
                                                ItemCode = x.Key,
                                                ItemCount = x.Count(),
                                                RouteCodes = PartitionAvailableLocations(partitionCode)
                                                            .Where(a => a.ContainsUndrilledItemCode(x.Key))
                                                            .SelectMany(i => i.RouteCodes)
                                                            .Distinct()
                                                            .OrderBy(x => x)
                                                            .ToList(),
                                                ScheduleIds = AvailableLocations
                                                            .Where(a => a.ContainsUndrilledItemCode(x.Key))
                                                             .Select(i => i.Schedule.Id)
                                                             .Distinct()
                                                             .OrderBy(x => x)
                                                             .ToList(),
                                                DeviceIds = AvailableLocations
                                                            .Where(a => a.ContainsUndrilledItemCode(x.Key))
                                                            .Select(i => i.Code)
                                                            .Distinct()
                                                            .OrderBy(x => x)
                                                            .ToList(),
                                            })
                                            .ToList()
                                            .AsReadOnly();

    public IReadOnlyList<FirstItemSummary> FirstItemSummaries => AvailableLocations
                                        .SelectMany(x => x.Panels.FirstPanels)
                                        .GroupBy(x => x.ItemCode)
                                        .Select(x => new FirstItemSummary
                                        {
                                            ItemCode = x.Key,
                                            ItemCount = x.Count(),
                                            RouteCodes = AvailableLocations
                                                            .Where(a => a.ContainsDrilledItemCode(x.Key))
                                                            .SelectMany(i => i.RouteCodes)
                                                            .Distinct()
                                                            .OrderBy(x => x)
                                                            .ToList(),
                                            ScheduleIds = AvailableLocations
                                                            .Where(a => a.ContainsDrilledItemCode(x.Key))
                                                             .Select(i => i.Schedule.Id)
                                                             .Distinct()
                                                             .OrderBy(x => x)
                                                             .ToList(),
                                            DeviceIds = AvailableLocations
                                                            .Where(a => a.ContainsDrilledItemCode(x.Key))
                                                            .Select(i => i.Code)
                                                            .Distinct()
                                                            .OrderBy(x => x)
                                                            .ToList(),
                                        })
                                        .ToList()
                                        .AsReadOnly();

    public IReadOnlyList<FirstItemSummary> PartitionFirstItemSummaries(string partitionCode) => PartitionAvailableLocations(partitionCode)
                                        .SelectMany(x => x.Panels.FirstPanels)
                                        .GroupBy(x => x.ItemCode)
                                        .Select(x => new FirstItemSummary
                                        {
                                            ItemCode = x.Key,
                                            ItemCount = x.Count(),
                                            RouteCodes = PartitionAvailableLocations(partitionCode)
                                                            .Where(a => a.ContainsDrilledItemCode(x.Key))
                                                            .SelectMany(i => i.RouteCodes)
                                                            .Distinct()
                                                            .OrderBy(x => x)
                                                            .ToList(),
                                            ScheduleIds = AvailableLocations
                                                            .Where(a => a.ContainsDrilledItemCode(x.Key))
                                                             .Select(i => i.Schedule.Id)
                                                             .Distinct()
                                                             .OrderBy(x => x)
                                                             .ToList(),
                                            DeviceIds = AvailableLocations
                                                            .Where(a => a.ContainsDrilledItemCode(x.Key))
                                                            .Select(i => i.Code)
                                                            .Distinct()
                                                            .OrderBy(x => x)
                                                            .ToList(),
                                        })
                                        .ToList()
                                        .AsReadOnly();

    #endregion ItemSummaries

    #region PanelCount

    /// <summary>
    /// 所有插齿上的板料汇总
    /// </summary>
    /// <returns></returns>
    public int PanelCount()
    {
        return DutyRacks.SelectMany(x => x.AvailableLocations)
             .Sum(s => s == null ? 0 : s.PanelCount());
    }

    /// <summary>
    /// 某个插齿上的板料汇总
    /// </summary>
    /// <param name="deviceId">pin的设备id</param>
    /// <returns></returns>
    public int PanelCount(string deviceId)
    {
        return DutyRacks.Where(x => x.DeviceId == deviceId)
             .SelectMany(x => x.AvailableLocations)
             .Sum(s => s == null ? 0 : s.PanelCount());
    }

    /// <summary>
    /// 所有插齿上的某些状态的板料汇总
    /// </summary>
    /// <param name="productStatuses">物料状态</param>
    /// <returns></returns>
    public int PanelCount(IReadOnlyList<ProductStatus> productStatuses)
    {
        return DutyRacks.SelectMany(x => x.AvailableLocations)
             .Sum(s => s == null ? 0 : s.PanelCount(productStatuses));
    }

    /// <summary>
    /// 某个插齿上的某些状态的板料汇总
    /// </summary>
    /// <param name="deviceId">pin的设备id</param>
    /// <param name="productStatuses">物料状态</param>
    /// <returns></returns>
    public int PanelCount(string deviceId, IReadOnlyList<ProductStatus> productStatuses)
    {
        return DutyRacks.Where(x => x.DeviceId == deviceId)
            .SelectMany(x => x.AvailableLocations)
            .Sum(s => s == null ? 0 : s.PanelCount(productStatuses));
    }

    /// <summary>
    /// 某个库位上的某些状态的板料汇总
    /// </summary>
    /// <param name="locationCode">库位</param>
    /// <param name="productStatuses">物料状态</param>
    /// <returns></returns>
    public int LocationPanelCount(string locationCode, IReadOnlyList<ProductStatus> productStatuses)
    {
        return DutyRacks.SelectMany(x => x.AvailableLocations)
            .Where(x => x.Code == locationCode)
            .Sum(s => s == null ? 0 : s.PanelCount(productStatuses));
    }

    #endregion PanelCount

    #region TryFindEmptySiloBoxLocation

    /// <inheritdoc/>
    public bool TryFindEmptySiloBoxLocation(out Location? outboundingLocation)
    {
        return TryFindEmptySiloBoxLocation(string.Empty, out outboundingLocation);
    }

    public bool TryFindEmptySiloBoxLocation(string[] excludedLocations, out Location? outboundingLocation)
    {
        return TryFindEmptySiloBoxLocation(string.Empty, excludedLocations, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindEmptySiloBoxLocation(string partitionCode, out Location? outboundingLocation)
    {
        return TryFindEmptySiloBoxLocation(partitionCode, new string[] { }, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindEmptySiloBoxLocation(string partitionCode, string[] excludedLocations, out Location? outboundingLocation)
    {
        foreach (T fork in DutyRacks)
        {
            if (fork.TryFindEmptySiloBoxLocation(partitionCode, excludedLocations, out outboundingLocation))
            {
                return true;
            }
        }

        outboundingLocation = null;
        return false;
    }

    #endregion TryFindEmptySiloBoxLocation

    #region ScheduleRequirement TryFindLocation

    /// <inheritdoc/>
    public bool TryFindLocation(ScheduleRequirement scheduleRequirement, out Location? outboundingLocation)
    {
        return TryFindLocation(scheduleRequirement, string.Empty, new List<string> { }, new string[] { }, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindLocation(ScheduleRequirement scheduleRequirement, string partitionCode, out Location? outboundingLocation)
    {
        return TryFindLocation(scheduleRequirement, partitionCode, new List<string> { }, new string[] { }, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindLocation(ScheduleRequirement scheduleRequirement, string[] excludedLocations, out Location? outboundingLocation)
    {
        return TryFindLocation(scheduleRequirement, string.Empty, new List<string> { }, excludedLocations, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindLocation(ScheduleRequirement scheduleRequirement, string partitionCode, string[] excludedLocations, out Location? outboundingLocation)
    {
        return TryFindLocation(scheduleRequirement, partitionCode, new List<string> { }, excludedLocations, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindLocation(ScheduleRequirement scheduleRequirement, string partitionCode, IReadOnlyList<string> routeCodes, out Location? outboundingLocation)
    {
        return TryFindLocation(scheduleRequirement, partitionCode, routeCodes, new string[] { }, out outboundingLocation);
    }

    public bool TryFindLocation(ScheduleRequirement scheduleRequirement, string partitionCode, IReadOnlyList<string> routeCodes, string[] excludedLocations, out Location? outboundingLocation)
    {
        foreach (var fork in DutyRacks)
        {
            if (fork.TryFindLocation(scheduleRequirement, partitionCode, routeCodes, excludedLocations, out outboundingLocation)
                && outboundingLocation != null)
            {
                outboundingLocation.ServingForSchedule = scheduleRequirement.OriginalSchedule;
                return true;
            }
        }

        outboundingLocation = null;
        return false;
    }

    #endregion ScheduleRequirement TryFindLocation

    #region TryFindArbitraryDrillRequiredLocation

    /// <inheritdoc/>
    public bool TryFindArbitraryDrillRequiredLocation(out Location? outboundingLocation)
    {
        return TryFindArbitraryDrillRequiredLocation(string.Empty, new List<string>() { }, new string[] { }, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindArbitraryDrillRequiredLocation(string partitionCode, out Location? outboundingLocation)
    {
        return TryFindArbitraryDrillRequiredLocation(partitionCode, new List<string>() { }, new string[] { }, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindArbitraryDrillRequiredLocation(string[] excludedLocations, out Location? outboundingLocation)
    {
        return TryFindArbitraryDrillRequiredLocation(string.Empty, new List<string>() { }, excludedLocations, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindArbitraryDrillRequiredLocation(string partitionCode, string[] excludedLocations, out Location? outboundingLocation)
    {
        return TryFindArbitraryDrillRequiredLocation(partitionCode, new List<string>() { }, excludedLocations, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindArbitraryDrillRequiredLocation(string partitionCode, IReadOnlyList<string> routeCodes, out Location? outboundingLocation)
    {
        return TryFindArbitraryDrillRequiredLocation(partitionCode, routeCodes, new string[] { }, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindArbitraryDrillRequiredLocation(string partitionCode, IReadOnlyList<string> routeCodes, string[] excludedLocations, out Location? outboundingLocation)
    {
        _logger.LogDebug($"DrillScheduleHandler:partitionCode={partitionCode} agv.RouteCodes={string.Join(",", routeCodes)} has silo, TryFindArbitraryDrillRequiredLocation...");
        var requirements = _drillManager.OrderedRequirements(routeCodes);
        requirements.ToList().Sort();

        _logger.LogDebug($"DrillScheduleHandler:partitionCode={partitionCode} agv.RouteCodes={string.Join(",", routeCodes)}, drillRequirements={requirements.Count}");
        foreach (var scheduleRequirement in requirements)
        {
            _logger.LogDebug($"DrillScheduleHandler:partitionCode={partitionCode} agv.RouteCodes={string.Join(",", routeCodes)}, scheduleRequirement={scheduleRequirement.CallerDeviceId}");
            if (TryFindLocation(scheduleRequirement, partitionCode, routeCodes, excludedLocations, out outboundingLocation))
            {
                _logger.LogDebug($"DrillScheduleHandler:partitionCode={partitionCode} agv.RouteCodes={string.Join(",", routeCodes)}, " +
                    $"scheduleRequirement={scheduleRequirement.CallerDeviceId}, match location={outboundingLocation?.Code}");
                return true;
            }

            _logger.LogDebug($"DrillScheduleHandler:partitionCode={partitionCode} agv.RouteCodes={string.Join(",", routeCodes)}, " +
                $"scheduleRequirement={scheduleRequirement.CallerDeviceId}, match location failed");
        }

        _logger.LogDebug($"DrillScheduleHandler:partitionCode={partitionCode} agv.RouteCodes={string.Join(",", routeCodes)}, TryFindArbitraryDrillRequiredLocation failed");
        outboundingLocation = null;
        return false;
    }

    #endregion TryFindArbitraryDrillRequiredLocation

    #region TryFindLocation

    public bool TryFindLocation(out Location? outboundingLocation)
    {
        return TryFindLocation(string.Empty, new string[] { }, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindLocation(string[] excludedLocations, out Location? outboundingLocation)
    {
        return TryFindLocation(string.Empty, excludedLocations, out outboundingLocation);
    }

    public bool TryFindLocation(string partitionCode, out Location? outboundingLocation)
    {
        return TryFindLocation(string.Empty, new string[] { }, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindLocation(string partitionCode, string[] excludedLocations, out Location? outboundingLocation)
    {
        foreach (var fork in DutyRacks)
        {
            if (fork.TryFindLocation(partitionCode, excludedLocations, out outboundingLocation))
            {
                return true;
            }
        }

        outboundingLocation = null;
        return false;
    }

    #endregion TryFindLocation

    #region TryFindFullDrilledLocation

    /// <inheritdoc/>
    public bool TryFindFullDrilledLocation(ConfigParameters parameters, out Location? outboundingLocation)
    {
        return TryFindFullDrilledLocation(parameters, string.Empty, new string[] { }, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindFullDrilledLocation(ConfigParameters parameters, string[] excludedLocations, out Location? outboundingLocation)
    {
        return TryFindFullDrilledLocation(parameters, string.Empty, excludedLocations, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindFullDrilledLocation(ConfigParameters parameters, string partitionCode, out Location? outboundingLocation)
    {
        return TryFindFullDrilledLocation(parameters, partitionCode, new string[] { }, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindFullDrilledLocation(ConfigParameters parameters, string partitionCode, string[] excludedLocations, out Location? outboundingLocation)
    {
        foreach (var fork in DutyRacks)
        {
            if (fork.TryFindFullDrilledLocation(parameters, partitionCode, excludedLocations, out outboundingLocation))
            {
                return true;
            }
        }

        outboundingLocation = null;
        return false;
    }

    #endregion TryFindFullDrilledLocation

    #region TryFindFirstDrilledLocation

    /// <inheritdoc/>
    public bool TryFindFirstDrilledLocation(ConfigParameters parameters, out Location? outboundingLocation)
    {
        return TryFindFirstDrilledLocation(parameters, string.Empty, new string[] { }, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindFirstDrilledLocation(ConfigParameters parameters, string[] excludedLocations, out Location? outboundingLocation)
    {
        return TryFindFirstDrilledLocation(parameters, string.Empty, excludedLocations, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindFirstDrilledLocation(ConfigParameters parameters, string partitionCode, out Location? outboundingLocation)
    {
        return TryFindFirstDrilledLocation(parameters, string.Empty, new string[] { }, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindFirstDrilledLocation(ConfigParameters parameters, string partitionCode, string[] excludedLocations, out Location? outboundingLocation)
    {
        foreach (var fork in DutyRacks)
        {
            if (fork.TryFindFirstDrilledLocation(parameters, partitionCode, excludedLocations, out outboundingLocation))
            {
                return true;
            }
        }

        outboundingLocation = null;
        return false;
    }

    #endregion TryFindFirstDrilledLocation

    #region TryFindTimeoutUnloadingLocation

    /// <inheritdoc/>
    public bool TryFindTimeoutUnloadingLocation(out Location? outboundingLocation)
    {
        return TryFindTimeoutUnloadingLocation(string.Empty, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindTimeoutUnloadingLocation(string[] excludedLocations, out Location? outboundingLocation)
    {
        return TryFindTimeoutUnloadingLocation(string.Empty, excludedLocations, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindTimeoutUnloadingLocation(string partitionCode, out Location? outboundingLocation)
    {
        return TryFindTimeoutUnloadingLocation(partitionCode, new string[] { }, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindTimeoutUnloadingLocation(string partitionCode, string[] excludedLocations, out Location? outboundingLocation)
    {
        foreach (var fork in DutyRacks)
        {
            if (fork.TryFindTimeoutUnloadingLocation(partitionCode, excludedLocations, out outboundingLocation))
            {
                return true;
            }
        }

        outboundingLocation = null;
        return false;
    }

    #endregion TryFindTimeoutUnloadingLocation

    #region TryFindNoDrillRequiredUndrilledLocation

    /// <inheritdoc/>
    public bool TryFindNoDrillRequiredUndrilledLocation(out Location? outboundingLocation)
    {
        return TryFindNoDrillRequiredUndrilledLocation(string.Empty, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindNoDrillRequiredUndrilledLocation(string[] excludedLocations, out Location? outboundingLocation)
    {
        return TryFindNoDrillRequiredUndrilledLocation(string.Empty, excludedLocations, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindNoDrillRequiredUndrilledLocation(string partitionCode, out Location? outboundingLocation)
    {
        return TryFindNoDrillRequiredUndrilledLocation(partitionCode, new string[] { }, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindNoDrillRequiredUndrilledLocation(string partitionCode, string[] excludedLocations, out Location? outboundingLocation)
    {
        foreach (var fork in DutyRacks)
        {
            if (fork.TryFindNoDrillRequiredUndrilledLocation(partitionCode, excludedLocations, out outboundingLocation))
            {
                return true;
            }
        }

        outboundingLocation = null;
        return false;
    }

    #endregion TryFindNoDrillRequiredUndrilledLocation

    #region TryFindEmptyPayloadLocation

    /// <inheritdoc/>
    public bool TryFindEmptyPayloadLocation(int minEmptyPayloadNum, out Location? inboundingLocation)
    {
        return TryFindEmptyPayloadLocation(minEmptyPayloadNum, string.Empty, new List<string>() { }, new string[] { }, out inboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindEmptyPayloadLocation(int minEmptyPayloadNum, string[] excludedLocations, out Location? inboundingLocation)
    {
        return TryFindEmptyPayloadLocation(minEmptyPayloadNum, string.Empty, new List<string>() { }, excludedLocations, out inboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindEmptyPayloadLocation(int minEmptyPayloadNum, string partitionCode, out Location? inboundingLocation)
    {
        return TryFindEmptyPayloadLocation(minEmptyPayloadNum, partitionCode, new List<string>() { }, new string[] { }, out inboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindEmptyPayloadLocation(int minEmptyPayloadNum, string partitionCode, string[] excludedLocations, out Location? inboundingLocation)
    {
        return TryFindEmptyPayloadLocation(minEmptyPayloadNum, partitionCode, new List<string>() { }, excludedLocations, out inboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindEmptyPayloadLocation(int minEmptyPayloadNum, string partitionCode, IReadOnlyList<string> routeCodes, out Location? inboundingLocation)
    {
        return TryFindEmptyPayloadLocation(minEmptyPayloadNum, partitionCode, routeCodes, new string[] { }, out inboundingLocation);
    }

    public bool TryFindEmptyPayloadLocation(string partitionCode, IReadOnlyList<string> routeCodes, out Location? inboundingLocation)
    {
        return TryFindEmptyPayloadLocation(1, partitionCode, routeCodes, new string[] { }, out inboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindEmptyPayloadLocation(int minEmptyPayloadNum, string partitionCode, IReadOnlyList<string> routeCodes, string[] excludedLocations, out Location? inboundingLocation)
    {
        foreach (var fork in DutyRacks)
        {
            if (fork.TryFindEmptyPayloadLocation(minEmptyPayloadNum, partitionCode, routeCodes, excludedLocations, out inboundingLocation))
            {
                _logger.LogDebug($"DrillScheduleHandler:partitionCode={partitionCode} agv.RouteCodes={string.Join(",", routeCodes)} has silo, TryFindEmptyPayloadLocation={inboundingLocation?.Code}");
                return true;
            }
        }

        _logger.LogDebug($"DrillScheduleHandler:partitionCode={partitionCode} agv.RouteCodes={string.Join(",", routeCodes)} has silo, TryFindEmptyPayloadLocation failed!");
        inboundingLocation = null;
        return false;
    }

    #endregion TryFindEmptyPayloadLocation

    #region TryFindAllocatedLocation

    /// <summary>
    /// 从板料架子指定分区上 找到 已经分配给指定agv类型的库位
    /// </summary>
    /// <param name="agvKind">指定agv类型</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindAllocatedLocation(DeviceKind agvKind, out Location? outboundingLocation)
    {
        return TryFindAllocatedLocation(agvKind, string.Empty, new string[] { }, out outboundingLocation);
    }

    /// <summary>
    /// 从板料架子指定分区上 找到 已经分配给指定agv类型的库位
    /// </summary>
    /// <param name="agvKind">指定agv类型</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindAllocatedLocation(DeviceKind agvKind, string[] excludedLocations, out Location? outboundingLocation)
    {
        return TryFindAllocatedLocation(agvKind, string.Empty, excludedLocations, out outboundingLocation);
    }

    /// <summary>
    /// 从板料架子指定分区上 找到 已经分配给指定agv类型的库位
    /// </summary>
    /// <param name="agvKind">指定agv类型</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindAllocatedLocation(DeviceKind agvKind, string partitionCode, out Location? outboundingLocation)
    {
        return TryFindAllocatedLocation(agvKind, partitionCode, new string[] { }, out outboundingLocation);
    }

    /// <summary>
    /// 从板料架子指定分区上 找到 已经分配给指定agv类型的库位
    /// </summary>
    /// <param name="agvKind">指定agv类型</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindAllocatedLocation(DeviceKind agvKind, string partitionCode, string[] excludedLocations, out Location? outboundingLocation)
    {
        foreach (var fork in DutyRacks)
        {
            if (fork.TryFindAllocatedLocation(agvKind, partitionCode, excludedLocations, out outboundingLocation))
            {
                return true;
            }
        }

        outboundingLocation = null;
        return false;
    }

    /// <summary>
    /// 从板料架子指定分区上 找到 已经分配给某个车的库位
    /// </summary>
    /// <param name="agvId">指定agv</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindAllocatedLocation(string agvId, out Location? outboundingLocation)
    {
        return TryFindAllocatedLocation(agvId, string.Empty, new string[] { }, out outboundingLocation);
    }

    /// <summary>
    /// 从板料架子指定分区上 找到 已经分配给某个车的库位
    /// </summary>
    /// <param name="agvId">指定agv</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindAllocatedLocation(string agvId, string[] excludedLocations, out Location? outboundingLocation)
    {
        return TryFindAllocatedLocation(agvId, string.Empty, excludedLocations, out outboundingLocation);
    }

    /// <summary>
    /// 从板料架子指定分区上 找到 已经分配给某个车的库位
    /// </summary>
    /// <param name="agvId">指定agv</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindAllocatedLocation(string agvId, string partitionCode, out Location? outboundingLocation)
    {
        return TryFindAllocatedLocation(agvId, partitionCode, new string[] { }, out outboundingLocation);
    }

    /// <summary>
    /// 从板料架子指定分区上 找到 已经分配给某个车的库位
    /// </summary>
    /// <param name="agvId">指定agv</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindAllocatedLocation(string agvId, string partitionCode, string[] excludedLocations, out Location? outboundingLocation)
    {
        foreach (var fork in DutyRacks)
        {
            if (fork.TryFindAllocatedLocation(agvId, partitionCode, excludedLocations, out outboundingLocation))
            {
                return true;
            }
        }

        outboundingLocation = null;
        return false;
    }

    #endregion TryFindAllocatedLocation

    #region TryFindCanAcceptDrilledItemsLocation

    /// <inheritdoc/>
    public bool TryFindCanAcceptDrilledItemsLocation(ScheduleRequirement requirement, out Location? outboundingLocation)
    {
        return TryFindCanAcceptDrilledItemsLocation(requirement, string.Empty, new string[] { }, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindCanAcceptDrilledItemsLocation(ScheduleRequirement requirement, string[] excludedLocations, out Location? outboundingLocation)
    {
        return TryFindCanAcceptDrilledItemsLocation(requirement, string.Empty, excludedLocations, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindCanAcceptDrilledItemsLocation(ScheduleRequirement requirement, string partitionCode, out Location? outboundingLocation)
    {
        return TryFindCanAcceptDrilledItemsLocation(requirement, string.Empty, new string[] { }, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindCanAcceptDrilledItemsLocation(ScheduleRequirement requirement, string partitionCode, string[] excludedLocations, out Location? outboundingLocation)
    {
        return TryFindCanAcceptDrilledItemsLocation(requirement, partitionCode, new List<string> { }, new string[] { }, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindCanAcceptDrilledItemsLocation(ScheduleRequirement requirement, string partitionCode, IReadOnlyList<string> routeCodes, string[] excludedLocations, out Location? outboundingLocation)
    {
        foreach (var fork in DutyRacks.Where(x => x.PartitionAvailableLocations(partitionCode, routeCodes).Any()))
        {
            if (fork.TryFindCanAcceptDrilledPanelsLocation(requirement, partitionCode, routeCodes, excludedLocations, out outboundingLocation))
            {
                return true;
            }
        }

        outboundingLocation = null;
        return false;
    }

    #endregion TryFindCanAcceptDrilledItemsLocation

    #region TryFindHasUndrilledPanelsLocation

    /// <inheritdoc/>
    public bool TryFindHasUndrilledPanelsLocation(string requiredUndrilledItemCode, int requiredUndrilledItemQuantity, out Location? outboundingLocation)
    {
        foreach (var fork in DutyRacks)
        {
            if (fork.TryFindHasUndrilledPanelsLocation(requiredUndrilledItemCode, requiredUndrilledItemQuantity, out outboundingLocation))
            {
                return true;
            }
        }

        outboundingLocation = null;
        return false;
    }

    /// <inheritdoc/>
    public bool TryFindHasUndrilledPanelsLocation(ScheduleRequirement requirement, string partitionCode, string[] excludedLocations, out Location? outboundingLocation)
    {
        foreach (var fork in DutyRacks.Where(x => x.AvailablePartitionLocations(partitionCode, new string[] { }).Any()))
        {
            if (fork.TryFindHasUndrilledPanelsLocation(requirement, partitionCode, new string[] { }, excludedLocations, out outboundingLocation))
            {
                return true;
            }
        }

        outboundingLocation = null;
        return false;
    }

    /// <inheritdoc/>
    public bool TryFindHasUndrilledPanelsLocation(ScheduleRequirement requirement, string partitionCode, IReadOnlyList<string> routeCodes, string[] excludedLocations, out Location? outboundingLocation)
    {
        foreach (var fork in DutyRacks.Where(x => x.AvailablePartitionLocations(partitionCode, new string[] { }).Any()))
        {
            if (fork.TryFindHasUndrilledPanelsLocation(requirement, partitionCode, new string[] { }, excludedLocations, out outboundingLocation))
            {
                return true;
            }
        }

        outboundingLocation = null;
        return false;
    }

    #endregion TryFindHasUndrilledPanelsLocation

    #region TryFindIdleTimeoutUndrilledLocation

    /// <inheritdoc/>
    public bool TryFindIdleTimeoutUndrilledLocation(ConfigParameters parameters, out Location? outboundingLocation)
    {
        return TryFindIdleTimeoutUndrilledLocation(parameters, string.Empty, new string[] { }, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindIdleTimeoutUndrilledLocation(ConfigParameters parameters, string[] excludedLocations, out Location? outboundingLocation)
    {
        return TryFindIdleTimeoutUndrilledLocation(parameters, string.Empty, excludedLocations, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindIdleTimeoutUndrilledLocation(ConfigParameters parameters, string partitionCode, out Location? outboundingLocation)
    {
        return TryFindIdleTimeoutUndrilledLocation(parameters, partitionCode, new string[] { }, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindIdleTimeoutUndrilledLocation(ConfigParameters parameters, string partitionCode, string[] excludedLocations, out Location? outboundingLocation)
    {
        foreach (var fork in DutyRacks)
        {
            if (fork.TryFindIdleTimeoutUndrilledLocation(parameters, partitionCode, excludedLocations, out outboundingLocation))
            {
                return true;
            }
        }

        outboundingLocation = null;
        return false;
    }

    #endregion TryFindIdleTimeoutUndrilledLocation

    #region TryFindIdleTimeoutDrilledLocation

    /// <inheritdoc/>
    public bool TryFindIdleTimeoutDrilledLocation(ConfigParameters parameters, out Location? outboundingLocation)
    {
        return TryFindIdleTimeoutDrilledLocation(parameters, string.Empty, new string[] { }, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindIdleTimeoutDrilledLocation(ConfigParameters parameters, string[] excludedLocations, out Location? outboundingLocation)
    {
        return TryFindIdleTimeoutDrilledLocation(parameters, string.Empty, excludedLocations, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindIdleTimeoutDrilledLocation(ConfigParameters parameters, string partitionCode, out Location? outboundingLocation)
    {
        return TryFindIdleTimeoutDrilledLocation(parameters, string.Empty, new string[] { }, out outboundingLocation);
    }

    /// <inheritdoc/>
    public bool TryFindIdleTimeoutDrilledLocation(ConfigParameters parameters, string partitionCode, string[] excludedLocations, out Location? outboundingLocation)
    {
        foreach (var fork in DutyRacks)
        {
            if (fork.TryFindIdleTimeoutDrilledLocation(parameters, partitionCode, excludedLocations, out outboundingLocation))
            {
                return true;
            }
        }

        outboundingLocation = null;
        return false;
    }

    #endregion TryFindIdleTimeoutDrilledLocation

    #region 负载的料仓进行summary

    /// <summary>
    /// 负载的生料仓
    /// </summary>
    public IReadOnlyList<PayloadUndrilledSiloSummary> PayloadUndrilledSiloSummaries => DutyRacks
        .SelectMany(x => x.AvailableLocations)
        .SelectMany(x => x.UndrilledItemCodes)
        .Where(x => x.Any())
        .GroupBy(x => x)
        .Select(x => new PayloadUndrilledSiloSummary
        {
            ItemCode = x.Key,
            SiloCodes = DutyRacks.SelectMany(x => x.AvailableLocations)
                        .Where(y => y.UndrilledItemCodes.Contains(x.Key))
                        .Select(y => y.SiloCode)
                        .ToList()
                        .AsReadOnly(),
            SiloCount = DutyRacks.SelectMany(x => x.AvailableLocations)
                        .Where(y => y.UndrilledItemCodes.Contains(x.Key))
                        .Select(y => y.SiloCode)
                        .ToList()
                        .Count(),
            LocatioCodes = DutyRacks.SelectMany(x => x.AvailableLocations)
                        .Where(y => y.UndrilledItemCodes.Contains(x.Key))
                        .Select(y => y.Code)
                        .ToList(),
            UndrilledItemCounts = DutyRacks.SelectMany(x => x.AvailableLocations)
                        .Where(y => y.UndrilledItemCodes.Contains(x.Key))
                        .Select(y => y.UndrilledItemCodes.Count(z => z == x.Key))
                        .ToList()
                        .AsReadOnly()
        })
        .ToList()
        .AsReadOnly();

    /// <summary>
    /// 负载的熟料仓
    /// </summary>
    public IReadOnlyList<PayloadDrilledSiloSummary> PayloadDrilledSiloSummaries => DutyRacks
        .SelectMany(x => x.AvailableLocations)
        .Where(x => !x.ContainsFirst)
        .SelectMany(x => x.DrilledItemCodes)
        .Where(x => x.Any())
        .GroupBy(x => x)
        .Select(x => new PayloadDrilledSiloSummary
        {
            ItemCode = x.Key,
            SiloCodes = DutyRacks.SelectMany(x => x.AvailableLocations)
                        .Where(y => y.DrilledItemCodes.Contains(x.Key))
                        .Select(y => y.SiloCode)
                        .ToList()
                        .AsReadOnly(),
            SiloCount = DutyRacks.SelectMany(x => x.AvailableLocations)
                        .Where(y => y.DrilledItemCodes.Contains(x.Key))
                        .Select(y => y.SiloCode)
                        .ToList()
                        .Count(),
            LocatioCodes = DutyRacks.SelectMany(x => x.AvailableLocations)
                        .Where(y => y.DrilledItemCodes.Contains(x.Key))
                        .Select(y => y.Code)
                        .ToList()
                        .AsReadOnly(),
            EmptyLayerCounts = DutyRacks.SelectMany(x => x.AvailableLocations)
                        .Where(y => y.DrilledItemCodes.Contains(x.Key))
                        .Select(x => x.EmptySiloBoxCount)
                        .ToList()
                        .AsReadOnly()
        })
        .ToList()
        .AsReadOnly();

    /// <summary>
    /// 负载的首件仓
    /// </summary>
    public IReadOnlyList<PayloadFirstSiloSummary> PayloadFirstSiloSummaries => DutyRacks
        .SelectMany(x => x.AvailableLocations)
        .Where(x => x.ContainsFirst)
        .SelectMany(x => x.FirstItemCodes)
        .Where(x => x.Any())
        .GroupBy(x => x)
        .Select(x => new PayloadFirstSiloSummary
        {
            ItemCode = x.Key,
            SiloCodes = DutyRacks.SelectMany(x => x.AvailableLocations)
                        .Where(y => y.FirstItemCodes.Contains(x.Key))
                        .Select(y => y.SiloCode)
                        .ToList()
                        .AsReadOnly(),
            SiloCount = DutyRacks.SelectMany(x => x.AvailableLocations)
                        .Where(y => y.FirstItemCodes.Contains(x.Key))
                        .Select(y => y.SiloCode)
                        .ToList()
                        .Count(),
            LocatioCodes = DutyRacks.SelectMany(x => x.AvailableLocations)
                        .Where(y => y.FirstItemCodes.Contains(x.Key))
                        .Select(y => y.Code)
                        .ToList()
                        .AsReadOnly(),
            EmptyLayerCounts = DutyRacks.SelectMany(x => x.AvailableLocations)
                        .Where(y => y.FirstItemCodes.Contains(x.Key))
                        .Select(x => x.EmptySiloBoxCount)
                        .ToList()
                        .AsReadOnly()
        })
        .ToList()
        .AsReadOnly();

    #endregion 负载的料仓进行summary

    #region 指定分区下的负载的料仓进行summary

    /// <summary>
    /// 指定分区下的负载的生料仓
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    public IReadOnlyList<PayloadUndrilledSiloSummary> PartitionPayloadUndrilledSiloSummaries(string partitionCode) => DutyRacks
        .SelectMany(x => x.PartitionAvailableLocations(partitionCode))
        .SelectMany(x => x.UndrilledItemCodes)
        .Where(x => x.Any())
        .GroupBy(x => x)
        .Select(x => new PayloadUndrilledSiloSummary
        {
            ItemCode = x.Key,
            SiloCodes = DutyRacks.SelectMany(x => x.PartitionAvailableLocations(partitionCode))
                        .Where(y => y.UndrilledItemCodes.Contains(x.Key))
                        .Select(y => y.SiloCode)
                        .ToList()
                        .AsReadOnly(),
            SiloCount = DutyRacks.SelectMany(x => x.PartitionAvailableLocations(partitionCode))
                        .Where(y => y.UndrilledItemCodes.Contains(x.Key))
                        .Select(y => y.SiloCode)
                        .ToList()
                        .Count(),
            LocatioCodes = DutyRacks.SelectMany(x => x.PartitionAvailableLocations(partitionCode))
                        .Where(y => y.UndrilledItemCodes.Contains(x.Key))
                        .Select(y => y.Code)
                        .ToList(),
            UndrilledItemCounts = DutyRacks.SelectMany(x => x.PartitionAvailableLocations(partitionCode))
                        .Where(y => y.UndrilledItemCodes.Contains(x.Key))
                        .Select(y => y.UndrilledItemCodes.Count(z => z == x.Key))
                        .ToList()
                        .AsReadOnly()
        })
        .ToList()
        .AsReadOnly();

    /// <summary>
    /// 指定分区下的钻机需要的熟料仓
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    public IReadOnlyList<PayloadDrilledSiloSummary> PartitionPayloadDrilledSiloSummaries(string partitionCode) => DutyRacks
        .SelectMany(x => x.PartitionAvailableLocations(partitionCode))
        .Where(x => !x.ContainsFirst)
        .SelectMany(x => x.DrilledItemCodes)
        .Where(x => x.Any())
        .GroupBy(x => x)
        .Select(x => new PayloadDrilledSiloSummary
        {
            ItemCode = x.Key,
            SiloCodes = DutyRacks.SelectMany(x => x.PartitionAvailableLocations(partitionCode))
                        .Where(y => y.DrilledItemCodes.Contains(x.Key))
                        .Select(y => y.SiloCode)
                        .ToList()
                        .AsReadOnly(),
            SiloCount = DutyRacks.SelectMany(x => x.PartitionAvailableLocations(partitionCode))
                        .Where(y => y.DrilledItemCodes.Contains(x.Key))
                        .Select(y => y.SiloCode)
                        .ToList()
                        .Count(),
            LocatioCodes = DutyRacks.SelectMany(x => x.PartitionAvailableLocations(partitionCode))
                        .Where(y => y.DrilledItemCodes.Contains(x.Key))
                        .Select(y => y.Code)
                        .ToList()
                        .AsReadOnly(),
            EmptyLayerCounts = DutyRacks.SelectMany(x => x.PartitionAvailableLocations(partitionCode))
                        .Where(y => y.DrilledItemCodes.Contains(x.Key))
                        .Select(x => x.EmptySiloBoxCount)
                        .ToList()
                        .AsReadOnly()
        })
        .ToList()
        .AsReadOnly();

    /// <summary>
    /// 指定分区下的需要的首件仓
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    public IReadOnlyList<PayloadFirstSiloSummary> PartitionPayloadFirstSiloSummaries(string partitionCode) => DutyRacks
        .SelectMany(x => x.PartitionAvailableLocations(partitionCode))
        .Where(x => x.ContainsFirst)
        .SelectMany(x => x.FirstItemCodes)
        .Where(x => x.Any())
        .GroupBy(x => x)
        .Select(x => new PayloadFirstSiloSummary
        {
            ItemCode = x.Key,
            SiloCodes = DutyRacks.SelectMany(x => x.PartitionAvailableLocations(partitionCode))
                        .Where(y => y.FirstItemCodes.Contains(x.Key))
                        .Select(y => y.SiloCode)
                        .ToList()
                        .AsReadOnly(),
            SiloCount = DutyRacks.SelectMany(x => x.PartitionAvailableLocations(partitionCode))
                        .Where(y => y.FirstItemCodes.Contains(x.Key))
                        .Select(y => y.SiloCode)
                        .ToList()
                        .Count(),
            LocatioCodes = DutyRacks.SelectMany(x => x.PartitionAvailableLocations(partitionCode))
                        .Where(y => y.FirstItemCodes.Contains(x.Key))
                        .Select(y => y.Code)
                        .ToList()
                        .AsReadOnly(),
            EmptyLayerCounts = DutyRacks.SelectMany(x => x.PartitionAvailableLocations(partitionCode))
                        .Where(y => y.FirstItemCodes.Contains(x.Key))
                        .Select(x => x.EmptySiloBoxCount)
                        .ToList()
                        .AsReadOnly()
        })
        .ToList()
        .AsReadOnly();

    #endregion 指定分区下的负载的料仓进行summary

    public abstract Task Fetch();
}
