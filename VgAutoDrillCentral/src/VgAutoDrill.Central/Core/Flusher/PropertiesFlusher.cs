using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Central.Core.Mes.Interface;

namespace VgAutoDrill.Central.Core.Flusher;

public class PropertiesFlusher : IPropertiesFlusher
{
    private readonly ISysConfigManager _sysConfigManager;
    private readonly IDeviceAdapter _deviceAdapter;
    private ConcurrentDictionary<string, DateTime> _lastRefreshed = new();
    private readonly ILogger<PropertiesFlusher> _logger;

    public PropertiesFlusher(ISysConfigManager sysConfigManager,
        IDeviceAdapter deviceAdapter,
        ILoggerFactory loggerFactory)
    {
        _sysConfigManager = sysConfigManager;
        _deviceAdapter = deviceAdapter;
        _logger = loggerFactory.CreateLogger<PropertiesFlusher>();
    }

    public async Task OnPropertiesRefreshed(DeviceProxy deviceProxy)
    {
        var propertiesFlusherInterval = await _sysConfigManager.GetIntValue("PropertiesFlusherInterval");
        if (propertiesFlusherInterval == 0)
        {
            propertiesFlusherInterval = 10;
        }

        var saved = _lastRefreshed.TryGetValue(deviceProxy.DeviceId, out DateTime lastTime);
        if (saved && DateTime.Now.Subtract(lastTime) < TimeSpan.FromMinutes(propertiesFlusherInterval))
        {
            return;
        }
        _lastRefreshed.AddOrUpdate(deviceProxy.DeviceId, d => DateTime.Now, (d, time) => DateTime.Now);

        var enablePropertiesFlusher = await _sysConfigManager.GetBoolValue("EnablePropertiesFlusher");
        if (!enablePropertiesFlusher)
            return;

        var propertiesFlusherAllowedDevices = await _sysConfigManager.GetStringValue("PropertiesFlusherAllowedDevices");
        if (string.IsNullOrEmpty(propertiesFlusherAllowedDevices))
        {
            return;
        }
        else if (propertiesFlusherAllowedDevices.ToLower() != "none")
        {
            var allowedDevices = propertiesFlusherAllowedDevices.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (!allowedDevices.Any(x => x.ToLower() == deviceProxy.DeviceId.ToLower()))
            {
                return;
            }
        }

        _logger.LogInformation($"OnPropertiesRefreshed saved {deviceProxy.DeviceId}'s properties!");
        await _deviceAdapter.PersistProperties(deviceProxy.ProductId, deviceProxy.DeviceId, deviceProxy.Properties);
    }
}
