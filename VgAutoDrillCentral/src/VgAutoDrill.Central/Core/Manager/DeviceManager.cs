using System.Collections.Concurrent;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Extensions;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Manager;

public class DeviceManager : IDeviceManager
{
    private readonly IDeviceAdapter _deviceAdapter;
    private readonly ConcurrentDictionary<string, DeviceProxy> _deviceProxies = new();
    private readonly ILogger<DeviceManager> _logger;
    private readonly HashSet<string> _deviceCodes = new HashSet<string>();

    public DeviceManager(IServiceProvider serviceProvider)
    {
        _deviceAdapter = serviceProvider.GetRequiredService<IDeviceAdapter>();

        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<DeviceManager>();
    }

    public IReadOnlyList<Agv> Agvs => _deviceProxies.Values.Where(x => DeviceKindExtensions.IsAGV(x.DeviceKind))
        .OfType<Agv>()
        .ToList()
        .AsReadOnly();

    public IReadOnlyList<BackPanelAgv> BackPanelAgvs => _deviceProxies.Values.Where(x => x.DeviceKind == DeviceKind.BackPanelAgv)
        .OfType<BackPanelAgv>()
        .ToList()
        .AsReadOnly();

    public IReadOnlyList<Drill> Drills => _deviceProxies.Values.Where(x => DeviceKindExtensions.IsDrill(x.DeviceKind))
        .OfType<Drill>()
        .ToList()
        .AsReadOnly();

    public IReadOnlyList<Drill> OnlineDrills => OnlineDevices.Where(x => DeviceKindExtensions.IsDrill(x.DeviceKind))
        .OfType<Drill>()
        .ToList()
        .AsReadOnly();

    public IReadOnlyList<FrontPanelAgv> FrontPanelAgvs => _deviceProxies.Values.Where(x => x.DeviceKind == DeviceKind.FrontPanelAgv)
        .OfType<FrontPanelAgv>()
        .ToList()
        .AsReadOnly();

    public IReadOnlyList<PanelAgv> PanelAgvs => _deviceProxies.Values.Where(x => DeviceKindExtensions.IsPanelAGV(x.DeviceKind))
        .OfType<PanelAgv>()
        .ToList()
        .AsReadOnly();

    public IReadOnlyList<PanelSiloFork> PanelSiloForks => _deviceProxies.Values.Where(x => x.DeviceKind == DeviceKind.PanelSiloFork)
        .OfType<PanelSiloFork>()
        .ToList()
        .AsReadOnly();

    public IReadOnlyList<PanelSiloRack> PanelSiloRacks => _deviceProxies.Values.Where(x => x.DeviceKind == DeviceKind.PanelSiloFork || x.DeviceKind == DeviceKind.PublicPanelSiloWIP)
        .OfType<PanelSiloRack>()
        .ToList()
        .AsReadOnly();

    public IReadOnlyList<PanelSiloShelf> PanelSiloShelfs => _deviceProxies.Values.Where(x => x.DeviceKind == DeviceKind.PublicPanelSiloWIP)
        .OfType<PanelSiloShelf>()
        .ToList()
        .AsReadOnly();

    public IReadOnlyList<Pin> Pins => _deviceProxies.Values.Where(x => x.DeviceKind == DeviceKind.Pin)
        .OfType<Pin>()
        .ToList()
        .AsReadOnly();

    public IReadOnlyList<TransferSiloAgv> TransferSiloAgvs => _deviceProxies.Values.Where(x => x.DeviceKind == DeviceKind.ShelfSiloAgv)
        .OfType<TransferSiloAgv>()
        .ToList()
        .AsReadOnly();

    public IReadOnlyList<UnPin> UnPins => _deviceProxies.Values.Where(x => x.DeviceKind == DeviceKind.UnPin)
        .OfType<UnPin>()
        .ToList()
        .AsReadOnly();

    public async Task AddOrUpdateDevice(DeviceProxy deviceProxy)
    {
        var device = GetOnlineDevice(deviceProxy.DeviceId);
        if (device != null)
        {
            device.Status = deviceProxy.Status;
            device.ClientId = deviceProxy.ClientId;
            device.Descriptor = deviceProxy.Descriptor;
            deviceProxy.ClientIp = deviceProxy.Descriptor.HostAddress;
            deviceProxy.Properties["LogInTime"] = DateTime.Now;
            _logger.LogInformation($"new device {deviceProxy.DeviceId} logged in with ClientIp {deviceProxy.ClientIp}");
        }
        else
        {
            _logger.LogInformation($"new device {deviceProxy.DeviceId} logged in with ClientIp {deviceProxy.ClientIp}");
            await AddOrUpdateDeviceProxy(deviceProxy);
        }

        _logger.LogDebug($"deviceProxy:{JsonSerializer.Serialize(deviceProxy)}");
    }

    public List<Agv> GetCapableAgvs(ProductStatus status) => Agvs.Where(x => x.Descriptor.InputCapabilities.Contains(status)
                                                                                   && x.Descriptor.OutputCapabilities.Contains(status)
                                                                                   && x.Status == DeviceStatus.Ready).ToList();

    public IReadOnlyList<BackPanelAgv> GetIdleBackPanelAgvs(string routeCode) => BackPanelAgvs
                    .Where(agv => agv.IsIdle
                        && (string.IsNullOrEmpty(routeCode) || agv.RouteCodes.Contains(routeCode.ToLower())))
                    .ToList();

    public IReadOnlyList<FrontPanelAgv> GetIdleFrontPanelAgvs(string routeCode) => FrontPanelAgvs
                    .Where(agv => agv.IsIdle
                    && (string.IsNullOrEmpty(routeCode) || agv.RouteCodes.Contains(routeCode.ToLower())))
                    .ToList();

    public IReadOnlyList<TransferSiloAgv> GetIdleTransferSiloAgvs(string routeCode) => TransferSiloAgvs
                    .Where(agv => agv.IsIdle
                    && (string.IsNullOrEmpty(routeCode) || agv.RouteCodes.Contains(routeCode.ToLower())))
                    .ToList();

    public DeviceProxy? GetOnlineDevice(string? deviceId)
    {
        //return  _deviceProxies.Values.FirstOrDefault(x => x.Status != DeviceStatus.Offline && x.DeviceId.ToLower() == deviceId.ToLower());

        TryGetOnlineDevice(deviceId, out DeviceProxy deviceProxy);

        return deviceProxy;
    }

    public IReadOnlyList<DeviceProxy> OnlineDevices => _deviceProxies.Values.Where(x => x.Status != DeviceStatus.Offline).ToList();
    public IReadOnlyList<DeviceProxy> Devices => _deviceProxies.Values.ToList();

    //public List<string> GetOnlineSameKindDevices(DeviceKind currentDeviceKind) => _deviceProxies.Values
    //        .Where(device => DeviceKindExtensions.IsSameKind(device.Descriptor.DeviceKind, currentDeviceKind))
    //        .Select(x => x.DeviceId.ToLower())
    //        .ToList();

    public async Task OfflineDevice(string deviceId)
    {
        _logger.LogInformation($"removing device {deviceId}");
        var deviceProxy = GetOnlineDevice(deviceId);
        if (deviceProxy == null)
        {
            _logger.LogError($"cannot find this device {deviceId}");
        }
        else
        {
            deviceProxy.Status = DeviceStatus.Offline;
            _logger.LogInformation($"removed device {deviceId}");
            await AddOrUpdateDeviceProxy(deviceProxy);
        }
    }

    public async Task RefreshAgvRouteCodes()
    {
        var relations = await _deviceAdapter.GetAgvRouteCodes();
        foreach (var device in PanelAgvs)
        {
            var routeCodes = relations.FirstOrDefault(x => x.DeviceId.ToLower() == device.DeviceId.ToLower())?.RouteCodes;
            device.RefreshRouteCodes(routeCodes ?? new List<string>());
        }
    }

    public async Task RefreshDrillRouteCodes()
    {
        var relations = await _deviceAdapter.GetDrillRouteCodes();

        foreach (var drill in Drills)
        {
            var routeCodes = relations.FirstOrDefault(x => x.DeviceId.ToLower() == drill.DeviceId.ToLower())?.RouteCodes;
            drill.RefreshRouteCodes(routeCodes ?? new List<string>());
        }
    }

    public async Task RemoveDeviceByClient(string clientId)
    {
        _logger.LogInformation($"removing device {clientId}");
        var predicate = (DeviceProxy device) => device.ClientId == clientId;
        if (_deviceProxies.Values.Any(predicate))
        {
            foreach (var deviceProxy in _deviceProxies.Values.Where(predicate))
            {
                deviceProxy.Status = DeviceStatus.Offline;
                _logger.LogInformation($"removed device {clientId} {deviceProxy.DeviceId}");
                await AddOrUpdateDeviceProxy(deviceProxy);
            }
        }
    }

    public bool TryGetIdleBackPanelAgv(string routeCode, out BackPanelAgv? panelAgv)
    {
        panelAgv = null;
        if (string.IsNullOrEmpty(routeCode)) return false;

        panelAgv = GetIdleBackPanelAgvs(routeCode).FirstOrDefault();
        return panelAgv != null;
    }

    public bool TryGetIdleTransferSiloAgv(string routeCode, out TransferSiloAgv? panelAgv)
    {
        panelAgv = null;
        if (string.IsNullOrEmpty(routeCode)) return false;

        panelAgv = GetIdleTransferSiloAgvs(routeCode).FirstOrDefault();
        return panelAgv != null;
    }

    public bool TryGetOnlineDrill(string deviceId, out Drill? deviceProxy)
    {
        if (TryGetOnlineDevice(deviceId, out Drill drill)
            && drill != null)
        {
            deviceProxy = drill;

            return true;
        }

        deviceProxy = null;
        return false;
    }

    public bool TryGetLocalDevice<T>(string deviceId, out T? deviceProxy)
        where T : DeviceProxy
    {
        _deviceProxies.TryGetValue(deviceId.ToLower(), out var device);
        deviceProxy = device as T;

        return deviceProxy != null;
    }

    public bool TryGetOnlineDevice<T>(string? deviceId, out T? deviceProxy)
        where T : DeviceProxy
    {
        _deviceProxies.TryGetValue(deviceId.ToLower(), out var device);

        if (device?.Status != DeviceStatus.Offline)
        {
            deviceProxy = device as T;

            return deviceProxy != null;
        }

        deviceProxy = null;
        return false;
    }

    /// <summary>
    /// 由数据库进行判定，是否需要新增数据或者修改现有的数据
    /// </summary>
    /// <param name="deviceProxy"></param>
    /// <returns></returns>
    private async Task AddOrUpdateDB(DeviceProxy deviceProxy)
    {
        _logger.LogInformation($"Device:{deviceProxy.DeviceId} connected to central system.");
        await _deviceAdapter.UpdateStatus(deviceProxy);
    }

    private async Task AddOrUpdateDeviceProxy(DeviceProxy deviceProxy)
    {
        RefreshLocal(deviceProxy);
        _ = AddOrUpdateDB(deviceProxy);
    }

    private void RefreshLocal(DeviceProxy deviceProxy)
    {
        if (deviceProxy.Status != DeviceStatus.Offline)
        {
            if (_deviceProxies.ContainsKey(deviceProxy.DeviceId.ToLower()))
            {
                var device = _deviceProxies[deviceProxy.DeviceId.ToLower()];
                device.Descriptor = deviceProxy.Descriptor;
                device.ClientId = deviceProxy.ClientId;
                device.ClientIp = deviceProxy.ClientIp;
                device.Status = deviceProxy.Status;
                device.Properties["ClientIp"] = deviceProxy.ClientIp;
            }
            else
            {
                _deviceProxies[deviceProxy.DeviceId.ToLower()] = deviceProxy;
            }
        }
    }
}
