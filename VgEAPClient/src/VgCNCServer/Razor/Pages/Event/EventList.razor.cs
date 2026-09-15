using Aspose.Cells;
using BlazorDownloadFile;
using Microsoft.EntityFrameworkCore;
using VgCNCServer.DAO;
using VgCNCServer.DAO.Models;

namespace VgCNCServer.Razor.Pages.Event;

public partial class EventList
{
    [Inject] private IBlazorDownloadFileService BlazorDownloadFileService { get; set; }
    [Inject] private IDbContextFactory<VegaContext> contextFactory { get; set; }

    private readonly List<DataTableHeader<usrEvent>> dataTableHeaders = new List<DataTableHeader<usrEvent>>
    {
      new (){Text= "机台编号",Align= DataTableHeaderAlign.Start,Sortable= false,Value= nameof(usrEvent.sEquipmentID)},
      new (){ Text= "日期", Value= nameof(usrEvent.dtDate)},
      new (){ Text= "事件时间", Value= nameof(usrEvent.sTime)},
      new (){ Text= "日志描述", Value= nameof(usrEvent.sEventDesc)},
      new (){ Text= "钻带文件", Value= nameof(usrEvent.sActProgram)},
      new (){ Text= "事件代码", Value= nameof(usrEvent.sEventID)}
    };

    private List<usrEvent> itemSource = new();

    public string? EquipmentID { get; set; }

    public string? sEventID { get; set; }

    public string? sEventDesc { get; set; }

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
            IQueryable<usrEvent> userQuery = context.usrEvents.Where(s => s.dtDate >= StartDate && s.dtDate <= EndDate);

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
        using (var context = contextFactory.CreateDbContext())
        {
            IQueryable<usrEvent> userQuery = context.usrEvents.Where(s => s.dtDate >= StartDate && s.dtDate <= EndDate);
            if (EquipmentID != null)
            {
                userQuery = userQuery.Where(s => s.sEquipmentID.Contains(EquipmentID));
            }
            if (sEventID != null)
            {
                userQuery = userQuery.Where(s => s.sEventID.Contains(sEventID));
            }
            if (sEventDesc != null)
            {
                userQuery = userQuery.Where(s => s.sEventDesc.Contains(sEventDesc));
            }
            itemSource = await userQuery.OrderByDescending(s => s.dtDate).ThenByDescending(s => s.sTime).ToListAsync();
        }
    }

    private async Task ExportExcel()
    {
        string path = Path.Combine(AppContext.BaseDirectory, $"Events-{DateTime.Now.ToString("yyyyMMdd")}.xlsx");
        Workbook book = new Workbook();
        Worksheet sheet = book.Worksheets[0];
        sheet.Cells[0, 0].PutValue("设备编号");
        sheet.Cells[0, 1].PutValue("日期");
        sheet.Cells[0, 2].PutValue("事件时间");
        sheet.Cells[0, 3].PutValue("日志描述");
        sheet.Cells[0, 4].PutValue("钻带文件");
        sheet.Cells[0, 5].PutValue("事件代码");
        int i = 0;
        foreach (usrEvent v in itemSource)
        {
            i++;
            sheet.Cells[i, 0].PutValue(v.sEquipmentID);
            sheet.Cells[i, 1].PutValue(v.dtDate);
            sheet.Cells[i, 2].PutValue(v.sTime);
            sheet.Cells[i, 3].PutValue(v.sEventDesc);
            sheet.Cells[i, 3].PutValue(v.sActProgram);
            sheet.Cells[i, 3].PutValue(v.sEventID);
        }
        book.Save(path, SaveFormat.Xlsx);
        byte[] contentBytes = File.ReadAllBytes(path);

        await BlazorDownloadFileService.DownloadFile(Path.GetFileName(path), contentBytes, "application/octet-stream");
    }
}
