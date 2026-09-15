using System.Collections.Generic;
using Masa.Blazor.Presets;
using Microsoft.EntityFrameworkCore;
using Quartz.Util;
using VgCNCServer.DAO;
using VgCNCServer.DAO.Models;
using VgEAPClient.Common;

namespace VgCNCServer.Razor.Pages.Dashboard;

public partial class DrillRUT : ProComponentBase
{
    public int CurrentCount => drillList.Count();

    public int PageIndex { get; set; } = 1;

    public int PageSize { get; set; } = 8 * 3;

    public int PageCount => (int)Math.Ceiling(CurrentCount / (double)PageSize);

    private List<sysDrillInformation> drillList = new();

    public List<sysDrillInformation> GetPageDatas()
    {
        return drillList.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
    }

    private System.Timers.Timer heartBeatTimer;

    [Inject]
    public NavigationManager Nav { get; set; } = default!;

    [CascadingParameter]
    public IPageTabsProvider? PageTabsProvider { get; set; }

    [Inject] private IDbContextFactory<VegaContext> contextFactory { get; set; }

    public int WorkingCount { get; set; } = 0;
    public int AlarmCount { get; set; } = 0;
    public int StopCount { get; set; } = 0;
    public int WaitingCount { get; set; } = 0;
    public int IdleCount { get; set; } = 0;
    public int OfflineCount { get; set; } = 0;

    protected override void OnInitialized()
    {

        allWorkModes =
        [
            "加工","报警","停止","等待","闲置","离线"
        ];

        selectedWorkModes["加工"] = true;
        selectedWorkModes["报警"] = true;
        selectedWorkModes["停止"] = true;
        selectedWorkModes["等待"] = true;
        selectedWorkModes["闲置"] = true;
        selectedWorkModes["离线"] = true;

        Refresh();

        heartBeatTimer = new System.Timers.Timer();
        heartBeatTimer.AutoReset = true;
        heartBeatTimer.Interval = 1000 * 60 * 5;
        heartBeatTimer.Elapsed += HeartBeatTimer_Elapsed;
        heartBeatTimer.Start();

        PageTabsProvider?.UpdateTabTitle(Nav.GetAbsolutePath(), () => T("RutKanban"));
    }

    private void HeartBeatTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        Refresh();
    }

    private bool showModal = false;
    private List<string> allWorkModes = new List<string>();
    private readonly Dictionary<string, bool> selectedWorkModes = new Dictionary<string, bool>();
    private void ShowDeviceList()
    {
        showModal = true;
    }
    private void CloseModal()
    {
        showModal = false;
    }

    private void ConfirmSelection()
    {
        Refresh();
        CloseModal();
    }

    private void Refresh()
    {
        IdleCount = WaitingCount = WorkingCount = StopCount = AlarmCount = OfflineCount = 0;
        drillList.Clear();
        List<sysDrillInformation> drillListMy = new List<sysDrillInformation>();
        using (var context = contextFactory.CreateDbContext())
        {
            drillListMy = context.sysDrillInformations.OrderBy(s => s.sEquipmentID).ToList();
            DateTime nowtime = DateTime.Now;
            foreach (var drill in drillListMy)
            {
                if (drill.dtDateTime == null || nowtime.Subtract(drill.dtDateTime.Value).TotalMinutes >= 30)
                {
                    drill.sWorkMode = "OFFLINE";
                }
            }
        }

        foreach (var drill in drillListMy)
        {
            if (drill.sEqpType.ToStringEx().Equals("RUT", StringComparison.OrdinalIgnoreCase))
            {
                switch (drill.sWorkMode)
                {
                    case "IDLE":
                        IdleCount++;
                        if (selectedWorkModes["闲置"])
                        {
                            drillList.Add(drill);
                        }
                        break;

                    case "WAIT":
                        WaitingCount++;
                        if (selectedWorkModes["等待"])
                        {
                            drillList.Add(drill);
                        }
                        break;

                    case "WORK":
                        WorkingCount++;
                        if (selectedWorkModes["加工"])
                        {
                            drillList.Add(drill);
                        }
                        break;

                    case "STOP":
                        StopCount++;
                        if (selectedWorkModes["停止"])
                        {
                            drillList.Add(drill);
                        }
                        break;

                    case "ALAM":
                        AlarmCount++;
                        if (selectedWorkModes["报警"])
                        {
                            drillList.Add(drill);
                        }
                        break;

                    case "SERV":
                        StopCount++;
                        if (selectedWorkModes["停止"])
                        {
                            drillList.Add(drill);
                        }
                        break;

                    default:
                        OfflineCount++;
                        if (selectedWorkModes["离线"])
                        {
                            drillList.Add(drill);
                        }
                        break;
                }
            }


        }
        // 通知Blazor更新UI
        InvokeAsync(() => { StateHasChanged(); });
        //InvokeAsync(StateHasChanged);
    }

    private void NavigateToDetails(string id)
    {
        Nav.NavigateTo($"dashboard/detail/{id}");
    }

    public void Dispose()
    {
        heartBeatTimer?.Dispose();
    }
}
