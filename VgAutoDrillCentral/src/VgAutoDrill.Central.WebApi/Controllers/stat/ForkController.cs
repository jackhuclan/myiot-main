using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Model;

namespace VgAutoDrill.Central.WebApi.Controllers.stat;

[ApiController]
[Route("central/stat/fork")]
public class ForkController : ControllerBase
{
    private readonly IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask> _panelSiloForkManager;

    public ForkController(IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask> panelSiloForkManager)
    {
        _panelSiloForkManager = panelSiloForkManager;
    }

    [HttpGet]
    public object? Index(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return new
            {
                _panelSiloForkManager.DutyRacks,
            };
        }

        return _panelSiloForkManager.DutyRacks.FirstOrDefault(x => x.DeviceId == id);
    }


    [HttpGet("Get")]
    public object? GetEnabledLocations(string? partCode)
    {
        if (string.IsNullOrEmpty(partCode))
        {
            return new
            {
                _panelSiloForkManager.EnabledLocations,
            };
        }

        return _panelSiloForkManager.EnabledLocations.FirstOrDefault(p => p.PartitionCode == partCode);
    }
}
