using Masa.Blazor.Presets;
using Microsoft.EntityFrameworkCore;
using VgCNCServer.DAO;
using VgCNCServer.DAO.Models;

namespace VgCNCServer.Razor.Pages.Dashboard;

public partial class Detail : ProComponentBase
{
    private sysDrillInformation? drill;
    private List<sysDrillInformation> drillList = new();

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    [Inject] private IDbContextFactory<VegaContext> contextFactory { get; set; }

    [CascadingParameter]
    public IPageTabsProvider? PageTabsProvider { get; set; }

    [Parameter]
    public string? Id { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        using (var context = contextFactory.CreateDbContext())
        {
            drillList = context.sysDrillInformations.ToList();
            DateTime nowtime = DateTime.Now;
            foreach (var drill in drillList)
            {
                if (drill.dtDateTime == null || nowtime.Subtract(drill.dtDateTime.Value).TotalMinutes >= 30)
                {
                    drill.sWorkMode = "OFFLINE";
                }
            }
            if (Id != null)
            {
                drill = context.sysDrillInformations.Where(s => s.sEquipmentID.Contains(Id)).FirstOrDefault();
            }
            else
            {
                drill = drillList.FirstOrDefault();
            }
        }

        UpdateTabTitle();
    }

    private void UpdateTabTitle()
    {
        PageTabsProvider?.UpdateTabTitle(NavigationManager.GetAbsolutePath(), () => T("Details of {0}", drill.sEquipmentID));
    }
}
