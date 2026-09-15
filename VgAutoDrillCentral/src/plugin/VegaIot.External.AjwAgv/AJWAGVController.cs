using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VegaIot.External.AgvEntity.AJW;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.OpenAPI;

namespace VegaIot.External.AjwAgv;

/// <summary>
/// 艾吉威 接口回调
/// </summary>
[ApiController]
[Route("v1/central/ajw")]
public class AjwAGVController : Controller
{
    private readonly IDeviceManager _deviceHolder;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly AjwAgvAgentOptions _ajwAgvAgentOptions;
    private readonly ILogger<AjwAGVController> _logger;

    public AjwAGVController(IDeviceManager deviceHolder,
        IHttpRequestInvoker httpRequestInvoker,
        IOptions<AjwAgvAgentOptions> options,
        ILoggerFactory loggerFactory)
    {
        _deviceHolder = deviceHolder;
        _httpRequestInvoker = httpRequestInvoker;
        _ajwAgvAgentOptions = options.Value;
        _logger = loggerFactory.CreateLogger<AjwAGVController>();
    }

    /// <summary>
    /// 车辆到位
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    [HttpPost("arrived", Name = "arrived_TransferProxy")]
    public async Task<AJWArrivedResponseEntity?> Arrived(AJWArrivedRequestEntity status)
    {
        var response = new AJWArrivedResponseEntity();

        if (status.AGV_ID == null)
        {
            _logger.LogWarning($"Arrived is callded with null input.");
            return response;
        }

        _logger.LogDebug($"Arrived is called with input AJWArrivedRequestEntity,{JsonSerializer.Serialize(status)}");

        response.AGV_ID = status.AGV_ID;
        var agvId = status.AGV_ID.ToStr();
        agvId = agvId.Replace("#", "");

        if (!_deviceHolder.TryGetLocalDevice(agvId, out DeviceProxy device) || device == null)
        //var device = _deviceHolder.GetOnlineDevice(agvId);
        //if (device == null)
        {
            _logger.LogError($"deviceHolder Can't get device[{agvId}]");
            return response;
        }

        var url = device.Descriptor.HostAddress + "/" + _ajwAgvAgentOptions.BaseUrlPrefix + "/arrived";
        _logger.LogInformation(url);
        return await _httpRequestInvoker.PostAsJsonAsync<AJWArrivedRequestEntity, AJWArrivedResponseEntity>(url, status);
    }

    [HttpPost("battery", Name = "battery_TransferProxy")]
    public async Task<AJWBatteryResponseEntity?> Battery(AJWBatteryRequestEntity request)
    {
        var agvId = request.AGV_ID.ToStr();
        agvId = agvId.Replace("#", "");

        if (!_deviceHolder.TryGetLocalDevice(agvId, out DeviceProxy device) || device == null)
        //var device = _deviceHolder.GetOnlineDevice(agvId);
        //if (device == null)
        {
            _logger.LogError($"deviceHolder Can't get device[{agvId}]");
            return new AJWBatteryResponseEntity
            {
                AGV_ID = agvId,
            };
        }

        var url = device.Descriptor.HostAddress + "/" + _ajwAgvAgentOptions.BaseUrlPrefix + "/battery";
        _logger.LogDebug(url);
        return await _httpRequestInvoker.PostAsJsonAsync<AJWBatteryRequestEntity, AJWBatteryResponseEntity>(url, request);
    }
}
