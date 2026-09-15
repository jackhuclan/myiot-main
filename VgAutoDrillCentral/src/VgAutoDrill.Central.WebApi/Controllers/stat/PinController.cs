using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Central.Core.Manager;

namespace VgAutoDrill.Central.WebApi.Controllers.stat;

[ApiController]
[Route("central/stat/pin")]
public class PinController : ControllerBase
{
    private readonly IPinManager _pinManager;

    public PinController(IPinManager pinManager)
    {
        _pinManager = pinManager;
    }

    [HttpGet]
    public object? Index(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return new
            {
                _pinManager.Pins,
            };
        }

        return _pinManager.Pins.FirstOrDefault(x => x.DeviceId == id);
    }
}
