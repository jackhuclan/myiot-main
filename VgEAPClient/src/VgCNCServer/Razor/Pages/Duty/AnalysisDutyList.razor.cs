using Aspose.Cells;
using BlazorDownloadFile;
using Microsoft.EntityFrameworkCore;
using VgCNCServer.DAO;
using VgCNCServer.DAO.Models;

namespace VgCNCServer.Razor.Pages.Duty;

public partial class AnalysisDutyList
{
    [Inject] private IBlazorDownloadFileService BlazorDownloadFileService { get; set; }
    [Inject] private IDbContextFactory<VegaContext> contextFactory { get; set; }

    private readonly List<DataTableHeader<usrAnalysisDuty>> _headers = new List<DataTableHeader<usrAnalysisDuty>>
    {
      new (){Text= "机台编号",Align= DataTableHeaderAlign.Start,Sortable= false,Value= nameof(usrAnalysisDuty.sEquipmentID)},
      new (){Text= "方式", Value= nameof(usrAnalysisDuty.sZ)},
      new (){ Text= "日期", Value= nameof(usrAnalysisDuty.dtDate)},
      new (){ Text= "采集时间", Value= nameof(usrAnalysisDuty.sTime)},
      new (){ Text= "换板次数", Value= nameof(usrAnalysisDuty.sChangePanels)},
      new (){ Text= "标准换板时间", Value= nameof(usrAnalysisDuty.sStandardChangePanelsTime)},
      new (){ Text= "换料次数", Value= nameof(usrAnalysisDuty.sChangeDrills)},
      new (){ Text= "标准换料时间", Value= nameof(usrAnalysisDuty.sStandardChangeDrillsTime)},
      new (){ Text= "异常处理次数", Value= nameof(usrAnalysisDuty.sExceptionHandleCounts)},
      new (){ Text= "异常处理时间", Value= nameof(usrAnalysisDuty.sExceptionHandleTime)},
      new (){ Text= "保养时间", Value= nameof(usrAnalysisDuty.sMaintainTime)},
      new (){ Text= "理论稼动率", Value= nameof(usrAnalysisDuty.sTheoryDuty)},
      new (){ Text= "实际稼动率", Value= nameof(usrAnalysisDuty.sActualDuty)},
      new (){ Text= "差异", Value= nameof(usrAnalysisDuty.sDifference)},
      new (){ Text= "所属班次", Value= nameof(usrAnalysisDuty.sShift)}
    };

    private List<usrAnalysisDuty> itemSource = new();

    public string? EquipmentID { get; set; }

    public string? Shift { get; set; }

    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    #region 新增选择设备

    private bool showModal = false;
    private List<string> allEquipmentIDs = new List<string>();
    private readonly Dictionary<string, bool> selectedEquipmentIDs = new Dictionary<string, bool>();
    private string queryResult = "";

    private void ShowDeviceList()
    {
        showModal = true;
    }

    private void CloseModal()
    {
        showModal = false;
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadAllEquipmentIDs();
    }

    private async Task LoadAllEquipmentIDs()
    {
        using (var ctx = contextFactory.CreateDbContext())
        {
            allEquipmentIDs = await ctx.sysDrillInformations.Select(s => s.sEquipmentID).Distinct().ToListAsync();
            foreach (var equipment in allEquipmentIDs)
            {
                selectedEquipmentIDs[equipment] = false;
            }
        }
    }

    private void ConfirmSelection()
    {
        PerformQuery();
        CloseModal();
    }

    private async Task PerformQuery()
    {
        var selectedEquipments = selectedEquipmentIDs.Where(d => d.Value).Select(d => d.Key).ToList();

        using (var ctx = contextFactory.CreateDbContext())
        {
            IQueryable<usrAnalysisDuty> userQuery = ctx.usrAnalysisDutys.Where(s => s.dtDate >= StartDate && s.dtDate <= EndDate);

            if (selectedEquipments.Any())
            {
                userQuery = userQuery.Where(s => selectedEquipments.Contains(s.sEquipmentID));
            }

            itemSource = await userQuery.OrderByDescending(s => s.dtDate).ThenByDescending(s => s.sTime).ToListAsync();
        }

        queryResult = $"查询结果:已选择设备 - {string.Join(", ", selectedEquipments)}";
        StateHasChanged();
    }

    #endregion 新增选择设备

    private async Task QueryDetail()
    {
        using (var ctx = contextFactory.CreateDbContext())
        {
            IQueryable<usrAnalysisDuty> userQuery = ctx.usrAnalysisDutys.Where(s => s.dtDate >= StartDate && s.dtDate <= EndDate);
            if (EquipmentID != null)
            {
                userQuery = userQuery.Where(s => s.sEquipmentID.Contains(EquipmentID));
            }
            if (Shift != null)
            {
                userQuery = userQuery.Where(s => s.sShift.Contains(Shift));
            }
            itemSource = await userQuery.OrderByDescending(s => s.dtDate).ThenByDescending(s => s.sTime).ToListAsync();
        }
    }

    private async Task ExportExcel()
    {
        string path = Path.Combine(AppContext.BaseDirectory, $"AnalysisDuty-{DateTime.Now.ToString("yyyyMMdd")}.xlsx");
        Workbook book = new Workbook();
        Worksheet sheet = book.Worksheets[0];
        sheet.Cells[0, 0].PutValue("设备编号");
        sheet.Cells[0, 1].PutValue("方式");
        sheet.Cells[0, 2].PutValue("日期时间");
        sheet.Cells[0, 3].PutValue("换板次数");
        sheet.Cells[0, 4].PutValue("标准换板时间");
        sheet.Cells[0, 5].PutValue("换料次数");
        sheet.Cells[0, 6].PutValue("标准换料时间");
        sheet.Cells[0, 7].PutValue("异常处理次数");
        sheet.Cells[0, 8].PutValue("异常处理时间");
        sheet.Cells[0, 9].PutValue("保养时间");
        sheet.Cells[0, 10].PutValue("理论稼动率");
        sheet.Cells[0, 11].PutValue("实际稼动率");
        sheet.Cells[0, 12].PutValue("差异");
        sheet.Cells[0, 13].PutValue("所属班次");
        int i = 0;
        foreach (var v in itemSource)
        {
            i++;
            sheet.Cells[i, 0].PutValue(v.sEquipmentID);
            sheet.Cells[i, 1].PutValue(v.sZ);
            sheet.Cells[i, 2].PutValue(v.dtDate + " " + v.sTime);
            sheet.Cells[i, 3].PutValue(v.sChangePanels);
            sheet.Cells[i, 4].PutValue(v.sStandardChangePanelsTime);
            sheet.Cells[i, 5].PutValue(v.sChangeDrills);
            sheet.Cells[i, 6].PutValue(v.sStandardChangeDrillsTime);
            sheet.Cells[i, 7].PutValue(v.sExceptionHandleCounts);
            sheet.Cells[i, 8].PutValue(v.sExceptionHandleTime);
            sheet.Cells[i, 9].PutValue(v.sMaintainTime);
            sheet.Cells[i, 10].PutValue(v.sTheoryDuty);
            sheet.Cells[i, 11].PutValue(v.sActualDuty);
            sheet.Cells[i, 12].PutValue(v.sDifference);
            sheet.Cells[i, 13].PutValue(v.sShift);
        }
        book.Save(path, SaveFormat.Xlsx);
        byte[] contentBytes = File.ReadAllBytes(path);

        await BlazorDownloadFileService.DownloadFile(Path.GetFileName(path), contentBytes, "application/octet-stream");
    }
}
