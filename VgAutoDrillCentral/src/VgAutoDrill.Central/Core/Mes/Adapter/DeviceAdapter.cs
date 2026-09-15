using System.Collections.Concurrent;
using System.Text.Json;
using AutoMapper;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.CentralModels;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceAndRoute;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DevicePanel;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceRecords;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillRateFactor;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.Core.Mes.Adapter;

public class DeviceAdapter : IDeviceAdapter
{
    private readonly IDeviceService _deviceService;
    private readonly IDeviceAndRouteService _deviceAndRouteService;
    private readonly IDevicePanelService _devicePanelService;
    private readonly IRouteProcessAndWorkStationService _routeProcessAndWorkStationService;
    private readonly IMapper _mapper;
    private readonly ILogger<DeviceAdapter> _logger;
    private readonly IDeviceRecordsService _deviceRecordsService;
    private readonly IDrillRateFactorService _drillRateFactorService;
    private readonly ISysConfigManager _sysConfigManager;

    public DeviceAdapter(IDeviceService deviceService,
        IDeviceAndRouteService deviceAndRouteService,
        IDevicePanelService devicePanelService,
        IRouteProcessAndWorkStationService routeProcessAndWorkStationService,
        IMapper mapper,
        ILogger<DeviceAdapter> logger,
        IDeviceRecordsService deviceRecordsService,
        IDrillRateFactorService drillRateFactorService,
        ISysConfigManager sysConfigManager)
    {
        _deviceService = deviceService;
        _deviceAndRouteService = deviceAndRouteService;
        _devicePanelService = devicePanelService;
        _routeProcessAndWorkStationService = routeProcessAndWorkStationService;
        _mapper = mapper;
        _logger = logger;
        _deviceRecordsService = deviceRecordsService;
        _drillRateFactorService = drillRateFactorService;
        _sysConfigManager = sysConfigManager;
    }

    public async Task<DeviceDto> GetDevice(string deviceId)
    {
        var device = await _deviceService.FindSingle(deviceId);
        if (device == null)
        {
            return null;
        }

        return _mapper.Map<DeviceDto>(device);
    }

    public async Task<List<string>> GetRouteCodes(string deviceId)
    {
        var routeCodes = new List<string>();
        var configs = await _deviceAndRouteService.GetList(new Admin.Model.ViewModels.Mes.DeviceAndRoute.GetDeviceAndRouteListReq { DeviceCode = deviceId });
        if (configs.Data != null && configs.Data.List != null && configs.Data.List.Where(d => !string.IsNullOrEmpty(d.RouteCode)).Any())
        {
            var codes = configs.Data.List.Where(d => !string.IsNullOrEmpty(d.RouteCode)).Select(d => d.RouteCode.ToStr()).Distinct().ToList();
            if (codes != null && codes.Any())
            {
                routeCodes.AddRange(codes);
            }
        }

        return routeCodes;
    }

    public async Task UpdateStatus(DeviceProxy deviceProxy)
    {
        try
        {
            var adminDeviceDescriptor = JsonSerializer.Deserialize<DeviceDescriptor>(JsonSerializer.Serialize(deviceProxy.Descriptor));

            var result = await _deviceService.UpdateStatus(adminDeviceDescriptor, (DeviceStatus)deviceProxy.Status);

            if (result.Code == ResponseCode.Fail)
            {
                _logger.LogWarning($"device:{deviceProxy.DeviceId}, newStatus:{deviceProxy.Status},message:{result.Message}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, " UpdateStatus，转换失败 Descriptor");
        }
    }

    public async Task<List<DeviceRouteCodesPair>> GetDrillRouteCodes()
    {
        var relations = await _routeProcessAndWorkStationService.GetDrillRouteCodes();
        return relations.Where(x => !string.IsNullOrEmpty(x.Code)
                            && x.RouteCodes != null
                            && x.RouteCodes.Any())
                        .GroupBy(x => x.Code)
                        .Select(x => new DeviceRouteCodesPair()
                        {
                            DeviceId = x.Key!,
                            RouteCodes = x.SelectMany(i => i.RouteCodes.Select(r => r.ToLower())).ToList()
                        }).ToList();
    }

    public async Task<List<DeviceRouteCodesPair>> GetAgvRouteCodes()
    {
        var relations = await _deviceAndRouteService.GetList(new GetDeviceAndRouteListReq { PageSize = int.MaxValue });
        return relations.Data.List
                .Where(x => !string.IsNullOrEmpty(x.DeviceCode) && !string.IsNullOrEmpty(x.RouteCode))
                .Distinct()
                .ToList()
                .GroupBy(x => x.DeviceCode)
                    .Select(x => new DeviceRouteCodesPair()
                    {
                        DeviceId = x.Key!,
                        RouteCodes = x.Select(i => i.RouteCode?.ToLower() ?? string.Empty).Distinct().ToList()
                    }).ToList();
    }

    public async Task PersistPanels(string productId, string deviceId, PanelList panels)
    {
        var items = new List<AddOrUpdateDevicePanelReq>();
        foreach (var panel in panels)
        {
            items.Add(new AddOrUpdateDevicePanelReq
            {
                DeviceCode = deviceId,
                PanelCode = panel.PanelCode,
                SiloCode = panel.SiloCode,
                ItemCode = panel.ItemCode,
                LotId = panel.LotId,
                BatchCode = panel.BatchCode,
                Layer = panel.Layer,
                Position = panel.Position,
                ProductStatus = panel.ProductStatus,
                LocationCode = panels.LocationCode,
                PinOffset = (decimal)panel.PinOffset,
                PanelWidth = (decimal)panel.PanelWidth,
            });
        }

        await _devicePanelService.BulkInsert(items);
    }

    public Task PersistCutters(string productId, string deviceId, CutterTrays payloadCutterTrays)
    {
        return Task.CompletedTask;
    }

    public async Task PersistProperties(string productId, string deviceId, ConcurrentDictionary<string, object> Properties)
    {
        if (string.IsNullOrEmpty(productId) || productId.ToLower() != "drill")
        {
            _logger.LogInformation("PersistProperties productId is not drill !");
            return;
        }
        if (string.IsNullOrEmpty(deviceId) || Properties == null)
        {
            _logger.LogError("PersistProperties deviceId is null or Properties is null !");
            return;
        }
        var workTime = Properties.ContainsKey("Drill_CurrentWorktime") ? Properties["Drill_CurrentWorktime"].ToStr() : string.Empty;
        var waitTime = Properties.ContainsKey("Drill_CurrentWaittime") ? Properties["Drill_CurrentWaittime"].ToStr() : string.Empty;
        var errorTime = Properties.ContainsKey("Drill_CurrentErrortime") ? Properties["Drill_CurrentErrortime"].ToStr() : string.Empty;
        var openTime = Properties.ContainsKey("Drill_CurrentOpentime") ? Properties["Drill_CurrentOpentime"].ToStr() : string.Empty;
        var duty = Properties.ContainsKey("Drill_CurrentDuty") ? Properties["Drill_CurrentDuty"].ToStr() : string.Empty;
        var endToStartTime = Properties.ContainsKey("Drill_EndToStartTime") ? Properties["Drill_EndToStartTime"].ToStr() : string.Empty;
        var drillLocalTime = Properties.ContainsKey("Drill_LocalTime") ? Properties["Drill_LocalTime"].ToStr() : string.Empty;

        if (string.IsNullOrEmpty(workTime) && string.IsNullOrEmpty(waitTime) && string.IsNullOrEmpty(errorTime)
            && string.IsNullOrEmpty(openTime) && string.IsNullOrEmpty(duty) && string.IsNullOrEmpty(endToStartTime)
            && string.IsNullOrEmpty(drillLocalTime))
        {
            _logger.LogInformation($"PersistProperties deviceId {deviceId} 未识别有效的 WorkTime、WaitTime 、ErrorTime、OpenTime、Duty、EndToStartTime、Drill_LocalTime !");
            return;
        }

        //todo, 分析钻机的无任务时间点，结合白班、晚班（或者 还有中班）

        await _deviceRecordsService.Add(new AddOrUpdateDeviceRecordsReq
        {
            DeviceCode = deviceId,
            WaitTime = waitTime,
            WorkTime = workTime,
            DateString = drillLocalTime,
            Duty = duty,
            EndToStartTime = endToStartTime,
            ErrorTime = errorTime,
            OpenTime = openTime,
        });
    }

    public async Task ReCordDrillRateFactor(string deviceId, DrillRateFactorReason reason, DateTime startTime, DateTime endTime)
    {
        if (string.IsNullOrEmpty(deviceId))
        {
            _logger.LogInformation("ReCordDrillRateFactor deviceId is null !");
            return;
        }

        await _drillRateFactorService.AddOrUpdate(new AddOrUpdateDrillRateFactorReq
        {
            DeviceId = deviceId,
            Reason = reason,
            StartTime = startTime,
            EndTime = endTime
        });
    }

    public async Task BulkReCordDrillRateFactor(List<ReCordDrillRateFactorDto> reqs)
    {
        if (reqs == null || reqs.Count == 0)
        {
            return;
        }

        List<AddOrUpdateDrillRateFactorReq> addList = new List<AddOrUpdateDrillRateFactorReq>();
        foreach (var req in reqs)
        {
            if (string.IsNullOrEmpty(req.DeviceId))
            {
                continue;
            }
            if (!req.Reason.HasValue)
            {
                continue;
            }

            addList.Add(new AddOrUpdateDrillRateFactorReq
            {
                DeviceId = req.DeviceId,
                Reason = req.Reason,
                StartTime = req.StartTime,
                EndTime = req.EndTime
            });
        }

        await _drillRateFactorService.BulkAddOrUpdate(addList);
    }
}
