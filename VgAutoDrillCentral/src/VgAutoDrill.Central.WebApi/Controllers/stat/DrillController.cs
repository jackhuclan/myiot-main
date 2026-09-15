using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Model;

namespace VgAutoDrill.Central.WebApi.Controllers.stat;

[ApiController]
[Route("central/stat/drill")]
public class DrillController : ControllerBase
{
    private readonly IDrillManager _drillManager;
    private readonly IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask> _panelSiloForkManager;
    private IPanelAgvManager<BackPanelAgv> _backPanelAgvManager;
    public DrillController(IDrillManager drillManager,
        IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask> panelSiloForkManager,
        IPanelAgvManager<BackPanelAgv> backPanelAgvManager)
    {
        _drillManager = drillManager;
        _panelSiloForkManager = panelSiloForkManager;
        _backPanelAgvManager = backPanelAgvManager;
    }

    [HttpGet]
    public object? Index(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return new
            {
                _drillManager.Drills,
                _drillManager.Requirements,
                _drillManager.RequiredDrilledItemCodes,
                _drillManager.RequiredUndrilledItemCodes,
                _drillManager.RequiredDrilledItemSummaries,
                _drillManager.RequiredUndrilledItemSummaries,
                _drillManager.PendingWorkOrders,
            };
        }

        return _drillManager.Drills.FirstOrDefault(x => x.DeviceId == id);
    }

    [HttpPost("summary", Name = "Summary")]
    public object? Summary(string partCode, List<string> routeCodes)
    {
        if (string.IsNullOrEmpty(partCode)
            || !routeCodes.Any())
        {
            return null;
        }

        routeCodes = routeCodes.Select(x => x.ToLower()).ToList();

        return new
        {
            ForkSummary = _panelSiloForkManager.PartitionUndrilledItemSummaries(partCode).ToList(),
            DrillSummary = _drillManager.RouteRequiredUndrilledItemSummaries(routeCodes),
            AgvSummary = _backPanelAgvManager.RouteUndrilledItemSummaries(routeCodes)
        };
    }
}
