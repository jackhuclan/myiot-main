using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Schedule;
using VgAutoDrill.Central.Core.Schedule.Summary;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Manager;

public interface IDrillManager
{
    IReadOnlyList<Drill> Drills { get; }
    IReadOnlyList<DrillScheduleTask> NotStartedSchedules { get; }
    IReadOnlyList<Location> OnlineLocations { get; }

    /// <summary>
    /// 待生产工单数
    /// </summary>
    IReadOnlyList<WorkOrderTask> PendingWorkOrders { get; }

    /// <summary>
    /// 存在未完成的库位
    /// </summary>
    IReadOnlyList<Location> NotStartedLocations { get; }

    /// <summary>
    /// 指定分区存在未完成的库位
    /// </summary>
    /// <param name="partitionCode"></param>
    /// <returns></returns>
    IReadOnlyList<Location> PartitionNotStartedLocations(string partitionCode);

    /// <summary>
    /// 指定工艺路线下存在未完成的库位
    /// </summary>
    /// <param name="routeCodes">工艺路线</param>
    /// <returns></returns>
    IReadOnlyList<Location> RouteNotStartedLocations(IReadOnlyList<string> routeCodes);

    /// <summary>
    /// 指定分区和工艺路线下存在未完成的库位
    /// </summary>
    /// <param name="partitionCode">分区</param>
    /// <param name="routeCodes">工艺路线</param>
    /// <returns></returns>
    IReadOnlyList<Location> PartitionNotStartedLocations(string partitionCode, IReadOnlyList<string> routeCodes);

    /// <summary>
    /// 存在未分配agv的库位
    /// </summary>
    IReadOnlyList<Location> NotAllocatedLocations { get; }

    IReadOnlyList<Location> AvailableLocations { get; }

    IReadOnlyList<Location> Locations { get; }

    /// <summary>
    /// 所有钻机上的板料汇总
    /// </summary>
    /// <returns></returns>
    int PanelCount();

    /// <summary>
    /// 某个钻机上的板料汇总
    /// </summary>
    /// <param name="deviceId">钻机的设备id</param>
    /// <returns></returns>
    int PanelCount(string deviceId);

    /// <summary>
    /// 所有钻机上的某些状态的板料汇总
    /// </summary>
    /// <param name="productStatuses">物料状态</param>
    /// <returns></returns>
    int PanelCount(IReadOnlyList<ProductStatus> productStatuses);

    /// <summary>
    /// 某个钻机上的某些状态的板料汇总
    /// </summary>
    /// <param name="deviceId">钻机的设备id</param>
    /// <param name="productStatuses">物料状态</param>
    /// <returns></returns>
    int PanelCount(string deviceId, IReadOnlyList<ProductStatus> productStatuses);

    /// <summary>
    /// 统计包含所给物料状态的料仓数量
    /// </summary>
    /// <param name="productStatuses">所给物料状态</param>
    /// <returns></returns>
    int SiloCount(IReadOnlyList<ProductStatus> productStatuses);

    /// <summary>
    /// 统计包含所给物料状态的钻机设备数量
    /// </summary>
    /// <param name="productStatuses">所给物料状态</param>
    /// <returns></returns>
    int Count(IReadOnlyList<ProductStatus> productStatuses);

    Task Refresh();

    /// <summary>
    /// 分区库位
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <returns></returns>
    IReadOnlyList<Location> PartitionLocations(string partitionCode);

    /// <summary>
    /// 可用分区库位
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <returns></returns>
    IReadOnlyList<Location> PartitionAvailableLocations(string partitionCode);

    /// <summary>
    /// 工艺路线上所有可用钻机的库位,schedule create/partcomplete
    /// </summary>
    /// <param name="routeCodes">工艺路线</param>
    /// <returns></returns>
    IReadOnlyList<Location> RouteAvailableLocations(IReadOnlyList<string> routeCodes);

    IReadOnlyList<Drill> RouteDrills(IReadOnlyList<string> routeCodes);

    /// <summary>
    /// 根据agv直接匹配钻机需求
    /// </summary>
    /// <param name="agv"></param>
    /// <param name="drillRequirement"></param>
    /// <returns></returns>
    bool TryFindArbitraryDrillRequirementByAgv(PanelAgv agv, out ScheduleRequirement? drillRequirement);

    /// <summary>
    /// 钻机指定分区指定工艺路线可用库位
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <param name="routeCodes">工艺路线</param>
    /// <returns></returns>
    IReadOnlyList<Location> PartitionAvailableLocations(string partitionCode, IReadOnlyList<string> routeCodes);

    #region 钻机上的板料汇总

    /// <summary>
    /// 钻机上的板料汇总
    /// </summary>
    IReadOnlyList<ItemSummary> PayloadPanelSummaries { get; }

    /// <summary>
    /// 所给分区上的钻机的板料汇总
    /// </summary>
    /// <param name="partitionCode"></param>
    /// <returns></returns>
    IReadOnlyList<ItemSummary> PartitionPayloadPanelSummaries(string partitionCode);

    /// <summary>
    /// 所给可用分区上的钻机的板料汇总
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <returns></returns>
    IReadOnlyList<ItemSummary> PartitionAvailablePayloadPanelSummaries(string partitionCode);

    #endregion 钻机上的板料汇总

    #region 钻机呼叫的需求

    /// <summary>
    /// 所有钻机的所有需求
    /// </summary>
    IReadOnlyList<ScheduleRequirement> Requirements { get; }

    /// <summary>
    /// 工艺路线上所有钻机的需求
    /// </summary>
    /// <param name="routeCodes">工艺路线</param>
    /// <returns></returns>
    IReadOnlyList<ScheduleRequirement> RouteRequirements(IReadOnlyList<string> routeCodes);

    /// <summary>
    /// 钻机指定分区所需要的需求汇总
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <returns></returns>
    IReadOnlyList<ScheduleRequirement> PartitionRequirements(string partitionCode);

    /// <summary>
    /// 钻机指定分区指定工艺路线所需要的需求汇总
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <param name="routeCodes">工艺路线</param>
    /// <returns></returns>
    IReadOnlyList<ScheduleRequirement> PartitionRouteRequirements(string partitionCode, IReadOnlyList<string> routeCodes);

    IReadOnlyList<ScheduleRequirement> OrderedRequirements(IReadOnlyList<string> routeCodes);

    #endregion 钻机呼叫的需求

    #region 对钻机需要的生料板料进行summary

    /// <summary>
    /// 所有钻机的生料需求汇总
    /// </summary>
    IReadOnlyList<UndrilledItemSummary> RequiredUndrilledItemSummaries { get; }

    /// <summary>
    /// 工艺路线上所有钻机的生料汇总
    /// </summary>
    /// <param name="routeCodes">工艺路线</param>
    /// <returns></returns>
    IReadOnlyList<UndrilledItemSummary> RouteRequiredUndrilledItemSummaries(IReadOnlyList<string> routeCodes);

    /// <summary>
    /// 钻机指定分区生料汇总
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <returns></returns>
    IReadOnlyList<UndrilledItemSummary> PartitionRequiredUndrilledItemSummaries(string partitionCode);

    /// <summary>
    /// 钻机指定分区指定工艺路线所需要的生料汇总
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <param name="routeCodes">工艺路线</param>
    /// <returns></returns>
    IReadOnlyList<UndrilledItemSummary> PartitionRouteRequiredUndrilledItemSummaries(string partitionCode, IReadOnlyList<string> routeCodes);

    #endregion 对钻机需要的生料板料进行summary

    #region 对钻机需要的熟料板料进行summary

    /// <summary>
    /// 所有钻机的要下的熟料汇总
    /// </summary>
    IReadOnlyList<DrilledItemSummary> RequiredDrilledItemSummaries { get; }

    /// <summary>
    /// 工艺路线上所有钻机要下的熟料汇总
    /// </summary>
    /// <param name="routeCodes">工艺路线</param>
    /// <returns></returns>
    IReadOnlyList<DrilledItemSummary> RouteRequiredDrilledItemSummaries(IReadOnlyList<string> routeCodes);

    /// <summary>
    /// 工艺路线上所有钻机要下的熟料汇总
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <returns></returns>
    IReadOnlyList<DrilledItemSummary> PartitionRequiredDrilledItemSummaries(string partitionCode);

    /// <summary>
    /// 工艺路线上所有钻机要下的熟料汇总
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <param name="routeCodes">工艺路线</param>
    /// <returns></returns>
    IReadOnlyList<DrilledItemSummary> PartitionRouteRequiredDrilledItemSummaries(string partitionCode, IReadOnlyList<string> routeCodes);

    #endregion 对钻机需要的熟料板料进行summary

    #region 对钻机需要的首件板料进行summary

    /// <summary>
    /// 所有钻机的要下的首件汇总
    /// </summary>
    IReadOnlyList<FirstItemSummary> FirstDrilledItemSummaries { get; }

    /// <summary>
    /// 工艺路线上所有钻机的要下的首件汇总
    /// </summary>
    IReadOnlyList<FirstItemSummary> RouteFirstDrilledItemSummaries(IReadOnlyList<string> routeCodes);

    #endregion 对钻机需要的首件板料进行summary

    #region 对料号进行汇总

    /// <summary>
    /// 所有钻机的需求的生料料号
    /// </summary>
    IReadOnlyList<string> RequiredUndrilledItemCodes { get; }

    IReadOnlyList<string> RouteRequiredUndrilledItemCodes(IReadOnlyList<string> routeCodes);

    IReadOnlyList<string> PartitionRequiredUndrilledItemCodes(string partitionCode);

    IReadOnlyList<string> PartitionRouteRequiredUndrilledItemCodes(string partitionCode, IReadOnlyList<string> routeCodes);

    /// <summary>
    /// 所有钻机的需求的熟料料号
    /// </summary>
    IReadOnlyList<string> RequiredDrilledItemCodes { get; }

    /// <summary>
    /// 存在部分完成的库位
    /// </summary>
    IReadOnlyList<Location> PartCompletedLocations { get; }

    /// <summary>
    /// 钻机指定工艺路线所需要的熟料料号
    /// </summary>
    /// <param name="routeCodes">工艺路线</param>
    /// <returns></returns>
    IReadOnlyList<string> RouteRequiredDrilledItemCodes(IReadOnlyList<string> routeCodes);

    /// <summary>
    ///钻机指定分区所需要的熟料料号
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <returns></returns>
    IReadOnlyList<string> PartitionRequiredDrilledItemCodes(string partitionCode);

    /// <summary>
    /// 钻机指定分区指定工艺路线所需要的熟料料号
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    /// <param name="routeCodes">工艺路线</param>
    /// <returns></returns>
    IReadOnlyList<string> PartitionRouteRequiredDrilledItemCodes(string partitionCode, IReadOnlyList<string> routeCodes);

    #endregion 对料号进行汇总
}
