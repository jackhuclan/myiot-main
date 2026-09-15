using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using Panel = VgAutoDrill.Fundation.Iot.Models.Panel;
using Partition = VgAutoDrill.Central.Core.Mes.Model.Partition;

namespace VgAutoDrill.Central.Core.Manager;

public class LocationManager : ILocationManager
{
    private readonly ILocationAdapter _locationAdapter;
    private readonly ConcurrentDictionary<string, Location> _locations = new();
    private readonly IDeviceManager _deviceManager;
    private readonly IPartitionManager _partitionManager;

    private Predicate<Location> availableLocationPredicate => x => x.Available;

    private Predicate<Location> allocatedLocationPredicate =>
                        x => x.Appointed
                            && x.Panels.Any()
                            && x.Schedule != null
                            && x.Schedule.ScheduledTaskStatus == ScheduledTaskStatus.Allocated;

    private Predicate<Location> runningLocationPredicate =>
                        x => x.Appointed
                            && x.Panels.Any()
                            && x.Schedule != null
                            && x.Schedule.ScheduledTaskStatus == ScheduledTaskStatus.Running;

    public LocationManager(IServiceProvider serviceProvider)
    {
        _locationAdapter = serviceProvider.GetRequiredService<ILocationAdapter>();
        _deviceManager = serviceProvider.GetRequiredService<IDeviceManager>();
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
    }

    public IReadOnlyList<Location> Locations => _locations.Values.ToList().AsReadOnly();

    public IReadOnlyList<Location> OnlineLocations => Locations.Where(x => !string.IsNullOrEmpty(x.Code)
                                                    && !string.IsNullOrEmpty(x.DeviceId)
                                                    && x.HostDevice != null).ToList().AsReadOnly();

    /// <summary>
    /// 已经被分配的库位
    /// </summary>
    public IReadOnlyList<Location> AllocatedLocations => OnlineLocations.Where(x => allocatedLocationPredicate(x)).ToList().AsReadOnly();

    /// <summary>
    /// 正在被agv执行任务的库位
    /// </summary>
    public IReadOnlyList<Location> RunningLocations => OnlineLocations.Where(x => runningLocationPredicate(x)).ToList().AsReadOnly();

    public IReadOnlyList<Location> AvailableLocations => OnlineLocations.Where(x => availableLocationPredicate(x)).ToList().AsReadOnly();

    public IReadOnlyList<Panel> GetLocationPanels(string locationCode) => OnlineLocations
        .Where(x => !string.IsNullOrEmpty(locationCode) && x.Code.ToLower() == locationCode.ToLower())
        .SelectMany(x => x.Panels)
        .OrderBy(x => x.Position)
        .ToList()
        .AsReadOnly();

    public IReadOnlyList<Panel> GetDevicePanels(string deviceId) => OnlineLocations
        .Where(x => !string.IsNullOrEmpty(deviceId)
            && x != null
            && x.DeviceId.ToLower() == deviceId.ToLower()
            && x.Panels != null
            && x.Panels.Any())
        .SelectMany(location => location.Panels)
        .Where(panel => panel != null)
        .OrderBy(panel => panel.Position)
        .ToList()
        .AsReadOnly();

    /// <summary>
    /// 库位数
    /// </summary>
    public int LocationCount() => Locations.Count;

    public Location? GetLocation(string locationCode) => Locations.FirstOrDefault(x => x.Code == locationCode);

    public Location? GetLocation(string deviceId, int position) => Locations.FirstOrDefault(x => x.DeviceId == deviceId && x.Position == position);

    public IReadOnlyList<Location> GetLocations(string deviceId) => Locations.Where(x => x.DeviceId == deviceId).ToList().AsReadOnly();

    public Partition? GetPartition(string locationCode) => GetLocation(locationCode)?.Partition;

    public IReadOnlyList<Partition> Partitions => Locations.Where(x => x.Partition != null)
        .Select(p => p.Partition!)
        .ToList()
        .AsReadOnly();

    /// <summary>
    /// 统计包含所给物料状态的库位数量
    /// </summary>
    /// <param name="productStatuses">所给物料状态</param>
    /// <returns></returns>
    public int LocationCount(IReadOnlyList<ProductStatus> productStatuses)
    {
        return OnlineLocations.Where(x => x.ContainProductStatuses(productStatuses)).Count();
    }

    /// <summary>
    /// 统计包含所给物料状态的库位数量
    /// </summary>
    /// <param name="productStatus">所给物料状态</param>
    /// <returns></returns>
    public int LocationCount(ProductStatus productStatus)
    {
        return OnlineLocations.Where(x => x.ContainProductStatus(productStatus)).Count();
    }

    public bool HasLocation(string locationCode)
    {
        if (_locations.TryGetValue(locationCode, out var _))
        {
            return true;
        }

        return false;
    }

    public bool TryGetLocation(string locationCode, out Location? location)
    {
        if (string.IsNullOrEmpty(locationCode)
            || !_locations.ContainsKey(locationCode))
        {
            location = null;
            return false;
        }

        location = _locations[locationCode];
        return true;
    }

    public bool TryAddOrUpdateLocation(Location location)
    {
        if (location == null
            || string.IsNullOrEmpty(location.Code))
            return false;

        _locations.AddOrUpdate(location.Code,
           location,
           (code, oldLocation) =>
           {
               _locations.TryRemove(location.Code, out _);
               _locations.TryAdd(location.Code, location);
               return location;
           });

        UpdatePartition(location);
        UpdateDevice(location);

        return true;
    }

    public async Task Refresh()
    {
        var locations = await _locationAdapter.GetLocations() ?? new List<Location>();

        var forkLocationCodesFromDb = locations
            .Where(x => x.LocationDeviceKind == DeviceKind.PanelSiloFork
                    || x.LocationDeviceKind == DeviceKind.PublicPanelSiloWIP
                    || x.LocationDeviceKind == DeviceKind.Pin
                    || x.LocationDeviceKind == DeviceKind.UnPin)
            .Select(x => x.Code.ToLower())
            .ToList();

        var ignoreLocations = _locations.Values
            .Where(x => (x.LocationDeviceKind == DeviceKind.PanelSiloFork
                    || x.LocationDeviceKind == DeviceKind.PublicPanelSiloWIP
                    || x.LocationDeviceKind == DeviceKind.Pin
                    || x.LocationDeviceKind == DeviceKind.UnPin)
                    && !forkLocationCodesFromDb.Contains(x.Code.ToLower()))
            .ToList();

        foreach (var location in ignoreLocations)
        {
            _locations.TryRemove(location.Code, out _);
        }

        foreach (var location in locations)
        {
            if (_locations.ContainsKey(location.Code))
            {
                _locations[location.Code].PartitionCode = location.PartitionCode;
                _locations[location.Code].Status = location.Status;
                _locations[location.Code].FeedAGVInnerPoint = location.FeedAGVInnerPoint;
                _locations[location.Code].FeedAGVOutputPoint = location.FeedAGVOutputPoint;
                _locations[location.Code].FeedAGVRestPoint = location.FeedAGVRestPoint;
                _locations[location.Code].TransAGVInnerPoint = location.TransAGVInnerPoint;
                _locations[location.Code].TransAGVOutputPoint = location.TransAGVOutputPoint;
                _locations[location.Code].TransAGVRestPoint = location.TransAGVRestPoint;
                _locations[location.Code].DeviceId = location.DeviceId;
                _locations[location.Code].PositionCode = location.PositionCode;

                UpdatePartition(location);
                UpdateDevice(location);
            }
            else
            {
                _locations.TryAdd(location.Code, location);
                UpdatePartition(location);
                UpdateDevice(location);
            }
        }
    }

    private void UpdateDevice(Location location)
    {
        if (!string.IsNullOrEmpty(location.DeviceId)
            && _deviceManager.TryGetOnlineDevice<DeviceProxy>(location.DeviceId, out var device)
            && device != null)
        {
            _locations[location.Code].HostDevice = device;
        }
    }

    private void UpdatePartition(Location location)
    {
        if (!string.IsNullOrEmpty(location.PartitionCode)
            && _partitionManager.TryGetPartition(location.PartitionCode, out var partition)
            && partition != null)
        {
            _locations[location.Code].PartitionCode = partition.PartCode ?? string.Empty;
            _locations[location.Code].Partition = partition;
        }
    }

    public int GetEmptyLayerCount()
        => Locations.Where(x => x.HasSilo).Sum(x => x.EmptySiloBoxCount);

    public int GetEmptyLayerCount(string siloCode)
        => Locations.Where(x => x.HasSilo && x.SiloCode.ToLower() == siloCode.ToLower()).Sum(x => x.EmptySiloBoxCount);

    public int GetDeviceEmptyLayerCount(string deviceId)
        => Locations.Where(x => x.HasSilo
        && x.DeviceId.ToLower() == deviceId.ToLower()).Sum(x => x.EmptySiloBoxCount);

    public int GetDeviceEmptyLayerCount(string deviceId, string siloCode)
        => Locations.Where(x => x.HasSilo
        && x.SiloCode.ToLower() == siloCode.ToLower()
        && x.DeviceId.ToLower() == deviceId.ToLower()).Sum(x => x.EmptySiloBoxCount);

    public int SiloCount()
        => Locations.Where(x => x.HasSilo)
        .Select(x => x.SiloCode).Distinct().Count();

    public int SiloCountOfContains(string itemCode)
        => Locations.Where(x => x.HasSilo && x.ContainsItemCode(itemCode))
        .Select(x => x.SiloCode).Distinct().Count();

    public int SiloCount(IReadOnlyList<ProductStatus> productStatuses)
        => Locations.Where(x => x.HasSilo && x.ContainProductStatuses(productStatuses))
        .Select(x => x.SiloCode).Distinct().Count();

    public int PanelCount()
        => Locations.Sum(x => x.PanelCount());

    public int PanelCount(string siloCode)
        => Locations.Where(x => x.HasSilo && x.SiloCode.ToLower() == siloCode.ToLower()).Sum(x => x.PanelCount());

    public int PanelCount(ProductStatus productStatus)
        => Locations.Where(x => x.HasSilo && x.ContainProductStatus(productStatus)).Sum(x => x.PanelCount());

    public int PanelCount(IReadOnlyList<ProductStatus> productStatuses)
        => Locations.Where(x => x.HasSilo && x.ContainProductStatuses(productStatuses)).Sum(x => x.PanelCount());

    public int PanelCount(string siloCode, IReadOnlyList<ProductStatus> productStatuses)
        => Locations.Where(x => x.HasSilo && x.SiloCode.ToLower() == siloCode.ToLower() && x.ContainProductStatuses(productStatuses)).Sum(x => x.PanelCount());

    public int SiloCount(string siloCode)
        => Locations.Where(x => x.HasSilo && x.SiloCode.ToLower() == siloCode.ToLower()).Count();

    public int PanelCount(string deviceId, string siloCode, ProductStatus productStatus)
        => Locations.Where(x => x.HasSilo
        && x.SiloCode.ToLower() == siloCode.ToLower()
        && x.DeviceId.ToLower() == deviceId.ToLower()
        && x.ContainProductStatus(productStatus)).Sum(x => x.PanelCount());

    public int PanelCount(string deviceId, string siloCode, ReadOnlyCollection<ProductStatus> productStatuses)
        => Locations.Where(x => x.HasSilo
        && x.SiloCode.ToLower() == siloCode.ToLower()
        && x.DeviceId.ToLower() == deviceId.ToLower()
        && x.ContainProductStatuses(productStatuses)).Sum(x => x.PanelCount());

    public int PanelCount(string siloCode, ProductStatus productStatus)
        => Locations.Where(x => x.HasSilo
        && x.SiloCode.ToLower() == siloCode.ToLower()
        && x.ContainProductStatus(productStatus)).Sum(x => x.PanelCount());

    public bool TryFindLocationWithSiloCode(string siloCode, out Location? location)
    {
        location = Locations.FirstOrDefault(x => x.HasSilo && x.SiloCode.ToLower() == siloCode.ToLower());
        return location != null;
    }

    public Location Create(string locationCode)
    {
        return new Location() { Code = locationCode };
    }
}
