using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Schedule.Deliver;
using VgAutoDrill.Central.Core.Schedule.Summary;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Manager;

public abstract class PanelAgvManager<T> : IPanelAgvManager<T> where T : PanelAgv
{
    private readonly IDeviceManager _deviceManager;
    private readonly IScheduleTaskDeliverPolicyFactory _scheduleTaskDeliverPolicyFactory;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly ILogger<PanelAgvManager<T>> _logger;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);

    public PanelAgvManager(IServiceProvider serviceProvider)
    {
        _deviceManager = serviceProvider.GetRequiredService<IDeviceManager>();
        _scheduleTaskDeliverPolicyFactory = serviceProvider.GetRequiredService<IScheduleTaskDeliverPolicyFactory>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<PanelAgvManager<T>>();
    }

    public abstract IReadOnlyList<PanelAgv> DutyAgvs { get; }
    public IReadOnlyList<PanelAgv> PanelAgvs => _deviceManager.PanelAgvs;

    public IReadOnlyList<PanelAgv> RouteAgvs(IReadOnlyList<string> routeCodes) => DutyAgvs.Where(x => x.RouteCodes.Intersect(routeCodes).Any()).ToList();

    public IReadOnlyList<PanelAgv> PartitionAgvs(string partitionCode) => DutyAgvs.Where(x => string.Equals(x.Location.PartitionCode, partitionCode, StringComparison.OrdinalIgnoreCase)).ToList();

    public IReadOnlyList<PanelAgv> RouteAgvsWithSilo(IReadOnlyList<string> routeCodes) => RouteAgvs(routeCodes).Where(x => x.Location.HasSilo).ToList();

    public IReadOnlyList<BackPanelAgv> BackPanelAgvs => _deviceManager.BackPanelAgvs;

    public IReadOnlyList<TransferSiloAgv> TransferSiloAgvs => _deviceManager.TransferSiloAgvs;

    public IReadOnlyList<Location> Locations => DutyAgvs.Select(x => x.Location).Where(x => x != null).ToList().AsReadOnly()!;

    /// <summary>
    /// 库位数
    /// </summary>
    public int Count() => DutyAgvs.Count();

    /// <summary>
    /// 空库位数
    /// </summary>
    public int EmptyPayloadCount => DutyAgvs.Count(x => x.Location != null && x.Location.IsEmptyPayload);

    /// <summary>
    /// 空仓数
    /// </summary>
    public int EmptySiloBoxCount => DutyAgvs.Count(x => x.Location != null && x.Location.IsEmptySiloBox);

    public IReadOnlyList<DrilledItemSummary> DrilledItemSummaries => DutyAgvs
                                        .Select(x => x.Location)
                                        .SelectMany(x => x.Panels)
                                        .Where(x => ProductStatusConstants.Finished_DRILL.Contains(x.ProductStatus))
                                        .GroupBy(x => x.ItemCode)
                                        .Select(x => new DrilledItemSummary
                                        {
                                            ItemCode = x.Key,
                                            ItemCount = x.Count(),
                                            RouteCodes = DutyAgvs
                                                        .Where(r => r.Location.ContainsDrilledItemCode(x.Key))
                                                        .SelectMany(i => i.RouteCodes)
                                                        .Distinct()
                                                        .OrderBy(x => x)
                                                        .ToList(),
                                            DeviceIds = DutyAgvs
                                                        .Where(r => r.Location.ContainsDrilledItemCode(x.Key))
                                                        .Select(i => i.DeviceId)
                                                        .Distinct()
                                                        .OrderBy(x => x)
                                                        .ToList(),
                                        })
                                        .ToList()
                                        .AsReadOnly();

    public IReadOnlyList<DrilledItemSummary> RouteDrilledItemSummaries(IReadOnlyList<string> routeCodes) => RouteAgvs(routeCodes)
                                        .Select(x => x.Location)
                                        .SelectMany(x => x.Panels)
                                        .Where(x => ProductStatusConstants.Finished_DRILL.Contains(x.ProductStatus))
                                        .GroupBy(x => x.ItemCode)
                                        .Select(x => new DrilledItemSummary
                                        {
                                            ItemCode = x.Key,
                                            ItemCount = x.Count(),
                                            RouteCodes = RouteAgvs(routeCodes)
                                                        .Where(r => r.Location.ContainsDrilledItemCode(x.Key))
                                                        .SelectMany(i => i.RouteCodes)
                                                        .Distinct()
                                                        .OrderBy(x => x)
                                                        .ToList(),
                                            DeviceIds = RouteAgvs(routeCodes)
                                                        .Where(r => r.Location.ContainsDrilledItemCode(x.Key))
                                                        .Select(i => i.DeviceId)
                                                        .Distinct()
                                                        .OrderBy(x => x)
                                                        .ToList(),
                                        })
                                        .ToList()
                                        .AsReadOnly();

    public IReadOnlyList<UndrilledItemSummary> UndrilledItemSummaries => DutyAgvs
                                        .Select(x => x.Location)
                                        .SelectMany(x => x.Panels)
                                        .Where(x => !string.IsNullOrEmpty(x.ItemCode) && ProductStatusConstants.Finished_PIN.Contains(x.ProductStatus))
                                        .GroupBy(x => x.ItemCode)
                                        .Select(x => new UndrilledItemSummary
                                        {
                                            ItemCode = x.Key,
                                            ItemCount = x.Count(),
                                            RouteCodes = DutyAgvs
                                                        .Where(r => r.Location.ContainsUndrilledItemCode(x.Key))
                                                        .SelectMany(i => i.RouteCodes)
                                                        .Distinct()
                                                        .OrderBy(x => x)
                                                        .ToList(),
                                            DeviceIds = DutyAgvs
                                                        .Where(r => r.Location.ContainsUndrilledItemCode(x.Key))
                                                        .Select(i => i.DeviceId)
                                                        .Distinct()
                                                        .OrderBy(x => x)
                                                        .ToList(),
                                        })
                                        .ToList()
                                        .AsReadOnly();

    public IReadOnlyList<UndrilledItemSummary> RouteUndrilledItemSummaries(IReadOnlyList<string> routeCodes) => RouteAgvs(routeCodes)
                                        .Select(x => x.Location)
                                        .SelectMany(x => x.Panels)
                                        .Where(x => !string.IsNullOrEmpty(x.ItemCode) && ProductStatusConstants.Finished_PIN.Contains(x.ProductStatus))
                                        .GroupBy(x => x.ItemCode)
                                        .Select(x => new UndrilledItemSummary
                                        {
                                            ItemCode = x.Key,
                                            ItemCount = x.Count(),
                                            RouteCodes = RouteAgvs(routeCodes)
                                                        .Where(r => r.Location.ContainsUndrilledItemCode(x.Key))
                                                        .SelectMany(i => i.RouteCodes)
                                                        .Distinct()
                                                        .OrderBy(x => x)
                                                        .ToList(),
                                            DeviceIds = RouteAgvs(routeCodes)
                                                        .Where(r => r.Location.ContainsUndrilledItemCode(x.Key))
                                                        .Select(i => i.DeviceId)
                                                        .Distinct()
                                                        .OrderBy(x => x)
                                                        .ToList(),
                                        })
                                        .ToList()
                                        .AsReadOnly();

    public IReadOnlyList<FirstItemSummary> FirstItemSummaries => DutyAgvs
                                        .Select(x => x.Location)
                                        .SelectMany(x => x.Panels)
                                        .Where(x => x.IsFirst)
                                        .GroupBy(x => x.ItemCode)
                                        .Select(x => new FirstItemSummary
                                        {
                                            ItemCode = x.Key,
                                            ItemCount = x.Count(),
                                            RouteCodes = DutyAgvs
                                                        .Where(r => r.Location.ContainsDrilledItemCode(x.Key))
                                                        .SelectMany(i => i.RouteCodes)
                                                        .Distinct()
                                                        .OrderBy(x => x)
                                                        .ToList(),
                                            DeviceIds = DutyAgvs
                                                        .Where(r => r.Location.ContainsDrilledItemCode(x.Key))
                                                        .Select(i => i.DeviceId)
                                                        .Distinct()
                                                        .OrderBy(x => x)
                                                        .ToList(),
                                        })
                                        .ToList()
                                        .AsReadOnly();

    public IReadOnlyList<FirstItemSummary> RouteFirstItemSummaries(IReadOnlyList<string> routeCodes) => RouteAgvs(routeCodes)
                                        .Select(x => x.Location)
                                        .SelectMany(x => x.Panels)
                                        .Where(x => x.IsFirst)
                                        .GroupBy(x => x.ItemCode)
                                        .Select(x => new FirstItemSummary
                                        {
                                            ItemCode = x.Key,
                                            ItemCount = x.Count(),
                                            RouteCodes = RouteAgvs(routeCodes)
                                                        .Where(r => r.Location.ContainsDrilledItemCode(x.Key))
                                                        .SelectMany(i => i.RouteCodes)
                                                        .Distinct()
                                                        .OrderBy(x => x)
                                                        .ToList(),
                                            DeviceIds = RouteAgvs(routeCodes)
                                                        .Where(r => r.Location.ContainsDrilledItemCode(x.Key))
                                                        .Select(i => i.DeviceId)
                                                        .Distinct()
                                                        .OrderBy(x => x)
                                                        .ToList(),
                                        })
                                        .ToList()
                                        .AsReadOnly();

    #region AGV负载的料仓进行summary

    /// <summary>
    /// AGV负载的生料仓
    /// </summary>
    public IReadOnlyList<PayloadUndrilledSiloSummary> PayloadUndrilledSiloSummaries => DutyAgvs
        .SelectMany(x => x.Location.UndrilledItemCodes)
        .Where(x => x.Any())
        .GroupBy(x => x)
        .Select(x => new PayloadUndrilledSiloSummary
        {
            ItemCode = x.Key,
            SiloCodes = DutyAgvs.Select(y => y.Location)
                        .Where(y => y.UndrilledItemCodes.Contains(x.Key))
                        .Select(y => y.SiloCode)
                        .ToList()
                        .AsReadOnly(),
            SiloCount = DutyAgvs.Select(y => y.Location)
                        .Where(y => y.UndrilledItemCodes.Contains(x.Key))
                        .Select(y => y.SiloCode)
                        .ToList()
                        .Count(),
            LocatioCodes = DutyAgvs.Select(y => y.Location)
                        .Where(y => y.UndrilledItemCodes.Contains(x.Key))
                        .Select(y => y.Code)
                        .ToList()
                        .AsReadOnly(),
            UndrilledItemCounts = DutyAgvs.Select(y => y.Location)
                        .Where(y => y.UndrilledItemCodes.Contains(x.Key))
                        .Select(y => y.UndrilledItemCodes.Count(z => z == x.Key))
                        .ToList()
                        .AsReadOnly()
        })
        .ToList()
        .AsReadOnly();

    /// <summary>
    /// AGV负载的熟料仓
    /// </summary>
    public IReadOnlyList<PayloadDrilledSiloSummary> PayloadDrilledSiloSummaries => DutyAgvs
        .Where(x => !x.Location.ContainsFirst)
        .SelectMany(x => x.Location.DrilledItemCodes)
        .Where(x => x.Any())
        .GroupBy(x => x)
        .Select(x => new PayloadDrilledSiloSummary
        {
            ItemCode = x.Key,
            SiloCodes = DutyAgvs.Select(y => y.Location)
                        .Where(y => y.DrilledItemCodes.Contains(x.Key))
                        .Select(y => y.SiloCode)
                        .ToList()
                        .AsReadOnly(),
            SiloCount = DutyAgvs.Select(y => y.Location)
                        .Where(y => y.DrilledItemCodes.Contains(x.Key))
                        .Select(y => y.SiloCode)
                        .ToList()
                        .Count(),
            LocatioCodes = DutyAgvs.Select(y => y.Location)
                        .Where(y => y.DrilledItemCodes.Contains(x.Key))
                        .Select(y => y.Code)
                        .ToList()
                        .AsReadOnly(),
            EmptyLayerCounts = DutyAgvs.Select(y => y.Location)
                        .Where(y => y.DrilledItemCodes.Contains(x.Key))
                        .Select(x => x.EmptySiloBoxCount)
                        .ToList()
                        .AsReadOnly()
        })
        .ToList()
        .AsReadOnly();

    /// <summary>
    /// AGV负载的首件仓
    /// </summary>
    public IReadOnlyList<PayloadFirstSiloSummary> PayloadFirstSiloSummaries => DutyAgvs
        .Where(x => x.Location.ContainsFirst)
        .SelectMany(x => x.Location.FirstItemCodes)
        .Where(x => x.Any())
        .GroupBy(x => x)
        .Select(x => new PayloadFirstSiloSummary
        {
            ItemCode = x.Key,
            SiloCodes = DutyAgvs.Select(y => y.Location)
                        .Where(y => y.FirstItemCodes.Contains(x.Key))
                        .Select(y => y.SiloCode)
                        .ToList()
                        .AsReadOnly(),
            SiloCount = DutyAgvs.Select(y => y.Location)
                        .Where(y => y.FirstItemCodes.Contains(x.Key))
                        .Select(y => y.SiloCode)
                        .ToList()
                        .Count(),
            LocatioCodes = DutyAgvs.Select(y => y.Location)
                        .Where(y => y.FirstItemCodes.Contains(x.Key))
                        .Select(y => y.Code)
                        .ToList()
                        .AsReadOnly(),
            EmptyLayerCounts = DutyAgvs.Select(y => y.Location)
                        .Where(y => y.FirstItemCodes.Contains(x.Key))
                        .Select(x => x.EmptySiloBoxCount)
                        .ToList()
                        .AsReadOnly()
        })
        .ToList()
        .AsReadOnly();

    #endregion AGV负载的料仓进行summary

    #region 指定分区下的AGV负载的料仓进行summary

    /// <summary>
    /// 指定分区下的AGV负载的生料仓
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    public IReadOnlyList<PayloadUndrilledSiloSummary> PartitionPayloadUndrilledSiloSummaries(string partitionCode) => PartitionAgvs(partitionCode)
         .SelectMany(x => x.Location.UndrilledItemCodes)
         .Where(x => x.Any())
         .GroupBy(x => x)
         .Select(x => new PayloadUndrilledSiloSummary
         {
             ItemCode = x.Key,
             SiloCodes = DutyAgvs.Select(y => y.Location)
                         .Where(y => y.UndrilledItemCodes.Contains(x.Key))
                         .Select(y => y.SiloCode)
                         .ToList()
                         .AsReadOnly(),
             SiloCount = DutyAgvs.Select(y => y.Location)
                         .Where(y => y.UndrilledItemCodes.Contains(x.Key))
                         .Select(y => y.SiloCode)
                         .ToList()
                         .Count(),
             LocatioCodes = DutyAgvs.Select(y => y.Location)
                         .Where(y => y.UndrilledItemCodes.Contains(x.Key))
                         .Select(y => y.Code)
                         .ToList()
                         .AsReadOnly(),
             UndrilledItemCounts = DutyAgvs.Select(y => y.Location)
                         .Where(y => y.UndrilledItemCodes.Contains(x.Key))
                         .Select(y => y.UndrilledItemCodes.Count(z => z == x.Key))
                         .ToList()
                         .AsReadOnly()
         })
         .ToList()
         .AsReadOnly();

    /// <summary>
    /// 指定分区下的AGV负载的熟料仓
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    public IReadOnlyList<PayloadDrilledSiloSummary> PartitionPayloadDrilledSiloSummaries(string partitionCode) => PartitionAgvs(partitionCode)
        .Where(x => !x.Location.ContainsFirst)
        .SelectMany(x => x.Location.DrilledItemCodes)
        .Where(x => x.Any())
        .GroupBy(x => x)
        .Select(x => new PayloadDrilledSiloSummary
        {
            ItemCode = x.Key,
            SiloCodes = DutyAgvs.Select(y => y.Location)
                        .Where(y => y.DrilledItemCodes.Contains(x.Key))
                        .Select(y => y.SiloCode)
                        .ToList()
                        .AsReadOnly(),
            SiloCount = DutyAgvs.Select(y => y.Location)
                        .Where(y => y.DrilledItemCodes.Contains(x.Key))
                        .Select(y => y.SiloCode)
                        .ToList()
                        .Count(),
            LocatioCodes = DutyAgvs.Select(y => y.Location)
                        .Where(y => y.DrilledItemCodes.Contains(x.Key))
                        .Select(y => y.Code)
                        .ToList()
                        .AsReadOnly(),
            EmptyLayerCounts = DutyAgvs.Select(y => y.Location)
                        .Where(y => y.DrilledItemCodes.Contains(x.Key))
                        .Select(x => x.EmptySiloBoxCount)
                        .ToList()
                        .AsReadOnly()
        })
        .ToList()
        .AsReadOnly();

    /// <summary>
    /// 指定分区下的AGV负载的首件仓
    /// </summary>
    /// <param name="partitionCode">分区code</param>
    public IReadOnlyList<PayloadFirstSiloSummary> PartitionPayloadFirstSiloSummaries(string partitionCode) => PartitionAgvs(partitionCode)
        .Where(x => x.Location.ContainsFirst)
        .SelectMany(x => x.Location.FirstItemCodes)
        .Where(x => x.Any())
        .GroupBy(x => x)
        .Select(x => new PayloadFirstSiloSummary
        {
            ItemCode = x.Key,
            SiloCodes = DutyAgvs.Select(y => y.Location)
                        .Where(y => y.FirstItemCodes.Contains(x.Key))
                        .Select(y => y.SiloCode)
                        .ToList()
                        .AsReadOnly(),
            SiloCount = DutyAgvs.Select(y => y.Location)
                        .Where(y => y.FirstItemCodes.Contains(x.Key))
                        .Select(y => y.SiloCode)
                        .ToList()
                        .Count(),
            LocatioCodes = DutyAgvs.Select(y => y.Location)
                        .Where(y => y.FirstItemCodes.Contains(x.Key))
                        .Select(y => y.Code)
                        .ToList()
                        .AsReadOnly(),
            EmptyLayerCounts = DutyAgvs.Select(y => y.Location)
                        .Where(y => y.FirstItemCodes.Contains(x.Key))
                        .Select(x => x.EmptySiloBoxCount)
                        .ToList()
                        .AsReadOnly()
        })
        .ToList()
        .AsReadOnly();

    #endregion 指定分区下的AGV负载的料仓进行summary

    /// <summary>
    /// 所有agv上的板料汇总
    /// </summary>
    /// <returns></returns>
    public int PanelCount()
    {
        return DutyAgvs.Sum(x => x.Location.PanelCount());
    }

    /// <summary>
    /// 某个agv上的板料汇总
    /// </summary>
    /// <param name="deviceId">agv的设备id</param>
    /// <returns></returns>
    public int PanelCount(string deviceId)
    {
        return DutyAgvs.Where(x => x.DeviceId == deviceId)
             .Sum(x => x.Location.PanelCount());
    }

    /// <summary>
    /// 所有agv上的某些状态的板料汇总
    /// </summary>
    /// <param name="productStatuses">物料状态</param>
    /// <returns></returns>
    public int PanelCount(IReadOnlyList<ProductStatus> productStatuses)
    {
        return DutyAgvs.Sum(x => x.Location.PanelCount(productStatuses));
    }

    /// <summary>
    /// 某个agv上的某些状态的板料汇总
    /// </summary>
    /// <param name="deviceId">agv的设备id</param>
    /// <param name="productStatuses">物料状态</param>
    /// <returns></returns>
    public int PanelCount(string deviceId, IReadOnlyList<ProductStatus> productStatuses)
    {
        return DutyAgvs.Where(x => x.DeviceId == deviceId)
            .Sum(x => x.Location.PanelCount(productStatuses));
    }

    /// <summary>
    /// 统计包含所给物料状态的料仓数量
    /// </summary>
    /// <param name="productStatuses">所给物料状态</param>
    /// <returns></returns>
    public int SiloCount(IReadOnlyList<ProductStatus> productStatuses)
    {
        return DutyAgvs.Select(x => x.Location).Count(x => x.ContainProductStatuses(productStatuses));
    }

    /// <summary>
    /// 统计包含所给物料状态的agv设备
    /// </summary>
    /// <param name="productStatuses">所给物料状态</param>
    /// <returns></returns>
    public int Count(IReadOnlyList<ProductStatus> productStatuses)
    {
        return DutyAgvs.Count(x => x.Location.ContainProductStatuses(productStatuses));
    }

    public abstract Task RunAsync();
}
