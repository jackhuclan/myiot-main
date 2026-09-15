using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Central.Core.Manager;

namespace VgAutoDrill.Central.WebApi.Controllers.stat;

[ApiController]
[Route("central/stat/unpin")]
public class UnpinController : ControllerBase
{
    private readonly IUnpinManager _unpinManager;

    public UnpinController(IUnpinManager unpinManager)
    {
        _unpinManager = unpinManager;
    }

    [HttpGet]
    public object? Index(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return new
            {
                _unpinManager.UnPins,
            };
        }

        return _unpinManager.UnPins.FirstOrDefault(x => x.DeviceId == id);
    }
}
