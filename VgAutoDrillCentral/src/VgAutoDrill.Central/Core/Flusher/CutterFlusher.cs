using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Central.Core.Mes.Interface;

namespace VgAutoDrill.Central.Core.Flusher;

public class CutterFlusher : ICutterFlusher
{
    private readonly ISysConfigManager _sysConfigManager;
    private readonly IDeviceAdapter _deviceAdapter;
    private ConcurrentDictionary<string, DateTime> _lastRefreshed = new();
    private readonly ILogger<CutterFlusher> _logger;

    public CutterFlusher(ISysConfigManager sysConfigManager,
        IDeviceAdapter deviceAdapter,
        ILoggerFactory loggerFactory)
    {
        _sysConfigManager = sysConfigManager;
        _deviceAdapter = deviceAdapter;
        _logger = loggerFactory.CreateLogger<CutterFlusher>();
    }

    public async Task OnCutterRefreshed(DeviceProxy deviceProxy)
    {
        var saved = _lastRefreshed.TryGetValue(deviceProxy.DeviceId, out DateTime lastTime);
        var enableCutterFlusher = await _sysConfigManager.GetBoolValue("EnableCutterFlusher", Admin.Model.Enum.SysConfigCategoryEnum.None, false);
        if (!enableCutterFlusher)
            return;

        var cutterFlusherInterval = await _sysConfigManager.GetIntValue("CutterFlusherInterval", Admin.Model.Enum.SysConfigCategoryEnum.None, false);
        if (cutterFlusherInterval == 0)
        {
            cutterFlusherInterval = 10;
        }

        if (!saved || (saved && DateTime.Now.Subtract(lastTime) > TimeSpan.FromMinutes(cutterFlusherInterval)))
        {
            _logger.LogInformation($"OnCutterRefreshed saved {deviceProxy.DeviceId}'s Cutter!");
            _lastRefreshed.AddOrUpdate(deviceProxy.DeviceId, d => DateTime.Now, (d, time) =>
            {
                _lastRefreshed[d] = DateTime.Now;
                return _lastRefreshed[d];
            });

            _ = _deviceAdapter.PersistCutters(deviceProxy.ProductId, deviceProxy.DeviceId, deviceProxy.PayloadCutterTrays);
        }
    }
}
