using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Model;

namespace VgAutoDrill.Central.WebApi.Controllers.stat;

[ApiController]
[Route("central/stat/shelf")]
public class ShelfController : ControllerBase
{
    private readonly IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask> _panelSiloShelfManager;

    public ShelfController(IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask> panelSiloShelfManager)
    {
        _panelSiloShelfManager = panelSiloShelfManager;
    }

    [HttpGet]
    public object? Index(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return new
            {
                _panelSiloShelfManager.DutyRacks,
            };
        }

        return _panelSiloShelfManager.DutyRacks.FirstOrDefault(x => x.DeviceId == id);
    }
}
