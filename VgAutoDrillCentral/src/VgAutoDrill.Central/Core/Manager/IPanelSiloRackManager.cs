using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Schedule;
using VgAutoDrill.Central.Core.Schedule.Summary;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Manager;

/// <summary>
/// 板料架子 管理者
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TScheduleTask"></typeparam>
public interface IPanelSiloRackManager<T, TScheduleTask>
    where T : PanelSiloRack
    where TScheduleTask : ScheduleTaskWithRequest
{
    /// <summary>
    /// 执行任务的货架
    /// </summary>
    IReadOnlyList<PanelSiloRack> DutyRacks { get; }

    /// <summary>
    /// 所有架子，包括料架和插齿
    /// </summary>
    IReadOnlyList<PanelSiloRack> PanelSiloRacks { get; }

    /// <summary>
    /// 所有料架
    /// </summary>
    IReadOnlyList<PanelSiloShelf> PanelSiloShelfs { get; }

    /// <summary>
    /// 所有插齿
    /// </summary>
    IReadOnlyList<PanelSiloFork> PanelSiloForks { get; }

    /// <summary>
    /// 熟料汇总
    /// </summary>
    IReadOnlyList<DrilledItemSummary> DrilledItemSummaries { get; }

    /// <summary>
    /// 生料汇总
    /// </summary>
    IReadOnlyList<UndrilledItemSummary> UndrilledItemSummaries { get; }

    /// <summary>
    /// 首件汇总
    /// </summary>
    IReadOnlyList<FirstItemSummary> FirstItemSummaries { get; }

    IReadOnlyList<TScheduleTask> NotStartedSchedules { get; }

    /// <summary>
    /// 所有可用库位
    /// </summary>
    IReadOnlyList<Location> AvailableLocations { get; }

    /// <summary>
    /// 所有库位
    /// </summary>
    IReadOnlyList<Location> EnabledLocations { get; }

    /// <summary>
    /// 存在未完成的库位
    /// </summary>
    IReadOnlyList<Location> NotStartedLocations { get; }

    /// <summary>
    /// 存在未分配agv的库位
    /// </summary>
    IReadOnlyList<Location> NotAllocatedLocations { get; }

    /// <summary>
    /// 库位数
    /// </summary>
    int AvailableLocationsCount();

    /// <summary>
    /// 料仓数
    /// </summary>
    int SiloCount();

    /// <summary>
    /// 当前空库位数
    /// </summary>
    int EmptyPayloadNowCount { get; }

    /// <summary>
    /// 当前空仓数
    /// </summary>
    int EmptySiloBoxNowCount { get; }

    /// <summary>
    /// 即将空库位数
    /// </summary>
    int EmptyPayloadSoonCount { get; }

    /// <summary>
    /// 即将空仓数
    /// </summary>
    int EmptySiloBoxSoonCount { get; }

    IReadOnlyList<Partition?> Partitions { get; }

    Task Fetch();

    #region summary

    /// <summary>
    /// 所有料架或插齿上的板料汇总
    /// </summary>
    /// <returns></returns>
    int PanelCount();

    /// <summary>
    /// 某个料架或插齿上的板料汇总
    /// </summary>
    /// <param name="deviceId">料架或插齿的设备id</param>
    /// <returns></returns>
    int PanelCount(string deviceId);

    /// <summary>
    /// 所有料架或插齿上的某些状态的板料汇总
    /// </summary>
    /// <param name="productStatuses">物料状态</param>
    /// <returns></returns>
    int PanelCount(IReadOnlyList<ProductStatus> productStatuses);

    /// <summary>
    /// 某个料架或插齿上的某些状态的板料汇总
    /// </summary>
    /// <param name="deviceId">料架或插齿的设备id</param>
    /// <param name="productStatuses">物料状态</param>
    /// <returns></returns>
    int PanelCount(string deviceId, IReadOnlyList<ProductStatus> productStatuses);

    /// <summary>
    /// 统计包含所给物料状态的库位数量
    /// </summary>
    /// <param name="productStatuses">所给物料状态</param>
    /// <returns></returns>
    int AvailableLocationsCount(IReadOnlyList<ProductStatus> productStatuses);

    /// <summary>
    /// 某个库位上的某些状态的板料汇总
    /// </summary>
    /// <param name="locationCode">库位</param>
    /// <param name="productStatuses">物料状态</param>
    /// <returns></returns>
    int LocationPanelCount(string locationCode, IReadOnlyList<ProductStatus> productStatuses);

    /// <summary>
    /// 可用的库位，发出了调度任务，没有被预约的库位
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <returns></returns>
    IReadOnlyList<Location> PartitionAvailableLocations(string partitionCode);

    /// <summary>
    /// 分区库位
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <returns></returns>
    IReadOnlyList<Location> PartitionLocations(string partitionCode);

    /// <summary>
    /// 分区当前空库位数
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <returns></returns>
    int PartitionEmptyPayloadNowCount(string partitionCode);

    /// <summary>
    /// 分区当前空仓数
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <returns></returns>
    int PartitionEmptySiloBoxNowCount(string partitionCode);

    /// <summary>
    /// 分区即将空库位数
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <returns></returns>
    int PartitionEmptyPayloadSoonCount(string partitionCode);

    /// <summary>
    /// 分区即将空仓数
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <returns></returns>
    int PartitionEmptySiloBoxSoonCount(string partitionCode);

    IReadOnlyList<DrilledItemSummary> PartitionDrilledItemSummaries(string partitionCode);

    IReadOnlyList<UndrilledItemSummary> PartitionUndrilledItemSummaries(string partitionCode);

    IReadOnlyList<FirstItemSummary> PartitionFirstItemSummaries(string partitionCode);

    IReadOnlyList<Location> PartitionEnabledLocations(string partitionCode);

    IReadOnlyList<Location> PartitionAvailableLocations(string partitionCode, IReadOnlyList<string> routeCodes);

    #endregion summary

    #region TryFindEmptyPayloadLocation

    /// <summary>
    /// 从板料架子 找到 一个空位置的库位
    /// </summary>
    /// <param name="minEmptyPayloadNum">最少空库位数</param>
    /// <param name="inboundingLocation">上料库位</param>
    /// <returns></returns>
    bool TryFindEmptyPayloadLocation(int minEmptyPayloadNum, out Location? inboundingLocation);

    /// <summary>
    /// 从板料架子 找到 一个空位置的库位;空库位数少于minEmptyPayloadNum，将返回空
    /// </summary>
    /// <param name="minEmptyPayloadNum">最少空库位数</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="inboundingLocation">上料库位</param>
    /// <returns></returns>
    bool TryFindEmptyPayloadLocation(int minEmptyPayloadNum, string partitionCode, out Location? inboundingLocation);

    /// <summary>
    /// 从板料架子 找到 一个空位置的库位;空库位数少于minEmptyPayloadNum，将返回空
    /// </summary>
    /// <param name="minEmptyPayloadNum">最少空库位数</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="inboundingLocation">上料库位</param>
    /// <returns></returns>
    bool TryFindEmptyPayloadLocation(int minEmptyPayloadNum, string partitionCode, string[] excludedLocations, out Location? inboundingLocation);

    /// <summary>
    /// 从板料架子 找到该工艺路线上 一个空位置的库位;空库位数少于1，将返回空
    /// </summary>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="routeCodes">工艺路线</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="inboundingLocation">上料库位</param>
    /// <returns></returns>
    bool TryFindEmptyPayloadLocation(string partitionCode, IReadOnlyList<string> routeCodes, out Location? inboundingLocation);

    /// <summary>
    /// 从板料架子 找到该工艺路线上 一个空位置的库位;空库位数少于minEmptyPayloadNum，将返回空
    /// </summary>
    /// <param name="minEmptyPayloadNum">最少空库位数</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="routeCodes">工艺路线</param>
    /// <param name="inboundingLocation">上料库位</param>
    /// <returns></returns>
    bool TryFindEmptyPayloadLocation(int minEmptyPayloadNum, string partitionCode, IReadOnlyList<string> routeCodes, out Location? inboundingLocation);

    /// <summary>
    /// 从板料架子 找到该工艺路线上 一个空位置的库位;空库位数少于minEmptyPayloadNum，将返回空
    /// </summary>
    /// <param name="minEmptyPayloadNum">最少空库位数</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="routeCodes">工艺路线</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="inboundingLocation">上料库位</param>
    /// <returns></returns>
    bool TryFindEmptyPayloadLocation(int minEmptyPayloadNum, string partitionCode, IReadOnlyList<string> routeCodes, string[] excludedLocations, out Location? inboundingLocation);

    #endregion TryFindEmptyPayloadLocation

    #region TryFindEmptySiloBoxLocation

    /// <summary>
    /// 从板料架子 找到 一个可以导出的空盒子的库位
    /// </summary>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindEmptySiloBoxLocation(out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到 一个可以导出的空盒子的库位
    /// </summary>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindEmptySiloBoxLocation(string partitionCode, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到 一个可以导出的空盒子的库位
    /// </summary>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindEmptySiloBoxLocation(string[] excludedLocations, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到 一个可以导出的空盒子的库位
    /// </summary>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindEmptySiloBoxLocation(string partitionCode, string[] excludedLocations, out Location? outboundingLocation);

    #endregion TryFindEmptySiloBoxLocation

    #region TryFindFullDrilledLocation

    /// <summary>
    /// 从板料架子找到一个装满熟料的料仓，正在呼叫下料的料仓的库位
    /// </summary>
    /// <param name="parameters">系统配置参数</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindFullDrilledLocation(ConfigParameters parameters, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子找到一个装满熟料的料仓，正在呼叫下料的料仓的库位
    /// </summary>
    /// <param name="parameters">系统配置参数</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindFullDrilledLocation(ConfigParameters parameters, string[] excludedLocations, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上找到一个装满熟料的料仓，正在呼叫下料的料仓的库位
    /// </summary>
    /// <param name="parameters">系统配置参数</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindFullDrilledLocation(ConfigParameters parameters, string partitionCode, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上找到一个装满熟料的料仓，正在呼叫下料的料仓的库位
    /// </summary>
    /// <param name="parameters">系统配置参数</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindFullDrilledLocation(ConfigParameters parameters, string partitionCode, string[] excludedLocations, out Location? outboundingLocation);

    #endregion TryFindFullDrilledLocation

    #region TryFindIdleTimeoutUndrilledLocation

    /// <summary>
    /// 从板料架子找到一个空闲超过指定时长的生料库位
    /// </summary>
    /// <param name="parameters">系统配置参数</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindIdleTimeoutUndrilledLocation(ConfigParameters parameters, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子找到一个空闲超过指定时长的生料库位
    /// </summary>
    /// <param name="parameters">系统配置参数</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindIdleTimeoutUndrilledLocation(ConfigParameters parameters, string[] excludedLocations, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子找到一个空闲超过指定时长的生料库位
    /// </summary>
    /// <param name="parameters">系统配置参数</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindIdleTimeoutUndrilledLocation(ConfigParameters parameters, string partitionCode, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子找到一个空闲超过指定时长的生料库位
    /// </summary>
    /// <param name="parameters">系统配置参数</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindIdleTimeoutUndrilledLocation(ConfigParameters parameters, string partitionCode, string[] excludedLocations, out Location? outboundingLocation);

    #endregion TryFindIdleTimeoutUndrilledLocation

    #region TryFindFirstDrilledLocation

    /// <summary>
    /// 从板料架子找到一个钻完板的,首件的料仓库位
    /// </summary>
    /// <param name="parameters">系统配置参数</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindFirstDrilledLocation(ConfigParameters parameters, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子找到一个钻完板的,首件的料仓库位
    /// </summary>
    /// <param name="parameters">系统配置参数</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindFirstDrilledLocation(ConfigParameters parameters, string[] excludedLocations, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子找到一个钻完板的,首件的料仓库位
    /// </summary>
    /// <param name="parameters">系统配置参数</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindFirstDrilledLocation(ConfigParameters parameters, string partitionCode, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子找到一个钻完板的,首件的料仓库位
    /// </summary>
    /// <param name="parameters">系统配置参数</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindFirstDrilledLocation(ConfigParameters parameters, string partitionCode, string[] excludedLocations, out Location? outboundingLocation);

    #endregion TryFindFirstDrilledLocation

    #region TryFindTimeoutUnloadingLocation

    /// <summary>
    /// 从板料架子找到一个超时的正在呼叫下料的料仓的库位
    /// </summary>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindTimeoutUnloadingLocation(out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子找到一个超时的正在呼叫下料的料仓的库位
    /// </summary>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindTimeoutUnloadingLocation(string[] excludedLocations, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到一个超时的正在呼叫下料的料仓的库位
    /// </summary>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindTimeoutUnloadingLocation(string partitionCode, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到一个超时的正在呼叫下料的料仓的库位
    /// </summary>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindTimeoutUnloadingLocation(string partitionCode, string[] excludedLocations, out Location? outboundingLocation);

    #endregion TryFindTimeoutUnloadingLocation

    #region TryFindNoDrillRequiredUndrilledLocation

    /// <summary>
    /// 从板料架子 找到找到钻机当前不需要的生料仓
    /// </summary>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindNoDrillRequiredUndrilledLocation(out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子 找到找到钻机当前不需要的生料仓
    /// </summary>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindNoDrillRequiredUndrilledLocation(string[] excludedLocations, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到找到钻机当前不需要的生料仓
    /// </summary>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindNoDrillRequiredUndrilledLocation(string partitionCode, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到找到钻机当前不需要的生料仓
    /// </summary>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindNoDrillRequiredUndrilledLocation(string partitionCode, string[] excludedLocations, out Location? outboundingLocation);

    #endregion TryFindNoDrillRequiredUndrilledLocation

    #region TryFindLocation

    /// <summary>
    /// 从板料架子 找到 一个满足钻机需求的库位
    /// </summary>
    /// <param name="scheduleRequirement">调度任务需求汇总</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindLocation(ScheduleRequirement scheduleRequirement, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子 找到 一个满足钻机需求的库位
    /// </summary>
    /// <param name="scheduleRequirement">调度任务需求汇总</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindLocation(ScheduleRequirement scheduleRequirement, string[] excludedLocations, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到 一个满足钻机需求的库位
    /// </summary>
    /// <param name="scheduleRequirement">调度任务需求汇总</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindLocation(ScheduleRequirement scheduleRequirement, string partitionCode, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到 一个满足钻机需求的库位
    /// </summary>
    /// <param name="scheduleRequirement">调度任务需求汇总</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindLocation(ScheduleRequirement scheduleRequirement, string partitionCode, string[] excludedLocations, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到 一个满足钻机需求的库位
    /// </summary>
    /// <param name="scheduleRequirement">调度任务需求汇总</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="routeCodes">工艺路线</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindLocation(ScheduleRequirement scheduleRequirement, string partitionCode, IReadOnlyList<string> routeCodes, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子 找到 任意一个需求的库位
    /// </summary>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindLocation(out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到 任意一个需求的库位
    /// </summary>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindLocation(string partitionCode, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子 找到 任意一个需求的库位
    /// </summary>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindLocation(string[] excludedLocations, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到 任意一个需求的库位
    /// </summary>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindLocation(string partitionCode, string[] excludedLocations, out Location? outboundingLocation);

    bool TryFindLocation(ScheduleRequirement scheduleRequirement, string partitionCode, IReadOnlyList<string> routeCodes, string[] excludedLocations, out Location? outboundingLocation);

    #endregion TryFindLocation

    #region TryFindArbitraryDrillRequiredLocation

    /// <summary>
    /// 从板料架子 找到 任意一个钻机需求的库位
    /// </summary>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindArbitraryDrillRequiredLocation(out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到 任意一个钻机需求的库位
    /// </summary>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindArbitraryDrillRequiredLocation(string partitionCode, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到 任意一个钻机需求的库位
    /// </summary>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindArbitraryDrillRequiredLocation(string partitionCode, string[] excludedLocations, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到该工艺路线上 任意一个钻机需求的库位
    /// </summary>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="routeCodes">工艺路线</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindArbitraryDrillRequiredLocation(string partitionCode, IReadOnlyList<string> routeCodes, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到该工艺路线上 任意一个钻机需求的库位
    /// </summary>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="routeCodes">工艺路线</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindArbitraryDrillRequiredLocation(string partitionCode, IReadOnlyList<string> routeCodes, string[] excludedLocations, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到 任意一个钻机需求的库位
    /// </summary>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindArbitraryDrillRequiredLocation(string[] excludedLocations, out Location? outboundingLocation);

    #endregion TryFindArbitraryDrillRequiredLocation

    #region TryFindAllocatedLocation

    /// <summary>
    /// 从板料架子指定分区上 找到 已经分配给指定agv类型的库位
    /// </summary>
    /// <param name="agvKind">指定agv类型</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindAllocatedLocation(DeviceKind agvKind, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到 已经分配给指定agv类型的库位
    /// </summary>
    /// <param name="agvKind">指定agv类型</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindAllocatedLocation(DeviceKind agvKind, string[] excludedLocations, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到 已经分配给指定agv类型的库位
    /// </summary>
    /// <param name="agvKind">指定agv类型</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindAllocatedLocation(DeviceKind agvKind, string partitionCode, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到 已经分配给指定agv类型的库位
    /// </summary>
    /// <param name="agvKind">指定agv类型</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindAllocatedLocation(DeviceKind agvKind, string partitionCode, string[] excludedLocations, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到 已经分配给某个车的库位
    /// </summary>
    /// <param name="agvId">指定agv</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindAllocatedLocation(string agvId, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到 已经分配给某个车的库位
    /// </summary>
    /// <param name="agvId">指定agv</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindAllocatedLocation(string agvId, string[] excludedLocations, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到 已经分配给某个车的库位
    /// </summary>
    /// <param name="agvId">指定agv</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindAllocatedLocation(string agvId, string partitionCode, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到 已经分配给某个车的库位
    /// </summary>
    /// <param name="agvId">指定agv</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindAllocatedLocation(string agvId, string partitionCode, string[] excludedLocations, out Location? outboundingLocation);

    #endregion TryFindAllocatedLocation

    #region TryFindCanAcceptDrilledItemsLocation

    /// <summary>
    /// 从板料架子指定分区上 找到一个接受钻机下熟料需求的库位
    /// </summary>
    /// <param name="requirement">调度任务需求汇总</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindCanAcceptDrilledItemsLocation(ScheduleRequirement requirement, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到一个接受钻机下熟料需求的库位
    /// </summary>
    /// <param name="requirement">调度任务需求汇总</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindCanAcceptDrilledItemsLocation(ScheduleRequirement requirement, string partitionCode, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到一个接受钻机下熟料需求的库位
    /// </summary>
    /// <param name="requirement">调度任务需求汇总</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindCanAcceptDrilledItemsLocation(ScheduleRequirement requirement, string[] excludedLocations, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到一个接受钻机下熟料需求的库位
    /// </summary>
    /// <param name="requirement">调度任务需求汇总</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindCanAcceptDrilledItemsLocation(ScheduleRequirement requirement, string partitionCode, string[] excludedLocations, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到一个接受钻机下熟料需求的库位
    /// </summary>
    /// <param name="requirement">调度任务需求汇总</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="routeCodes">工艺路线</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindCanAcceptDrilledItemsLocation(ScheduleRequirement requirement, string partitionCode, IReadOnlyList<string> routeCodes, string[] excludedLocations, out Location? outboundingLocation);

    #endregion TryFindCanAcceptDrilledItemsLocation

    #region TryFindHasUndrilledPanelsLocation

    /// <summary>
    /// 根据生料料号从板料架子上找到一个有生料的料仓的库位
    /// </summary>
    /// <param name="requiredUndrilledItemCode">需要的生料料号</param>
    /// <param name="requiredUndrilledItemQuantity">需要的生料数量</param>
    /// <param name="outboundingLocation"></param>
    /// <returns></returns>
    bool TryFindHasUndrilledPanelsLocation(string requiredUndrilledItemCode, int requiredUndrilledItemQuantity, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到一个有生料的料仓的库位
    /// </summary>
    /// <param name="requirement">调度任务需求汇总</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindHasUndrilledPanelsLocation(ScheduleRequirement requirement, string partitionCode, string[] excludedLocations, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子指定分区上 找到一个有生料的料仓的库位
    /// </summary>
    /// <param name="requirement">调度任务需求汇总</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="routeCodes">工艺路线</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindHasUndrilledPanelsLocation(ScheduleRequirement requirement, string partitionCode, IReadOnlyList<string> routeCodes, string[] excludedLocations, out Location? outboundingLocation);

    #endregion TryFindHasUndrilledPanelsLocation

    #region TryFindIdleTimeoutDrilledLocation

    /// <summary>
    /// 从板料架子找到一个空闲超过指定时长的熟料库位
    /// </summary>
    /// <param name="parameters">系统配置参数</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindIdleTimeoutDrilledLocation(ConfigParameters parameters, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子找到一个空闲超过指定时长的熟料库位
    /// </summary>
    /// <param name="parameters">系统配置参数</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindIdleTimeoutDrilledLocation(ConfigParameters parameters, string[] excludedLocations, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子找到一个空闲超过指定时长的熟料库位
    /// </summary>
    /// <param name="parameters">系统配置参数</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindIdleTimeoutDrilledLocation(ConfigParameters parameters, string partitionCode, out Location? outboundingLocation);

    /// <summary>
    /// 从板料架子找到一个空闲超过指定时长的熟料库位
    /// </summary>
    /// <param name="parameters">系统配置参数</param>
    /// <param name="partitionCode">指定分区</param>
    /// <param name="excludedLocations">排除库位</param>
    /// <param name="outboundingLocation">导出库位</param>
    /// <returns></returns>
    bool TryFindIdleTimeoutDrilledLocation(ConfigParameters parameters, string partitionCode, string[] excludedLocations, out Location? outboundingLocation);

    #endregion TryFindIdleTimeoutDrilledLocation

    #region 负载的料仓进行summary

    /// <summary>
    /// 负载的生料仓
    /// </summary>
    IReadOnlyList<PayloadUndrilledSiloSummary> PayloadUndrilledSiloSummaries { get; }

    /// <summary>
    /// 负载的熟料仓
    /// </summary>
    IReadOnlyList<PayloadDrilledSiloSummary> PayloadDrilledSiloSummaries { get; }

    /// <summary>
    /// 负载的首件仓
    /// </summary>
    IReadOnlyList<PayloadFirstSiloSummary> PayloadFirstSiloSummaries { get; }

    #endregion 负载的料仓进行summary

    #region 指定分区下的负载的料仓进行summary

    /// <summary>
    /// 指定分区下的负载的生料仓
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    IReadOnlyList<PayloadUndrilledSiloSummary> PartitionPayloadUndrilledSiloSummaries(string partitionCode);

    /// <summary>
    /// 指定分区下的钻机需要的熟料仓
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    IReadOnlyList<PayloadDrilledSiloSummary> PartitionPayloadDrilledSiloSummaries(string partitionCode);

    /// <summary>
    /// 指定分区下的需要的首件仓
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    IReadOnlyList<PayloadFirstSiloSummary> PartitionPayloadFirstSiloSummaries(string partitionCode);

    #endregion 指定分区下的负载的料仓进行summary
}
