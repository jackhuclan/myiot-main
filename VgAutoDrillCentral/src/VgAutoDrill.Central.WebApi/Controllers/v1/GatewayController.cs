
using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceGateway;
using VgAutoDrill.Fundation.Iot.Configuration;

namespace VgAutoDrill.Central.WebApi.Controllers.v1;

[ApiController]
[Route("v1/central/gateway")]
public class GatewayController : Controller
{
    private List<DeviceGatewayOptions> _agentOptions;
    private readonly IDeviceGatewayService _deviceGatewayService;

    public GatewayController(IDeviceGatewayService deviceGatewayService)
    {
        _deviceGatewayService = deviceGatewayService;
        _agentOptions = new List<DeviceGatewayOptions>();

    }

    [HttpGet("online", Name = "Online")]
    public List<DeviceGatewayOptions> Online()
    {
        return new List<DeviceGatewayOptions>();
    }

    [HttpPost("login", Name = "Login")]
    public async Task<OkResult> Login(DeviceGatewayOptions app)
    {
        var result = await _deviceGatewayService.DeviceGatewayAddOrUpdate(new AddOrUpdateDeviceGatewayReq
        {
            AppName = app.AppName,
            Name = app.AppName,
            ServiceName = app.ServiceName,
            CVersion = app.FundationVersion,
            AVersion = app.AppVersion,
            VisitWebsite = app.HostAddress,
            InstalledLocation = app.InstalledLocation,
            IsOnline = app.Online == false ? 0 : 1,
        });

        return Ok();
    }

    [HttpPost("logout", Name = "LogOut")]
    public async Task<OkResult> LogOut(DeviceGatewayOptions app)
    {
        var result = await _deviceGatewayService.DeviceGatewayAddOrUpdate(new AddOrUpdateDeviceGatewayReq
        {
            AppName = app.AppName,
            Name = app.AppName,
            ServiceName = app.ServiceName,
            CVersion = app.FundationVersion,
            AVersion = app.AppVersion,
            VisitWebsite = app.HostAddress,
            InstalledLocation = app.InstalledLocation,
            IsOnline = app.Online == false ? 0 : 1,
        });

        return Ok();
    }
}
