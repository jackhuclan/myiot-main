using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.WebApi.Controllers.v1.Models;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.WebApi.Controllers.v1;

[ApiController]
[Route("v1/central/app")]
public class AppController : Controller
{
    private readonly IDeviceManager _deviceHolder;
    private readonly ILogger<AppController> _logger;
    private readonly ILocationManager _locationManager;

    public AppController(IDeviceManager deviceHolder,
        ILocationManager locationManager,
        ILoggerFactory loggerFactory)
    {
        _deviceHolder = deviceHolder;
        _logger = loggerFactory.CreateLogger<AppController>();
        _locationManager = locationManager;
    }

    [HttpGet("device", Name = "DeviceList")]
    public List<OnlineDevice> DeviceList()
    {
        return new List<OnlineDevice>();
    }

    [HttpGet("function", Name = "FunctionList")]
    public List<DeviceFunction> FunctionList(string deviceId)
    {
        return new List<DeviceFunction>();
    }


    [HttpGet("silo", Name = "SiloList")]
    public List<DeviceSilo> SiloList(string deviceId)
    {
        return new List<DeviceSilo>();
    }

    [HttpGet("panel", Name = "PanelList")]
    public List<DevicePanel> PanelList(string deviceId, string locationCode)
    {
        return new List<DevicePanel>();
    }

    [HttpPost("command", Name = "RemoteCommand")]
    public async Task<DeviceCommandResponse> RemoteCommand(DeviceCommandRequest commandRequest)
    {
        DeviceCommandResponse result = new DeviceCommandResponse();
        var device = _deviceHolder.GetOnlineDevice(commandRequest.DeviceId);
        if (device == null)
        {
            result.Code = ErrorCodes.Sys.FAIL;
            result.Message = $"未找到设备{commandRequest.DeviceId}！";
            return result;
        }

        var request = new DeviceServiceInvokeRequest
        {
            ProductId = device.ProductId,
            DeviceId = device.DeviceId,
            ClientId = device.ClientId,
            TargetDeviceId = device.DeviceId,
            TargetProductId = device.ProductId,
            TargetClientId = device.ClientId,
            HostAddress = device.Descriptor.HostAddress,
            TargetHostAddress = device.Descriptor.HostAddress,
            ServiceId = Topics.Services.REMOTE_COMMAND_SERVICE_ID,
            ServiceName = commandRequest.Command,
            Params = commandRequest.Params,
        };

        if (commandRequest.Command.Equals("RemoveSiloCommand") && commandRequest.Params.ContainsKey("DeviceCode"))
        {
            var locationCode = commandRequest.Params["DeviceCode"].ToStr();
            if (!string.IsNullOrEmpty(locationCode))
            {
                if (_locationManager.TryGetLocation(locationCode, out var location) && location != null)
                {
                    location.SetNoPayload();
                }
            }
        }

        var response = await device.InvokeService(request);
        if (response == null)
        {
            result.Code = ErrorCodes.Sys.FAIL;
            result.Message = $"设备{commandRequest.DeviceId}发送命令失败！";
            return result;
        }

        result.Code = response.Code;
        result.Message = response.Message;
        result.Params = response.Params;
        return result;
    }
}
