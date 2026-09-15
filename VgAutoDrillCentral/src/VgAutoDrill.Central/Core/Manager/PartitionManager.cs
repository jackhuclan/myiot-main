using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.AgvRestAndPart;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Transportation;
using VgAutoDrill.Infrastructure;
using Partition = VgAutoDrill.Central.Core.Mes.Model.Partition;

namespace VgAutoDrill.Central.Core.Manager;

public class PartitionManager : IPartitionManager
{
    private readonly IRestPointAdapter _restPointAdapter;
    private readonly IPartitionAdapter _partitionAdapter;
    private readonly ILogger<PartitionManager> _logger;
    private readonly IPartitionService _partitionService;
    private readonly IAgvRestAndPartService _agvRestAndPartService;
    private readonly ISysConfigManager _sysConfigManager;
    private readonly IObjectFactory _objectFactory;
    private readonly ConcurrentDictionary<string, Partition> _partitions = new();
    private readonly ConcurrentDictionary<string, RestPoint> _restPoints = new();
    private static readonly List<PartitionRelation> _partitionRelations = new();

    public PartitionManager(IServiceProvider serviceProvider)
    {
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _objectFactory = serviceProvider.GetRequiredService<IObjectFactory>();
        _restPointAdapter = serviceProvider.GetRequiredService<IRestPointAdapter>();
        _partitionAdapter = serviceProvider.GetRequiredService<IPartitionAdapter>();
        _partitionService = serviceProvider.GetRequiredService<IPartitionService>();
        _agvRestAndPartService = serviceProvider.GetRequiredService<IAgvRestAndPartService>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<PartitionManager>();
    }

    public IReadOnlyList<Partition> Partitions => _partitions.Values.ToList().AsReadOnly();
    public IReadOnlyList<Partition> PublicEmptySiloPartitions => _partitions.Values.Where(x => x.SiloKind == TransportationKind.EmptySilo && x.PartitionKind == PartitionKind.Public).ToList().AsReadOnly();
    public IReadOnlyList<Partition> PublicRawPartitions => _partitions.Values.Where(x => x.SiloKind == TransportationKind.Raw && x.PartitionKind == PartitionKind.Public).ToList().AsReadOnly();
    public IReadOnlyList<Partition> PublicClinkerPartitions => _partitions.Values.Where(x => x.SiloKind == TransportationKind.Clinker && x.PartitionKind == PartitionKind.Public).ToList().AsReadOnly();
    public IReadOnlyList<Partition> PublicFirstPartitions => _partitions.Values.Where(x => x.SiloKind == TransportationKind.First && x.PartitionKind == PartitionKind.Public).ToList().AsReadOnly();
    public IReadOnlyList<RestPoint> RestPoints => _restPoints.Values.ToList().AsReadOnly();
    public IReadOnlyList<PartitionRelation> PartitionRelations => _partitionRelations.ToList().AsReadOnly();

    public IReadOnlyList<RestPoint> BookedRestPoints => RestPoints.Where(p => !string.IsNullOrEmpty(p.PreBookAGV) && string.IsNullOrEmpty(p.CurrentAgv))
                    .ToList()
                    .AsReadOnly();

    public async Task Refresh()
    {
        await RefreshRestPoints();
        await RefreshPartitions();
        await RefreshPartitionRouteCodes();
        await RefreshPartitionRelations();
    }

    public bool TryGetPartition(string partitionCode, out Partition? partition)
    {
        return _partitions.TryGetValue(partitionCode, out partition);
    }

    public bool TryGetPartition(IReadOnlyList<string> routeCodes, out Partition? partition)
    {
        partition = _partitions.Values.FirstOrDefault(x => x.RouteCodes.Intersect(routeCodes).Any());
        return partition != null;
    }

    public async Task<string> SetCarPosition(string carDeviceId, string carCurrentPos)
    {
        //update rest 当前点位
        await _partitionAdapter.RefreshCurrentAgv(carDeviceId, carCurrentPos);

        //update rest 当前点位
        var restPoint = RestPoints.Where(x => !string.IsNullOrEmpty(x.Point) && x.Point.ToLower() == carCurrentPos.ToLower().Trim()).ToList(); ;

        foreach (var rel in restPoint)
        {
            if (rel.CurrentAgv.ToLower() != carDeviceId.ToLower())
            {
                rel.CurrentAgv = carDeviceId;
            }
            else
            {
                rel.PreBookAGV = string.Empty;
            }
        }

        return string.Empty;
    }

    public async Task<RestPoint?> TryBookRest(Agv agv)
    {
        if (RestPoints.Any(x => x.PreBookAGV.ToLower() == agv.DeviceId.ToLower() || x.CurrentAgv.ToLower() == agv.DeviceId.ToLower()))
        {
            _logger.LogDebug($"{agv.DeviceId},已经预约了休息点或者已经到达休息点");
            return null;
        }

        var _relations = _partitionRelations.Where(t => t.AgvDeviceKind == agv.DeviceKind
            && t.Status == 1
            && t.RestPoint.Status == 1
            && string.IsNullOrEmpty(t.RestPoint.PreBookAGV)
            && string.IsNullOrEmpty(t.RestPoint.CurrentAgv));

        //todo  分区
        //根据车辆当前的位置，就近查找休息点
        //如果增加了分区后，找不到休息点时，则不限制分区 再查找一次；
        var partitionCode = "";
        if (!string.IsNullOrEmpty(partitionCode))
        {
            _relations = _relations.Where(t => t.Partition.PartCode == partitionCode);
        }

        if (agv.DeviceKind == DeviceKind.BackPanelAgv || agv.DeviceKind == DeviceKind.FrontPanelAgv)
        {
            //工艺路线
            var routeCodes = agv.RouteCodes;
            if (routeCodes.Any())
            {
                _relations = _relations.Where(t => !string.IsNullOrEmpty(t.RouteCode) && routeCodes.Contains(t.RouteCode.ToLower())).OrderBy(t => t.Priority);
            }
        }

        var matchRelation = _relations.FirstOrDefault();
        if (matchRelation == null) return null;

        await _partitionAdapter.HandleBookRest(matchRelation.RestPoint.RestCode, agv.DeviceId);
        matchRelation.RestPoint.PreBookAGV = agv.DeviceId;

        return matchRelation.RestPoint;
    }

    public async Task<string> TryUnbookPart(string partitionCode)
    {
        var partition = _partitions.Values.FirstOrDefault(x => x.PartCode?.ToLower() == partitionCode?.ToLower());

        if (partition != null)
        {
            await _partitionAdapter.UnbookPart(partitionCode);
            partition.PreBookAGV = string.Empty;
            return string.Empty;
        }

        return "未找到分区，无法取消";
    }

    public async Task<string> TryUnbookPartition(string agvDeviceId)
    {
        var partition = _partitions.Values.FirstOrDefault(x => !string.IsNullOrEmpty(x.PreBookAGV) && x.PreBookAGV.ToLower() == agvDeviceId.ToLower());
        if (partition != null && !string.IsNullOrEmpty(partition.PartCode))
        {
            return await TryUnbookPart(partition.PartCode);
        }

        return "未找到分区";
    }

    public async Task<BookPartResult> TryBookPartition(Partition partition, Agv agvDeviceProxy)
    {
        var bookResult = await _partitionService.BookPart(partition.PartCode, agvDeviceProxy.DeviceId);

        if (string.IsNullOrEmpty(bookResult))
        {
            partition.PreBookAGV = agvDeviceProxy.DeviceId;
            return new BookPartResult { };
        }
        else
        {
            return new BookPartResult
            {
                ResponseMessage = bookResult,
            };
        }
    }

    public async Task<BookPartResult> TryBookPartition(Agv agv, Location location)
    {
        if (!string.IsNullOrEmpty(location.Partition.PreBookAGV) && agv.DeviceId.ToLower() == location.Partition.PreBookAGV.ToLower())
        {
            return new BookPartResult
            {
                PartitionCode = location.Partition.PartCode,
                ResponseMessage = string.Empty,
            };
        }
        else
        {
            var bookSolution = await TryBookPartition(location.Partition, agv);
            if (string.IsNullOrEmpty(bookSolution.ResponseMessage))
            {
                _logger.LogWarning($"预约分区或者休息点成功，response:{bookSolution.ResponseMessage}");
            }
            else
            {
                _logger.LogWarning($"预约分区或者休息点失败，response:{bookSolution.ResponseMessage}");
            }

            return bookSolution;
        }
    }

    public bool TryGetRestPoint(string restCode, out RestPoint? restPoint)
    {
        return _restPoints.TryGetValue(restCode, out restPoint);
    }

    public bool TryFindBookedRest(string carDeviceId, out RestPoint? restPoint)
    {
        restPoint = BookedRestPoints.FirstOrDefault(x => x.PreBookAGV.ToLower() == carDeviceId.ToLower());
        return restPoint != null;
    }

    private async Task RefreshRestPoints()
    {
        var restPoints = await _restPointAdapter.GetRestPoints();
        foreach (var restPoint in restPoints)
        {
            if (_restPoints.ContainsKey(restPoint.RestCode))
            {
                _restPoints[restPoint.RestCode].RestName = restPoint.RestName;
                _restPoints[restPoint.RestCode].PreBookAGV = restPoint.PreBookAGV;
                _restPoints[restPoint.RestCode].CurrentAgv = restPoint.CurrentAgv;
                _restPoints[restPoint.RestCode].Status = restPoint.Status;
                _restPoints[restPoint.RestCode].Point = restPoint.Point;
                _restPoints[restPoint.RestCode].Priority = restPoint.Priority;
            }
            else
            {
                _restPoints.TryAdd(restPoint.RestCode, restPoint);
            }
        }
    }

    private async Task RefreshPartitions()
    {
        var partitionDtos = await _partitionAdapter.GetPartitions();

        foreach (var partitionDto in partitionDtos)
        {
            var partitionKind = (PartitionKind)(int)partitionDto.PartitionKind;

            bool exist = _partitions.TryGetValue(partitionDto.Code, out var partition);

            if (!exist)
            {
                if (partitionKind == PartitionKind.Private)
                {
                    partition = _objectFactory.CreateObject<PrivatePartition>(partitionDto.Code, partitionKind);
                }
                else if (partitionKind == PartitionKind.Public)
                {
                    partition = _objectFactory.CreateObject<PublicPartition>(partitionDto.Code, partitionKind);
                }
            }

            if (partition == null)
            {
                continue;
            }

            partition.PartCode = partitionDto.Code;
            partition.PartName = partitionDto.Name;
            partition.PreBookAGV = partitionDto.PreBookAgv ?? string.Empty;
            partition.PreBookTime = partitionDto.PreBookTime;
            partition.PartitionKind = partitionKind;
            partition.SiloKind = (TransportationKind)(int)partitionDto.TransportationKind;
            partition.MinEmptyLocationNum = partitionDto.MinEmptyLocationNum;
            partition.MinEmptyBoxNum = partitionDto.MinEmptyBoxNum;
            partition.MaxEmptyBoxNum = partitionDto.MaxEmptyBoxNum;
            partition.MinDrilledTrackOutNum = partitionDto.MinDrilledTrackOutNum;
            partition.IsAutoTrackOutDrilledSilo = partitionDto.IsAutoTrackOutDrilledSilo;
            partition.RawTrackOutTimeOutTime = partitionDto.RawTrackOutTimeOutTime;
            partition.MinFirstTrackOutNum = partitionDto.MinFirstTrackOutNum;
            partition.DrilledTrackOutTimeMinutes = partitionDto.DrilledTrackOutTimeMinutes;
            partition.Status = partitionDto.Status;

            if (!exist)
            {
                _partitions.TryAdd(partitionDto.Code, partition);
            }
        }
    }

    private async Task RefreshPartitionRouteCodes()
    {
        var partitionSetting = await _sysConfigManager.GetStringValue("PartitionSetting", Admin.Model.Enum.SysConfigCategoryEnum.None, false);

        partitionSetting.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
            .ForEach(x =>
            {
                var part = x.Split(':')[0];
                var routeCodes = x.Split(':')[1].ToLower().Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

                if (_partitions.TryGetValue(part, out var partition)
                    && partition != null)
                {
                    if (routeCodes.Any())
                    {
                        routeCodes.ForEach(x => x = x.Trim().ToLower());
                        partition.RouteCodes.Clear();
                        partition.RouteCodes.AddRange(routeCodes);
                    }
                    else
                    {
                        partition.RouteCodes.Clear();
                    }
                }
            });
    }

    private async Task RefreshPartitionRelations()
    {
        _partitionRelations.Clear();
        var relations = await _agvRestAndPartService.GetList(new GetRestAndPartListReq
        {
            PageSize = int.MaxValue,
        });

        foreach (var rel in relations.Data.List)
        {
            if (TryGetPartition(rel.PartCode, out var part)
                && part != null
                && TryGetRestPoint(rel.RestCode, out var rest)
                && rest != null)
            {
                _partitionRelations.Add(new PartitionRelation
                {
                    Partition = part,
                    RestPoint = rest,
                    AgvDeviceKind = (Fundation.Iot.Models.DeviceKind)rel.AgvDeviceKind,
                    Priority = rel.Priority.Value,
                    RouteCode = rel.RouteCode,
                    Status = rel.Status
                });
            }
        }
    }
}
