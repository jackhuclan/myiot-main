using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Cache.Interfaces;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Schedule.Summary;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.WebApi.Controllers.stat;

[ApiController]
[Route("central/stat/location")]
public class LocationController : ControllerBase
{
    private readonly ILocationManager _locationManager;
    private readonly IDrillManager _drillManager;
    private readonly IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask> _panelSiloForkManager;
    private readonly IPanelAgvManager<BackPanelAgv> _backPanelAgvManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IMemoryCacheManager _memoryCacheManager;
    public LocationController(ILocationManager locationManager, IServiceProvider serviceProvider)
    {
        _locationManager = locationManager;
        _drillManager = serviceProvider.GetRequiredService<IDrillManager>();
        _panelSiloForkManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>>();
        _backPanelAgvManager = serviceProvider.GetRequiredService<IPanelAgvManager<BackPanelAgv>>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _memoryCacheManager = serviceProvider.GetRequiredService<IMemoryCacheManager>();
    }

    [HttpGet]
    public object? Index(string? code)
    {
        if (string.IsNullOrEmpty(code))
        {
            return _locationManager.Locations;
        }

        return _locationManager.Locations.FirstOrDefault(x => x.Code == code);
    }

    [HttpGet("schedule")]
    public object? Schedule(string deviceId)
    {
        var locations = _locationManager.Locations.Where(x => x.DeviceId == deviceId);
        return locations.Select(x =>
                new
                {
                    LocationCode = x.Code,
                    x.SiloCode,
                    x.DeviceId,
                    x.Panels,
                    x.TransAGVInnerPoint,
                    x.TransAGVOutputPoint,
                    x.TransAGVRestPoint,
                    x.FeedAGVInnerPoint,
                    x.FeedAGVOutputPoint,
                    x.FeedAGVRestPoint,
                    TraceId = x.Schedule?.Code,
                }).ToList();
    }

    [HttpGet("simple")]
    public object? Simple(DeviceKind deviceKind = DeviceKind.PanelSiloFork)
    {
        var locations = _locationManager.Locations.Where(x => x.RequestDeviceKind == deviceKind);

        return locations.Select(x =>
                new
                {
                    x.Code,
                    x.SiloCode,
                    x.DeviceId,
                    x.Panels,
                    x.Appointed,
                    x.AppointedMessage,
                    x.UndrilledItemCodes,
                    x.DrilledItemCodes,
                    x.Index,
                    x.HostDevice.Properties,
                }).ToList();
    }

    /// <summary>
    /// 查询生料滞留的库位
    /// </summary>
    /// <param name="code">分区编号</param>
    /// <param name="second">超时时间秒</param>
    /// <returns></returns>
    [HttpGet("RawTimeout")]
    public object? TryFindIdleTimeoutUndrilledLocation(string? code, int second)
    {
        return _panelSiloForkManager.PartitionLocations(code).Select(p => new
        {
            p.Code,
            TimeoutSecond = second,
            PartCode = code,
            p.PartitionCode,
            p.Appointed,
            p.CanUnloadPanelSilo,
            p.ContainsUndrilled,
            p.Schedule!.CreateTime,
            p.UndrilledItemCodes,
            NeedUndrilledItemCodes = _drillManager.PartitionRequiredUndrilledItemCodes(code).Distinct().ToList(),
            Interval = TimeSpan.FromTicks(DateTime.Now.Ticks - p.Schedule!.CreateTime.Ticks).TotalSeconds,
            idleTimeout = TimeSpan.FromTicks(DateTime.Now.Ticks - p.Schedule!.CreateTime.Ticks).TotalSeconds > second,
            notRequiredByDrill = !_drillManager.PartitionRequiredUndrilledItemCodes(code).Intersect(p.UndrilledItemCodes).Any(),
            InspectItems = _drillManager.PartitionRequiredUndrilledItemCodes(code).Intersect(p.UndrilledItemCodes)
        }).ToList();
    }

    /// <summary>
    /// 熟料转出
    /// </summary>
    /// <param name="code"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    [HttpGet("ClinerTransfer")]
    public object? TryFindFullDrilledLocation(string? code, int count)
    {
        var allLocations = _panelSiloForkManager.PartitionLocations(code);
        if (allLocations == null)
        {
            return "没有有效库位";
        }

        var sortLocations = allLocations.OrderBy(x => x.DrilledItemCodes.FirstOrDefault(t => !string.IsNullOrWhiteSpace(t))).ToList().AsReadOnly();
        if (sortLocations == null || sortLocations.Count == 0)
        {
            return "没有找到有熟料的库位";
        }

        var DrillPayloadItemCodes = _drillManager.PartitionPayloadPanelSummaries(code).Select(x => x.ItemCode).ToList();
        var CountEmptyPayloadForkNow = _panelSiloForkManager.PartitionEmptyPayloadNowCount(code);//现有空位数

        return sortLocations.Select(p => new
        {
            p.Code,
            InputPartCode = code,
            InputCount = count,
            p.PartitionCode,
            p.Appointed,
            p.DrilledPanelsCount,
            DrillPayloadItemCodes,
            p.Schedule!.CreateTime,
            IsTimeOut = (DateTime.Now - p.Schedule!.CreateTime).TotalSeconds > p.Partition?.DrilledTrackOutTimeMinutes * 60,
            p.DrilledItemCodes,
            p.CanUnloadPanelSilo,
            p.ContainsDrilled,
            p.ContainsFirst,
            p.ContainsUndrilled,
            p.Partition?.IsAutoTrackOutDrilledSilo,
            p.Partition?.MinDrilledTrackOutNum,
            p.Partition?.DrilledTrackOutTimeMinutes,
            Intersect = DrillPayloadItemCodes.Intersect(p.DrilledItemCodes).Any(),
            CountEmptyPayloadForkNow,
            Cantransfer = p.ContainsDrilled
            && p.Partition?.IsAutoTrackOutDrilledSilo == true
            && p.CanUnloadPanelSilo
            && !p.ContainsFirst && !p.ContainsUndrilled
            && (p.Partition?.MinDrilledTrackOutNum <= p.DrilledPanelsCount
            || !DrillPayloadItemCodes.Intersect(p.DrilledItemCodes).Any()
            || (DateTime.Now - p.Schedule!.CreateTime).TotalSeconds > p.Partition?.DrilledTrackOutTimeMinutes * 60
            || CountEmptyPayloadForkNow <= p.Partition?.MinEmptyLocationNum)

        }).ToList();
    }

    /// <summary>
    /// 生料转入
    /// </summary>
    /// <param name="code"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    [HttpGet("RawTransIn")]
    public object? FindRawTransIn(string? code)
    {
        var allLocations = _panelSiloForkManager.PartitionLocations(code!);
        var Partition = allLocations.FirstOrDefault()?.Partition;

        Func<ItemSummary, string> keySelector = x => x.ItemCode;
        Func<IGrouping<string, ItemSummary>, ItemSummary> selector = x => new ItemSummary
        {
            ItemCode = x.Key,
            ItemCount = x.Sum(x => x.ItemCount),
        };

        var undrilledSummaryFromDrill = _drillManager.RouteRequiredUndrilledItemSummaries(Partition.RouteCodes);
        var undrilledSummariesFromFork = Partition.RackUndrilledItemSummaries;//_panelSiloForkManager.PartitionUndrilledItemSummaries(Partition.PartCode!).ToList();
        var undrilltedSummaryFromAgv = _backPanelAgvManager.RouteUndrilledItemSummaries(Partition.RouteCodes);
        var undrilledSummariesFromTransferJobs = _transferPlanManager.PartitionUndrilledItemSummaries(Partition.PartCode!);
        var countEmptyPayloadForkNow = _panelSiloForkManager.PartitionEmptyPayloadNowCount(Partition.PartCode!);//现有空位数

        var totalProvidedItems = undrilledSummariesFromFork
        .Concat(undrilltedSummaryFromAgv, keySelector, selector)
        .Concat(undrilledSummariesFromTransferJobs, keySelector, selector);

        return new
        {
            countEmptyPayloadForkNow,
            undrilledSummaryFromDrill,
            undrilledSummariesFromFork,
            undrilltedSummaryFromAgv,
            undrilledSummariesFromTransferJobs,
            totalProvidedItems,
            locationInfo = allLocations.Select(p => new
            {
                p.Code,
                PartCode = code,
                p.PartitionCode,
                p.Appointed,
                p.UndrilledItemCodes,
                p.UndrilledPanelsCount,
                p.UndrilledPanels,
                p.Available
            }).ToList()
        };
    }


    [HttpGet("GetCache")]
    public object? GetCache(string key)
    {
        if (!_memoryCacheManager.Exists(key))
        {
            return "不存在这个key或者已过期!";
        }
        else
        {
            return _memoryCacheManager.Get(key);
        }
    }


    [HttpGet("RemoveCache")]
    public object? RemoveCache(string key)
    {
        if (!_memoryCacheManager.Exists(key))
        {
            return "不存在这个key或者已过期!";
        }
        else
        {
            _memoryCacheManager.Remove(key);
        }

        return "remove success!";
    }
}

internal static class ItemSummaryList
{
    public static IEnumerable<TSource> Concat<TSource, TKey>(this IEnumerable<TSource> first,
        IEnumerable<TSource> second,
        Func<TSource, TKey> keySelector,
        Func<IGrouping<TKey, TSource>, TSource> selector)
    {
        if (first == null)
        {
            ArgumentNullException.ThrowIfNull(first);
        }
        if (second == null)
        {
            ArgumentNullException.ThrowIfNull(second);
        }

        return first.Concat(second).GroupBy(keySelector).Select(selector);
    }
}

