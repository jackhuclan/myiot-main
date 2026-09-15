using Aspose.Cells;
using BlazorDownloadFile;
using Microsoft.EntityFrameworkCore;
using VgCNCServer.DAO;
using VgCNCServer.DAO.Models;

namespace VgCNCServer.Razor.Pages.Duty;

public partial class DutyList
{
    [Inject] private IBlazorDownloadFileService BlazorDownloadFileService { get; set; }
    [Inject] private IDbContextFactory<VegaContext> contextFactory { get; set; }

    private readonly List<DataTableHeader<usrDuty>> dataTableHeaders = new List<DataTableHeader<usrDuty>>
    {
      new (){Text= "机台编号",Align= DataTableHeaderAlign.Start,Sortable= false,Value= nameof(usrDuty.sEquipmentID)},
      new (){ Text= "日期", Value= nameof(usrDuty.dtDate)},
      new (){ Text= "白班稼动率", Value= nameof(usrDuty.sDutyShiftA)},
      new (){ Text= "夜班稼动率", Value= nameof(usrDuty.sDutyShiftB)}
    };

    private List<usrDuty> itemSource = new();

    public string? EquipmentID { get; set; }

    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddDays(-14));

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
        using (var context = contextFactory.CreateDbContext())
        {
            allEquipmentIDs = await context.sysDrillInformations.Select(s => s.sEquipmentID).Distinct().ToListAsync();
            foreach (string equipment in allEquipmentIDs)
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
        using (var context = contextFactory.CreateDbContext())
        {
            IQueryable<usrDuty> userQuery = context.usrDutys.Where(s => s.dtDate >= StartDate && s.dtDate <= EndDate);

            if (selectedEquipments.Any())
            {
                userQuery = userQuery.Where(s => selectedEquipments.Contains(s.sEquipmentID));
            }

            itemSource = await userQuery.OrderByDescending(s => s.dtDate).ToListAsync();
        }

        queryResult = $"查询结果:已选择设备 - {string.Join(", ", selectedEquipments)}";
        StateHasChanged();
    }

    #endregion 新增选择设备

    private async Task QueryDetail()
    {
        using (var context = contextFactory.CreateDbContext())
        {
            IQueryable<usrDuty> userQuery = context.usrDutys.Where(s => s.dtDate >= StartDate && s.dtDate <= EndDate);
            if (EquipmentID != null)
            {
                userQuery = userQuery.Where(s => s.sEquipmentID.Contains(EquipmentID));
            }
            itemSource = await userQuery.OrderByDescending(s => s.dtDate).ToListAsync();
        }
    }

    private async Task ExportExcel()
    {
        string path = Path.Combine(AppContext.BaseDirectory, $"Duty-{DateTime.Now.ToString("yyyyMMdd")}.xlsx");
        Workbook book = new Workbook();
        Worksheet sheet = book.Worksheets[0];
        sheet.Cells[0, 0].PutValue("设备编号");
        sheet.Cells[0, 1].PutValue("日期");
        sheet.Cells[0, 2].PutValue("白班稼动率");
        sheet.Cells[0, 3].PutValue("夜班稼动率");
        int i = 0;
        foreach (usrDuty v in itemSource)
        {
            i++;
            sheet.Cells[i, 0].PutValue(v.sEquipmentID);
            sheet.Cells[i, 1].PutValue(v.dtDate);
            sheet.Cells[i, 2].PutValue(v.sDutyShiftA);
            sheet.Cells[i, 3].PutValue(v.sDutyShiftB);
        }
        book.Save(path, SaveFormat.Xlsx);
        byte[] contentBytes = File.ReadAllBytes(path);

        await BlazorDownloadFileService.DownloadFile(Path.GetFileName(path), contentBytes, "application/octet-stream");
    }
}
