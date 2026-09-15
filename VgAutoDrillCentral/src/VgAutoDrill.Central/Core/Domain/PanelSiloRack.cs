using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mysql;
using VgAutoDrill.Central.Core.Schedule;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Domain;

/// <summary>
/// 板料架子，放板料仓
/// </summary>
public class PanelSiloRack : DeviceProxy
{
    private readonly IDrillManager _drillManager;
    private readonly ILocationManager _locationManager;
    private readonly MysqlTaskSchedulerOptions _taskScheduleOptions;
    private readonly ILogger<PanelSiloRack> _logger;

    [ActivatorUtilitiesConstructor]
    public PanelSiloRack(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _taskScheduleOptions = serviceProvider.GetRequiredService<IOptions<MysqlTaskSchedulerOptions>>().Value;
        _drillManager = serviceProvider.GetRequiredService<IDrillManager>();
        _locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<PanelSiloRack>();
    }

    #region Fields

    /// <summary>
    /// 已经被分配的库位
    /// </summary>
    public IReadOnlyList<Location> AllocatedLocations => _locationManager.AllocatedLocations
                                                    .Where(x => x.HostDevice?.DeviceId == this.DeviceId)
                                                    .ToList()
                                                    .AsReadOnly();

    /// <summary>
    /// 可用的库位，发出了调度任务，没有被预约的库位
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<Location> AvailableLocations => _locationManager.AvailableLocations
                                                    .Where(x => x.HostDevice?.DeviceId == this.DeviceId)
                                                    .ToList()
                                                    .AsReadOnly();

    /// <summary>
    /// don't suggest to use, use AvailableLocations
    /// </summary>
    public IReadOnlyList<Location> Locations => _locationManager.Locations
                                                    .Where(x => x.HostDevice?.DeviceId == this.DeviceId)
                                                    .ToList()
                                                    .AsReadOnly();

    public IReadOnlyList<Location> RunningLocations => _locationManager.RunningLocations
                                                    .Where(x => x.HostDevice?.DeviceId == this.DeviceId)
                                                    .ToList()
                                                    .AsReadOnly();

    public Location? this[int index] => Locations.FirstOrDefault(x => x.Index == index);

    public Location? this[string locationCode] => Locations.FirstOrDefault(x => x.Code == locationCode);

    #endregion Fields

    #region AllocatedPartitionLocations

    /// <summary>
    /// 已经被分区分配的库位
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <returns></returns>
    public IReadOnlyList<Location> AllocatedPartitionLocations(string partitionCode)
    {
        return string.IsNullOrEmpty(partitionCode)
            ? AllocatedLocations.ToList().AsReadOnly()
            : AllocatedLocations.Where(x => x.PartitionCode == partitionCode).ToList().AsReadOnly();
    }

    /// <summary>
    /// 已经被分配的库位
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <returns></returns>
    public IReadOnlyList<Location> AllocatedPartitionLocations(string partitionCode, string[] excludedLocations)
    {
        return excludedLocations.Any() ? AllocatedPartitionLocations(partitionCode)
            .Where(x => !excludedLocations.Contains(x.Code)).ToList().AsReadOnly() : AllocatedPartitionLocations(partitionCode);
    }

    #endregion AllocatedPartitionLocations

    #region AvailablePartitionLocations

    /// <summary>
    /// 分区库位
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <returns></returns>
    public IReadOnlyList<Location> PartitionLocations(string partitionCode)
        => string.IsNullOrEmpty(partitionCode) ? Locations : Locations
            .Where(x => string.IsNullOrEmpty(partitionCode) || string.Compare(x.PartitionCode, partitionCode, StringComparison.OrdinalIgnoreCase) == 0)
            .ToList()
            .AsReadOnly();

    /// <summary>
    /// 可用的库位，发出了调度任务，没有被预约的库位
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <returns></returns>
    public IReadOnlyList<Location> PartitionAvailableLocations(string partitionCode)
    {
        return AvailablePartitionLocations(partitionCode, Array.Empty<string>());
    }

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
    /// 可用的库位，发出了调度任务，没有被预约的库位
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <returns></returns>
    public IReadOnlyList<Location> AvailablePartitionLocations(string partitionCode, string[] excludedLocations)
    {
        return AvailablePartitionLocations(partitionCode, Array.Empty<string>(), excludedLocations);
    }

    /// <summary>
    /// 可用的库位，发出了调度任务，没有被预约的库位
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <param name="routeCodes">工艺路线</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <returns></returns>
    public IReadOnlyList<Location> AvailablePartitionLocations(string partitionCode, IReadOnlyList<string> routeCodes, string[] excludedLocations)
    {
        return excludedLocations.Any() ? PartitionAvailableLocations(partitionCode, routeCodes)
            .Where(x => !excludedLocations.Contains(x.Code)).ToList().AsReadOnly() : PartitionAvailableLocations(partitionCode, routeCodes);
    }

    #endregion AvailablePartitionLocations

    #region TryFindEmptySiloBoxLocation

    /// <summary>
    /// 从板料架子 找到 一个可以导出的空盒子的库位
    /// </summary>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindEmptySiloBoxLocation(out Location? outboundingLocation)
    {
        return TryFindEmptySiloBoxLocation(string.Empty, out outboundingLocation);
    }

    /// <summary>
    /// 从板料架子指定分区上 找到 一个可以导出的空盒子的库位
    /// </summary>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindEmptySiloBoxLocation(string partitionCode, out Location? outboundingLocation)
    {
        return TryFindEmptySiloBoxLocation(partitionCode, new string[] { }, out outboundingLocation);
    }

    /// <summary>
    /// 从板料架子指定分区上 找到 一个可以导出的空盒子的库位
    /// </summary>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindEmptySiloBoxLocation(string partitionCode, string[] excludedLocations, out Location? outboundingLocation)
    {
        var locations = AvailablePartitionLocations(partitionCode, excludedLocations);
        if (locations.Count == 0)
        {
            outboundingLocation = null;
            return false;
        }

        var location = locations.Where(x => x.CanUnloadPanelSilo && x.IsEmptySiloBox)
                                .OrderBy(x => x.Index)
                                .FirstOrDefault();
        outboundingLocation = location;
        return true;
    }

    #endregion TryFindEmptySiloBoxLocation

    #region TryFindLocation

    /// <summary>
    /// 从板料架子 找到 一个满足钻机需求的盒子的库位
    /// </summary>
    /// <param name="scheduleRequirement">调度任务需求汇总</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindLocation(ScheduleRequirement scheduleRequirement, out Location? outboundingLocation)
    {
        return TryFindLocation(scheduleRequirement, string.Empty, new string[] { }, out outboundingLocation);
    }

    /// <summary>
    /// 从板料架子 找到 一个满足钻机需求的盒子的库位
    /// </summary>
    /// <param name="scheduleRequirement">调度任务需求汇总</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindLocation(ScheduleRequirement scheduleRequirement, string[] excludedLocations, out Location? outboundingLocation)
    {
        return TryFindLocation(scheduleRequirement, string.Empty, excludedLocations, out outboundingLocation);
    }

    /// <summary>
    /// 从板料架子指定分区上 找到 一个满足钻机需求的盒子的库位
    /// </summary>
    /// <param name="requirement">调度任务需求汇总</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindLocation(ScheduleRequirement requirement, string partitionCode, string[] excludedLocations, out Location? outboundingLocation)
    {
        return TryFindLocation(requirement, partitionCode, new string[] { }, excludedLocations, out outboundingLocation);
    }

    /// <summary>
    /// 从板料架子指定分区上 找到 一个满足钻机需求的盒子的库位
    /// </summary>
    /// <param name="requirement">调度任务需求汇总</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="routeCodes">工艺路线</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindLocation(ScheduleRequirement requirement, string partitionCode, IReadOnlyList<string> routeCodes, string[] excludedLocations, out Location? outboundingLocation)
    {
        if (TryFindHasUndrilledPanelsLocation(requirement, partitionCode, routeCodes, excludedLocations, out outboundingLocation)
                    || TryFindCanAcceptDrilledPanelsLocation(requirement, partitionCode, routeCodes, excludedLocations, out outboundingLocation))
            return true;

        outboundingLocation = null;
        return false;

        //switch (requirement.InteractionSequence)
        //{
        //    case InteractionSequence.LoadOnly:
        //        if (!string.IsNullOrEmpty(requirement.RequireUndrilledItemCode)
        //            && TryFindHasUndrilledPanelsLocation(requirement, partitionCode, routeCodes, excludedLocations, out outboundingLocation))
        //            return true;

        //        outboundingLocation = null;
        //        return false;

        //    case InteractionSequence.LoadThenUnload:
        //    case InteractionSequence.UnloadThenLoad:
        //        if (TryFindHasUndrilledPanelsLocation(requirement, partitionCode, routeCodes, excludedLocations, out outboundingLocation)
        //            || TryFindCanAcceptDrilledPanelsLocation(requirement, partitionCode, routeCodes, excludedLocations, out outboundingLocation))
        //            return true;

        //        outboundingLocation = null;
        //        return false;

        //    case InteractionSequence.UnloadOnly:
        //        if (!string.IsNullOrWhiteSpace(requirement.RequireDrilledItemCode)
        //            && TryFindCanAcceptDrilledPanelsLocation(requirement, partitionCode, routeCodes, excludedLocations, out outboundingLocation))
        //            return true;

        //        outboundingLocation = null;
        //        return false;

        //    default:
        //        outboundingLocation = null;
        //        return false;
        //}
    }

    /// <summary>
    /// 从板料架子 找到 未指定需求的盒子的库位
    /// </summary>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindLocation(string[] excludedLocations, out Location? outboundingLocation)
    {
        return TryFindLocation(string.Empty, excludedLocations, out outboundingLocation);
    }

    public bool TryFindLocation(string partitionCode, out Location? outboundingLocation)
    {
        return TryFindLocation(string.Empty, new string[] { }, out outboundingLocation);
    }

    /// <summary>
    /// 从板料架子指定分区上 找到 未指定需求的盒子的库位
    /// </summary>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindLocation(string partitionCode, string[] excludedLocations, out Location? outboundingLocation)
    {
        var locations = AvailablePartitionLocations(partitionCode, excludedLocations);
        if (locations.Count == 0)
        {
            outboundingLocation = null;
            return false;
        }

        var unloadLocation = locations.FirstOrDefault();

        if (unloadLocation != null)
        {
            outboundingLocation = unloadLocation;
            return true;
        }

        outboundingLocation = null;
        return false;
    }

    #endregion TryFindLocation

    #region TryFindFullDrilledLocation

    /// <summary>
    /// 从板料架子指定分区上 找到一个装满熟料的料仓的库位
    /// </summary>
    /// <param name="parameters">系统配置参数</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindFullDrilledLocation(ConfigParameters parameters, string partitionCode, string[] excludedLocations, out Location? outboundingLocation)
    {
        var locations = AvailablePartitionLocations(partitionCode, excludedLocations);
        if (locations.Count == 0)
        {
            outboundingLocation = null;
            return false;
        }
        var minDrilledTrackOutNum = parameters.MinDrilledTrackOutNum;
        if (minDrilledTrackOutNum == 0) { minDrilledTrackOutNum = 5; }

        var autoTrackOutSilo = parameters.AutoDrilledTrackOutSilo;

        var drillPayloadItemCodes = _drillManager.PartitionPayloadPanelSummaries(partitionCode).Select(x => x.ItemCode).ToList();
        Predicate<Location> siloIsFull = x => x.DrilledPanelsCount >= minDrilledTrackOutNum;
        Predicate<Location> notRequiredByDrill = x => !drillPayloadItemCodes.Intersect(x.DrilledItemCodes).Any();
        Predicate<Location> idleTimeout = x => (DateTime.Now - x.Schedule!.CreateTime).TotalSeconds > parameters.DrilledItemOccupyLocationIdleTimeout * 60;
        Predicate<Location> noEnoughLocation = x => parameters.CountEmptyPayloadForkNow <= x.Partition?.MinEmptyLocationNum;

        var sortLocations = locations.OrderBy(x => x.DrilledItemCodes.FirstOrDefault(t => !string.IsNullOrWhiteSpace(t))).ToList().AsReadOnly();

        var unloadLocation = sortLocations.FirstOrDefault(x =>
                                                x.CanUnloadPanelSilo
                                                && (siloIsFull(x) || notRequiredByDrill(x) || noEnoughLocation(x) || idleTimeout(x))
                                                && x.ContainsDrilled
                                                && !x.ContainsFirst
                                                && !x.ContainsUndrilled);

        if (unloadLocation != null)
        {
            outboundingLocation = unloadLocation;
            return true;
        }

        outboundingLocation = null;
        return false;
    }

    #endregion TryFindFullDrilledLocation

    #region TryFindIdleTimeoutDrilledLocation

    /// <summary>
    /// 从板料架子找到一个空闲超过指定时长的熟料库位
    /// </summary>
    /// <param name="parameters">系统配置参数</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindIdleTimeoutDrilledLocation(ConfigParameters parameters, string partitionCode, string[] excludedLocations, out Location? outboundingLocation)
    {
        var locations = AvailablePartitionLocations(partitionCode, excludedLocations);
        if (locations.Count == 0)
        {
            outboundingLocation = null;
            return false;
        }

        Predicate<Location> idleTimeout = x => (DateTime.Now - x.Schedule!.CreateTime).TotalSeconds > parameters.DrilledItemOccupyLocationIdleTimeout * 60;
        Predicate<Location> notRequiredByDrill = x => !_drillManager.PartitionRequiredDrilledItemCodes(partitionCode).Intersect(x.DrilledItemCodes).Any();

        var unloadLocation = locations.FirstOrDefault(x =>
                                                x.CanUnloadPanelSilo
                                                && (notRequiredByDrill(x) || idleTimeout(x))
                                                && x.ContainsDrilled
                                                && x.EmptySiloBoxCount > 1
                                                && !x.ContainsUndrilled);

        if (unloadLocation != null)
        {
            outboundingLocation = unloadLocation;
            return true;
        }

        outboundingLocation = null;
        return false;
    }

    #endregion TryFindIdleTimeoutDrilledLocation

    #region TryFindIdleTimeoutUndrilledLocation

    /// <summary>
    /// 从板料架子找到一个空闲超过指定时长的生料库位
    /// </summary>
    /// <param name="parameters">系统配置参数</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindIdleTimeoutUndrilledLocation(ConfigParameters parameters, string partitionCode, string[] excludedLocations, out Location? outboundingLocation)
    {
        var locations = AvailablePartitionLocations(partitionCode, excludedLocations);
        if (locations.Count == 0)
        {
            outboundingLocation = null;
            return false;
        }

        Predicate<Location> idleTimeout = x => TimeSpan.FromTicks(DateTime.Now.Ticks - x.Schedule!.CreateTime.Ticks).TotalSeconds > parameters.UndrilledItemOccupyLocationIdleTimeout;
        Predicate<Location> notRequiredByDrill = x => !_drillManager.PartitionRequiredUndrilledItemCodes(partitionCode).Intersect(x.UndrilledItemCodes).Any();

        var unloadLocation = locations.FirstOrDefault(x =>
                                                x.CanUnloadPanelSilo
                                                && idleTimeout(x)
                                                && notRequiredByDrill(x)
                                                && x.ContainsUndrilled);

        _logger.LogInformation($"TryFindIdleTimeoutUndrilledLocation: PartitionRequiredUndrilledItemCodes: {JsonSerializer.Serialize(_drillManager.PartitionRequiredUndrilledItemCodes(partitionCode))}, partitionCode:{partitionCode},  ConfigParameters:{JsonSerializer.Serialize(parameters)}");

        if (unloadLocation != null)
        {
            outboundingLocation = unloadLocation;
            return true;
        }

        outboundingLocation = null;
        return false;
    }

    #endregion TryFindIdleTimeoutUndrilledLocation

    #region TryFindFirstDrilledLocation

    /// <summary>
    ///从板料架子找到一个钻完板的,首件的料仓库位
    /// </summary>
    /// <param name="parameters">系统配置参数</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindFirstDrilledLocation(ConfigParameters parameters, out Location? outboundingLocation)
    {
        return TryFindFirstDrilledLocation(parameters, string.Empty, new string[] { }, out outboundingLocation);
    }

    /// <summary>
    ///从板料架子找到一个钻完板的,首件的料仓库位
    /// </summary>
    /// <param name="parameters">系统配置参数</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindFirstDrilledLocation(ConfigParameters parameters, string partitionCode, string[] excludedLocations, out Location? outboundingLocation)
    {
        var locations = AvailablePartitionLocations(partitionCode, excludedLocations);
        if (locations.Count == 0)
        {
            outboundingLocation = null;
            return false;
        }

        var unloadLocation = locations.FirstOrDefault(x =>
                        x.CanUnloadPanelSilo
                        && x.ContainsFirst
                        && x.FirstPanelsCount >= parameters.MinFirstDrilledTrackOutNum
                        && !x.ContainsUndrilled
                        && !x.ContainsDrilled);

        if (unloadLocation != null)
        {
            outboundingLocation = unloadLocation;
            return true;
        }

        outboundingLocation = null;
        return false;
    }

    #endregion TryFindFirstDrilledLocation

    #region TryFindTimeoutUnloadingLocation

    /// <summary>
    /// 从板料架子找到一个要卸载料仓超时的料仓的库位
    /// </summary>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindTimeoutUnloadingLocation(out Location? outboundingLocation)
    {
        return TryFindTimeoutUnloadingLocation(string.Empty, Array.Empty<string>(), out outboundingLocation);
    }

    /// <summary>
    /// 从板料架子指定分区上 找到一个要卸载料仓超时的料仓的库位
    /// </summary>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindTimeoutUnloadingLocation(string partitionCode, string[] excludedLocations, out Location? outboundingLocation)
    {
        var locations = AvailablePartitionLocations(partitionCode, excludedLocations);
        if (locations.Count == 0)
        {
            outboundingLocation = null;
            return false;
        }

        Predicate<Location> timeout = x => x.CanUnloadPanelSilo && DateTime.Now.Subtract(x.Schedule.CreateTime).TotalSeconds > _taskScheduleOptions.ForkTimeoutSeconds;
        var unloadLocation = locations.FirstOrDefault(x => timeout(x));

        if (unloadLocation != null)
        {
            outboundingLocation = unloadLocation;
            return true;
        }

        outboundingLocation = null;
        return false;
    }

    #endregion TryFindTimeoutUnloadingLocation

    #region TryFindNoDrillRequiredUndrilledLocation

    /// <summary>
    /// 从板料架子 找到找到钻机当前不需要的生料仓的库位
    /// </summary>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindNoDrillRequiredUndrilledLocation(out Location? outboundingLocation)
    {
        return TryFindNoDrillRequiredUndrilledLocation(string.Empty, Array.Empty<string>(), out outboundingLocation);
    }

    /// <summary>
    /// 从板料架子指定分区上 找到找到钻机当前不需要的生料仓的库位
    /// </summary>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindNoDrillRequiredUndrilledLocation(string partitionCode, string[] excludedLocations, out Location? outboundingLocation)
    {
        var locations = AvailablePartitionLocations(partitionCode, excludedLocations);
        if (locations.Count == 0)
        {
            outboundingLocation = null;
            return false;
        }

        var undrilledItemCodes = _drillManager.RequiredUndrilledItemCodes;
        Predicate<Location> noDrillRequiredUndrilled = x => undrilledItemCodes.Any()
                                                        && !x.ContainsUndrilledItemCodes(undrilledItemCodes)
                                                        && x.CanUnloadPanelSilo
                                                        && DateTime.Now.Subtract(x.Schedule.CreateTime).TotalSeconds > _taskScheduleOptions.ForkUndrilledWaitingTimeoutSeconds;

        var unloadLocation = locations.FirstOrDefault(x => noDrillRequiredUndrilled(x));

        if (unloadLocation != null)
        {
            outboundingLocation = unloadLocation;
            return true;
        }

        outboundingLocation = null;
        return false;
    }

    #endregion TryFindNoDrillRequiredUndrilledLocation

    #region TryFindEmptyPayloadLocation

    /// <summary>
    /// 从板料架子 找到 一个空位置的库位
    /// </summary>
    /// <param name="minEmptyPayloadNum">最少空库位数</param>
    /// <param name="inboundingLocation"></param>
    /// <returns></returns>
    public bool TryFindEmptyPayloadLocation(int minEmptyPayloadNum, out Location? inboundingLocation)
    {
        return TryFindEmptyPayloadLocation(minEmptyPayloadNum, string.Empty, Array.Empty<string>(), out inboundingLocation);
    }

    /// <summary>
    /// 从板料架子指定分区上 找到 一个空位置的库位
    /// </summary>
    /// <param name="minEmptyPayloadNum">最少空库位数</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="inboundingLocation"></param>
    /// <returns></returns>
    public bool TryFindEmptyPayloadLocation(int minEmptyPayloadNum, string partitionCode, string[] excludedLocations, out Location? inboundingLocation)
    {
        return TryFindEmptyPayloadLocation(minEmptyPayloadNum, partitionCode, Array.Empty<string>(), excludedLocations, out inboundingLocation);
    }

    /// <summary>
    /// 从板料架子指定分区上 找到 一个空位置的库位
    /// </summary>
    /// <param name="minEmptyPayloadNum">最少空库位数</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="routeCodes">工艺路线</param>
    /// <param name="inboundingLocation"></param>
    /// <returns></returns>
    public bool TryFindEmptyPayloadLocation(int minEmptyPayloadNum, string partitionCode, IReadOnlyList<string> routeCodes, string[] excludedLocations, out Location? inboundingLocation)
    {
        var locations = AvailablePartitionLocations(partitionCode, routeCodes, excludedLocations);
        if (locations.Count == 0)
        {
            inboundingLocation = null;
            return false;
        }

        //if (minEmptyPayloadNum < 2)
        //    minEmptyPayloadNum = 1;

        if (locations.Count(x => x.CanLoadPanelSilo) <= minEmptyPayloadNum)
        {
            inboundingLocation = null;
            return false;
        }

        var location = locations.OrderBy(x => x.Index).FirstOrDefault(x => x.CanLoadPanelSilo);
        inboundingLocation = location;
        return true;
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
        return TryFindAllocatedLocation(agvKind, string.Empty, Array.Empty<string>(), out outboundingLocation);
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
        return TryFindAllocatedLocation(agvKind, partitionCode, Array.Empty<string>(), out outboundingLocation);
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
        var locations = AllocatedPartitionLocations(partitionCode, excludedLocations);
        if (locations.Count == 0)
        {
            outboundingLocation = null;
            return false;
        }

        outboundingLocation = locations.Where(location => location.Schedule != null
            && location.AllocatedAgv != null
            && location.AllocatedAgv.DeviceKind == agvKind).FirstOrDefault();

        if (outboundingLocation != null)
        {
            return true;
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
        var locations = AllocatedPartitionLocations(partitionCode, excludedLocations);
        if (locations.Count == 0)
        {
            outboundingLocation = null;
            return false;
        }

        outboundingLocation = locations.Where(location => location.Schedule != null && location.Schedule.AllocatedAgv == agvId).FirstOrDefault();
        if (outboundingLocation != null)
        {
            return true;
        }

        outboundingLocation = null;
        return false;
    }

    #endregion TryFindAllocatedLocation

    #region TryFindCanAcceptDrilledPanelsLocation

    /// <summary>
    /// 根据钻机需求找到能下熟料的库位
    /// </summary>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindCanAcceptDrilledPanelsLocation(ScheduleRequirement requirement, string partitionCode, string[] excludedLocations, out Location? outboundingLocation)
    {
        return TryFindCanAcceptDrilledPanelsLocation(requirement, partitionCode, Array.Empty<string>(), excludedLocations, out outboundingLocation);
    }

    /// <summary>
    /// 根据钻机需求找到能下熟料的库位
    /// </summary>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="routeCodes">工艺路线</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindCanAcceptDrilledPanelsLocation(ScheduleRequirement requirement, string partitionCode, IReadOnlyList<string> routeCodes, string[] excludedLocations, out Location? outboundingLocation)
    {
        var locations = AvailablePartitionLocations(partitionCode, routeCodes, excludedLocations);
        if (locations.Count == 0)
        {
            outboundingLocation = null;
            return false;
        }

        //仅上料时，不查找下熟料的库位
        if (requirement.InteractionSequence == InteractionSequence.LoadOnly)
        {
            outboundingLocation = null;
            return false;
        }

        var unloadLocation = locations.Where(x => x.CanAcceptDrilledItems(requirement) && x.CanUnloadPanelSilo)
                                      .OrderByDescending(x => x.Match(requirement))
                                      .FirstOrDefault();

        if (unloadLocation != null)
        {
            outboundingLocation = unloadLocation;
            return true;
        }

        outboundingLocation = null;
        return false;
    }

    #endregion TryFindCanAcceptDrilledPanelsLocation

    #region TryFindHasUndrilledPanelsLocation

    /// <summary>
    /// 根据生料料号从板料架子上找到一个有生料的料仓的库位
    /// </summary>
    /// <param name="requiredUndrilledItemCode">需要的生料料号</param>
    /// <param name="requiredUndrilledItemQuantity">需要的生料数量</param>
    /// <param name="outboundingLocation"></param>
    /// <returns></returns>
    public bool TryFindHasUndrilledPanelsLocation(string requiredUndrilledItemCode, int requiredUndrilledItemQuantity, out Location? outboundingLocation)
    {
        var locations = AvailableLocations;
        if (locations.Count == 0)
        {
            outboundingLocation = null;
            return false;
        }

        var requirement = new ScheduleRequirement
        {
            RequireUndrilledItemCode = requiredUndrilledItemCode,
            RequireUndrilledItemQty = requiredUndrilledItemQuantity,
        };

        var unloadLocation = locations.Where(x => x.CanAcceptUndrilledItems(requiredUndrilledItemCode))
                                      .OrderBy(x => x.LoadPanelScore(requirement))
                                      .FirstOrDefault();
        if (unloadLocation != null)
        {
            outboundingLocation = unloadLocation;
            return true;
        }

        if (unloadLocation != null)
        {
            outboundingLocation = unloadLocation;
            return true;
        }

        outboundingLocation = null;
        return false;
    }

    /// <summary>
    /// 从板料架子指定分区上 找到一个有生料的料仓的库位
    /// </summary>
    /// <param name="requirement">调度任务需求汇总</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindHasUndrilledPanelsLocation(ScheduleRequirement requirement, string partitionCode, string[] excludedLocations, out Location? outboundingLocation)
    {
        return TryFindHasUndrilledPanelsLocation(requirement, partitionCode, Array.Empty<string>(), excludedLocations, out outboundingLocation);
    }

    /// <summary>
    /// 从板料架子指定分区上 找到一个有生料的料仓的库位
    /// </summary>
    /// <param name="requirement">调度任务需求汇总</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="routeCodes">工艺路线</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    public bool TryFindHasUndrilledPanelsLocation(ScheduleRequirement requirement, string partitionCode, IReadOnlyList<string> routeCodes, string[] excludedLocations, out Location? outboundingLocation)
    {
        var locations = AvailablePartitionLocations(partitionCode, routeCodes, excludedLocations);
        if (locations.Count == 0)
        {
            outboundingLocation = null;
            return false;
        }

        var unloadLocation = locations.Where(x => x.CanAcceptUndrilledItems(requirement))
                                      .OrderByDescending(x => x.Match(requirement))
                                      .FirstOrDefault();
        if (unloadLocation != null)
        {
            outboundingLocation = unloadLocation;
            return true;
        }

        if (unloadLocation != null)
        {
            outboundingLocation = unloadLocation;
            return true;
        }

        outboundingLocation = null;
        return false;
    }

    #endregion TryFindHasUndrilledPanelsLocation

    public override string GetLocationCode(int position) => $"{DeviceId}{position.ToString().PadLeft(3, '0')}";
}
