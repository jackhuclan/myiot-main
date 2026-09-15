using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Reporter;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.WebApi.Controllers.v2;

/// <summary>
/// DeviceController version 2, merged service invoke
/// </summary>
[ApiController]
[Route("v2/central/device")]
public class DeviceControllerV2 : ControllerBase
{
    private readonly ILogger<DeviceControllerV2> logger;
    private readonly IDeviceManager deviceHolder;
    private readonly IDeviceServiceReporter _deviceServiceReporter;

    public DeviceControllerV2(IDeviceManager deviceHolder,
        IDeviceServiceReporter deviceServiceReporter,
        ILoggerFactory loggerFactory)
    {
        this.deviceHolder = deviceHolder;
        _deviceServiceReporter = deviceServiceReporter;
        logger = loggerFactory.CreateLogger<DeviceControllerV2>();
    }

    /// <summary>
    /// 执行业务操作
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("service/invoke", Name = "DeviceServiceInvokeV2")]
    public async Task<DeviceServiceInvokeResponse> DeviceServiceInvoke(DeviceServiceInvokeRequest request)
    {
        return await _deviceServiceReporter.Report(request);
    }
}
