using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using VgAutoDrill.Central.Core.Calculator;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Mysql;
using VgAutoDrill.Central.Core.Schedule.Summary;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Transportation;

namespace VgAutoDrill.Central.Core.Mes.Model;

/// <summary>
/// 库位分区
/// </summary>
public abstract class Partition
{
    private volatile bool _locked = false;
    private readonly IAutoSiloTransferStrategyFactory _autoSiloTransferStrategyFactory;
    private readonly MysqlTaskSchedulerOptions _mysqlTaskSchedulerOptions;
    private readonly PartitionAutoSiloTransferOptions _partitionAutoSiloTransferOptions;
    private readonly IAutoSiloTransferStrategy _autoSiloTransferStrategy;

    internal Partition()
    { }

    public Partition(IOptions<MysqlTaskSchedulerOptions> options,
        IAutoSiloTransferStrategyFactory autoSiloTransferStrategyFactory,
        string partCode,
        PartitionKind partitionKind)
    {
        PartCode = partCode;
        PartitionKind = partitionKind;
        _mysqlTaskSchedulerOptions = options.Value;
        _autoSiloTransferStrategyFactory = autoSiloTransferStrategyFactory;
        _partitionAutoSiloTransferOptions = (PartitionAutoSiloTransferOptions)(_mysqlTaskSchedulerOptions.TransferOptions.
            FirstOrDefault(x => string.Equals(x.PartCode, PartCode, StringComparison.OrdinalIgnoreCase)
             && x.PartitionKind == PartitionKind) ?? _mysqlTaskSchedulerOptions.TransferOptions.
            FirstOrDefault(x => x.PartitionKind == PartitionKind) ?? PartitionAutoSiloTransferOptions.None).Clone();
        _partitionAutoSiloTransferOptions.PartCode = partCode;
        _autoSiloTransferStrategy = _autoSiloTransferStrategyFactory.Create(_partitionAutoSiloTransferOptions);
    }

    /// <summary>
    /// 料仓任务转运策略
    /// </summary>
    ///
    [JsonIgnore]
    public IAutoSiloTransferStrategy SiloTransferStrategy => _autoSiloTransferStrategy;

    /// <summary>
    /// 货架库位
    /// </summary>
    public abstract IReadOnlyList<Location> RackLocations { get; }

    /// <summary>
    /// 分区code
    /// </summary>
    public string? PartCode { get; set; }

    /// <summary>
    /// 分区名
    /// </summary>
    public string? PartName { get; set; }

    /// <summary>
    /// 该点预定分配的AGV
    /// </summary>
    public string PreBookAGV { get; set; }

    public DateTime? PreBookTime { get; set; }
    ///// <summary>
    ///// 该点预定分配的AGV
    ///// </summary>
    //public string PreBookRoute { get; set; }

    public List<string> RouteCodes { get; set; } = new List<string>();

    /// <summary>
    /// 分区种类
    /// </summary>
    public PartitionKind PartitionKind { get; set; } = PartitionKind.Unknown;

    /// <summary>
    /// 料仓类型
    /// </summary>
    public TransportationKind SiloKind { get; set; } = TransportationKind.None;

    /// <summary>
    /// 本分区货架库位上的库位数量
    /// </summary>
    public int LocationsCount => RackLocations.Count();

    /// <summary>
    /// 本分区货架库位上的空层数
    /// </summary>
    public int EmptySiloBoxCount => RackLocations.Sum(x => x.EmptySiloBoxCount);

    /// <summary>
    /// 钻完孔的板数
    /// </summary>
    public int DrilledPanelsCount => RackLocations.Sum(x => x.DrilledPanelsCount);

    /// <summary>
    /// 本分区货架库位上的生料板数
    /// </summary>
    public int UndrilledPanelsCount => RackLocations.Sum(x => x.UndrilledPanelsCount);

    /// <summary>
    /// 本分区货架库位上的所载料仓的首件板料数量
    /// </summary>
    public int FirstPanelsCount => RackLocations.Sum(x => x.FirstPanelsCount);

    /// <summary>
    /// 本分区货架库位上的现有空仓数
    /// </summary>
    public int EmptyBoxCountNow => RackLocations.Count(x => x.IsEmptySiloBox);

    /// <summary>
    /// 本分区货架库位上的现有空位数
    /// </summary>
    public int EmptyPayloadCountNow => RackLocations.Count(x => x.IsEmptyPayload);

    /// <summary>
    /// 本分区货架库位上的即将空仓数
    /// </summary>
    public int EmptyBoxCountSoon => RackLocations.Count(x => x.IsEmptySiloBoxSoon);

    /// <summary>
    /// 本分区货架库位上的即将空位数
    /// </summary>
    public int EmptyPayloadCountSoon => RackLocations.Count(x => x.IsEmptyPayloadSoon);

    /// <summary>
    /// 本分区货架库位上的生料汇总
    /// </summary>
    public IReadOnlyList<UndrilledItemSummary> RackUndrilledItemSummaries => RackLocations
                                            .SelectMany(x => x.UndrilledPanels)
                                            .GroupBy(x => x.ItemCode)
                                            .Select(x => new UndrilledItemSummary
                                            {
                                                ItemCode = x.Key,
                                                ItemCount = x.Count(),
                                                RouteCodes = RackLocations
                                                            .Where(a => a.ContainsUndrilledItemCode(x.Key))
                                                            .SelectMany(i => i.RouteCodes)
                                                            .Distinct()
                                                            .OrderBy(x => x)
                                                            .ToList(),
                                                ScheduleIds = RackLocations
                                                            .Where(a => a.ContainsUndrilledItemCode(x.Key))
                                                             .Select(i => i.Schedule.Id)
                                                             .Distinct()
                                                             .OrderBy(x => x)
                                                             .ToList(),
                                                DeviceIds = RackLocations
                                                            .Where(a => a.ContainsUndrilledItemCode(x.Key))
                                                            .Select(i => i.Code)
                                                            .Distinct()
                                                            .OrderBy(x => x)
                                                            .ToList(),
                                            })
                                            .ToList()
                                            .AsReadOnly();

    /// <summary>
    /// 本分区货架库位上的熟料汇总
    /// </summary>
    public IReadOnlyList<DrilledItemSummary> RackDrilledItemSummaries => RackLocations
                                            .SelectMany(x => x.DrilledPanels)
                                            .GroupBy(x => x.ItemCode)
                                            .Select(x => new DrilledItemSummary
                                            {
                                                ItemCode = x.Key,
                                                ItemCount = x.Count(),
                                                RouteCodes = RackLocations
                                                            .Where(a => a.ContainsDrilledItemCode(x.Key))
                                                            .SelectMany(i => i.RouteCodes)
                                                            .Distinct()
                                                            .OrderBy(x => x)
                                                            .ToList(),
                                                ScheduleIds = RackLocations
                                                            .Where(a => a.ContainsDrilledItemCode(x.Key))
                                                             .Select(i => i.Schedule.Id)
                                                             .Distinct()
                                                             .OrderBy(x => x)
                                                             .ToList(),
                                                DeviceIds = RackLocations
                                                            .Where(a => a.ContainsDrilledItemCode(x.Key))
                                                            .Select(i => i.Code)
                                                            .Distinct()
                                                            .OrderBy(x => x)
                                                            .ToList(),
                                            })
                                            .ToList()
                                            .AsReadOnly();

    /// <summary>
    /// 本分区货架库位上的首件汇总
    /// </summary>
    public IReadOnlyList<FirstItemSummary> RackFirstItemSummaries => RackLocations
                                        .SelectMany(x => x.FirstPanels)
                                        .GroupBy(x => x.ItemCode)
                                        .Select(x => new FirstItemSummary
                                        {
                                            ItemCode = x.Key,
                                            ItemCount = x.Count(),
                                            RouteCodes = RackLocations
                                                            .Where(a => a.ContainsDrilledItemCode(x.Key))
                                                            .SelectMany(i => i.RouteCodes)
                                                            .Distinct()
                                                            .OrderBy(x => x)
                                                            .ToList(),
                                            ScheduleIds = RackLocations
                                                            .Where(a => a.ContainsDrilledItemCode(x.Key))
                                                             .Select(i => i.Schedule.Id)
                                                             .Distinct()
                                                             .OrderBy(x => x)
                                                             .ToList(),
                                            DeviceIds = RackLocations
                                                            .Where(a => a.ContainsDrilledItemCode(x.Key))
                                                            .Select(i => i.Code)
                                                            .Distinct()
                                                            .OrderBy(x => x)
                                                            .ToList(),
                                        })
                                        .ToList()
                                        .AsReadOnly();

    /// <summary>
    /// 锁住本区域
    /// </summary>
    /// <returns></returns>
    public bool TryLock()
    {
        if (!_locked)
        {
            _locked = true;
            return true;
        }

        return false;
    }

    /// <summary>
    /// 释放本区域的锁
    /// </summary>
    public void ReleaseLock()
    {
        _locked = false;
    }

    /// <summary>
    /// 最少空位数
    /// </summary>
    public int? MinEmptyLocationNum { get; set; }

    /// <summary>
    /// 最少空仓数
    /// </summary>
    public int? MinEmptyBoxNum { get; set; }

    /// <summary>
    /// 最多空仓数
    /// </summary>
    public int? MaxEmptyBoxNum { get; set; }

    /// <summary>
    /// 熟料最少转出数量
    /// </summary>
    public int? MinDrilledTrackOutNum { get; set; }

    /// <summary>
    /// 料仓中没有钻机中相同的料号时是否立即转出
    /// </summary>
    public bool? IsAutoTrackOutDrilledSilo { get; set; }

    /// <summary>
    /// 生料转出超时时间
    /// </summary>
    public int? RawTrackOutTimeOutTime { get; set; }

    /// <summary>
    /// 首件最少转出数量
    /// </summary>
    public int? MinFirstTrackOutNum { get; set; }

    /// <summary>
    /// 状态(1:启用;0禁用)
    /// </summary>
    public int? Status { get; set; }

    /// <summary>
    /// 熟料转出超时(分钟，默认10分钟)
    /// </summary>
    public virtual int? DrilledTrackOutTimeMinutes { get; set; }
}
