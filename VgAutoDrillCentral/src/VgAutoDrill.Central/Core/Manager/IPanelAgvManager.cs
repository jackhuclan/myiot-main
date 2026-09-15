using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Schedule.Summary;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Manager;

public interface IPanelAgvManager<T> where T : PanelAgv
{
    /// <summary>
    /// 当班agv，执行任务的agv
    /// </summary>
    IReadOnlyList<PanelAgv> DutyAgvs { get; }

    /// <summary>
    /// 所有板料agv
    /// </summary>
    IReadOnlyList<PanelAgv> PanelAgvs { get; }

    /// <summary>
    /// 所有上下料agv
    /// </summary>
    IReadOnlyList<BackPanelAgv> BackPanelAgvs { get; }

    /// <summary>
    /// 所有转运agv
    /// </summary>
    IReadOnlyList<TransferSiloAgv> TransferSiloAgvs { get; }

    /// <summary>
    /// 总共agv数量
    /// </summary>
    int Count();

    /// <summary>
    /// 空agv数
    /// </summary>
    int EmptyPayloadCount { get; }

    /// <summary>
    /// 托空料仓的agv数量
    /// </summary>
    int EmptySiloBoxCount { get; }

    IReadOnlyList<DrilledItemSummary> DrilledItemSummaries { get; }
    IReadOnlyList<UndrilledItemSummary> UndrilledItemSummaries { get; }
    IReadOnlyList<FirstItemSummary> FirstItemSummaries { get; }
    IReadOnlyList<Location> Locations { get; }

    /// <summary>
    /// 所有agv上的板料汇总
    /// </summary>
    /// <returns></returns>
    int PanelCount();

    /// <summary>
    /// 某个agv上的板料汇总
    /// </summary>
    /// <param name="deviceId">agv的设备id</param>
    /// <returns></returns>
    int PanelCount(string deviceId);

    /// <summary>
    /// 所有agv上的某些状态的板料汇总
    /// </summary>
    /// <param name="productStatuses">物料状态</param>
    /// <returns></returns>
    int PanelCount(IReadOnlyList<ProductStatus> productStatuses);

    /// <summary>
    /// 某个agv上的某些状态的板料汇总
    /// </summary>
    /// <param name="deviceId">agv的设备id</param>
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
    /// 统计包含所给物料状态的agv设备数量
    /// </summary>
    /// <param name="productStatuses">所给物料状态</param>
    /// <returns></returns>
    int Count(IReadOnlyList<ProductStatus> productStatuses);

    Task RunAsync();

    /// <summary>
    /// 满足传入工艺路线的agv
    /// </summary>
    /// <param name="routeCodes">工艺路线</param>
    /// <returns></returns>
    IReadOnlyList<PanelAgv> RouteAgvs(IReadOnlyList<string> routeCodes);

    /// <summary>
    /// 满足传入工艺路线的agv上的首件汇总
    /// </summary>
    /// <param name="routeCodes">工艺路线</param>
    /// <returns></returns>
    IReadOnlyList<FirstItemSummary> RouteFirstItemSummaries(IReadOnlyList<string> routeCodes);

    /// <summary>
    /// 满足传入工艺路线的agv上的生料汇总
    /// </summary>
    /// <param name="routeCodes">工艺路线</param>
    /// <returns></returns>
    IReadOnlyList<UndrilledItemSummary> RouteUndrilledItemSummaries(IReadOnlyList<string> routeCodes);

    /// <summary>
    /// 满足传入工艺路线的agv上的熟料汇总
    /// </summary>
    /// <param name="routeCodes">工艺路线</param>
    /// <returns></returns>
    IReadOnlyList<DrilledItemSummary> RouteDrilledItemSummaries(IReadOnlyList<string> routeCodes);

    /// <summary>
    /// 工艺路线上装有料仓的agv
    /// </summary>
    /// <param name="routeCodes">工艺路线</param>
    /// <returns></returns>
    IReadOnlyList<PanelAgv> RouteAgvsWithSilo(IReadOnlyList<string> routeCodes);

    #region AGV负载的料仓进行summary

    /// <summary>
    /// AGV负载的生料仓
    /// </summary>
    IReadOnlyList<PayloadUndrilledSiloSummary> PayloadUndrilledSiloSummaries { get; }

    /// <summary>
    ///负载的熟料仓
    /// </summary>
    IReadOnlyList<PayloadDrilledSiloSummary> PayloadDrilledSiloSummaries { get; }

    /// <summary>
    ///负载的首件仓
    /// </summary>
    IReadOnlyList<PayloadFirstSiloSummary> PayloadFirstSiloSummaries { get; }

    #endregion AGV负载的料仓进行summary

    #region 指定分区下的AGV负载的料仓进行summary

    /// <summary>
    /// 指定分区下的AGV负载的生料仓
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    IReadOnlyList<PayloadUndrilledSiloSummary> PartitionPayloadUndrilledSiloSummaries(string partitionCode);

    /// <summary>
    /// 指定分区下的AGV负载的熟料仓
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    IReadOnlyList<PayloadDrilledSiloSummary> PartitionPayloadDrilledSiloSummaries(string partitionCode);

    /// <summary>
    /// 指定分区下的AGV负载的首件仓
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    IReadOnlyList<PayloadFirstSiloSummary> PartitionPayloadFirstSiloSummaries(string partitionCode);

    IReadOnlyList<PanelAgv> PartitionAgvs(string partitionCode);

    #endregion 指定分区下的AGV负载的料仓进行summary
}
