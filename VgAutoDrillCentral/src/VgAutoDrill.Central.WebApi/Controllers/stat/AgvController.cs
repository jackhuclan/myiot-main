using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;

namespace VgAutoDrill.Central.WebApi.Controllers.stat;

[ApiController]
[Route("central/stat/agv")]
public class AgvController : ControllerBase
{
    private readonly IPanelAgvManager<TransferSiloAgv> _transferSiloAgvManager;
    private readonly IPanelAgvManager<BackPanelAgv> _backPanelAgvManager;

    public AgvController(IPanelAgvManager<TransferSiloAgv> transferSiloAgvManager,
        IPanelAgvManager<BackPanelAgv> backPanelAgvManager)
    {
        _transferSiloAgvManager = transferSiloAgvManager;
        _backPanelAgvManager = backPanelAgvManager;
    }

    [HttpGet]
    public object? Index(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return new
            {
                _backPanelAgvManager.BackPanelAgvs,
                _transferSiloAgvManager.TransferSiloAgvs
            };
        }

        return _backPanelAgvManager.BackPanelAgvs.FirstOrDefault(x => x.DeviceId == id) == null ?
            _transferSiloAgvManager.TransferSiloAgvs.FirstOrDefault(x => x.DeviceId == id) : _backPanelAgvManager.BackPanelAgvs.FirstOrDefault(x => x.DeviceId == id);
    }
}
