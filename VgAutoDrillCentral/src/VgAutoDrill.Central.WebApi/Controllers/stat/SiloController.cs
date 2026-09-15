using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Central.Core.Manager;

namespace VgAutoDrill.Central.WebApi.Controllers.stat;

[ApiController]
[Route("central/stat/silo")]
public class SiloController : ControllerBase
{
    private readonly ILocationManager _locationManager;

    public SiloController(ILocationManager locationManager)
    {
        _locationManager = locationManager;
    }

    [HttpGet]
    public object? Index(string? code)
    {
        if (string.IsNullOrEmpty(code))
        {
            return _locationManager.Locations.Where(x => x.HasSilo);
        }

        return _locationManager.Locations.FirstOrDefault(x => x.SiloCode.ToLower() == code.ToLower());
    }
}
