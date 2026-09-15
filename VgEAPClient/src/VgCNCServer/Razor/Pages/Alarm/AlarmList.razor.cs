using Aspose.Cells;
using BlazorDownloadFile;
using Microsoft.EntityFrameworkCore;
using VgCNCServer.DAO;
using VgCNCServer.DAO.Models;

namespace VgCNCServer.Razor.Pages.Alarm;

public partial class AlarmList
{
    [Inject] private IBlazorDownloadFileService BlazorDownloadFileService { get; set; }

    [Inject] private IDbContextFactory<VegaContext> contextFactory { get; set; }

    private readonly List<DataTableHeader<usrAlarm>> dataTableHeaders = new List<DataTableHeader<usrAlarm>>
    {
      new (){Text= "机台编号",Align= DataTableHeaderAlign.Start,Sortable= false,Value= nameof(usrAlarm.sEquipmentID)},
      new (){ Text= "日期", Value= nameof(usrAlarm.dtDate)},
      new (){ Text= "开始时间", Value= nameof(usrAlarm.sStartTime)},
      new (){ Text= "结束时间", Value= nameof(usrAlarm.sEndTime)},
      new (){ Text= "报警指令", Value= nameof(usrAlarm.sAlarmID)},
      new (){ Text= "报警说明", Value= nameof(usrAlarm.sAlarmDesc)},
      new (){ Text= "钻带文件", Value= nameof(usrAlarm.sActProgram)}
    };

    private List<usrAlarm> itemSource = new();

    public string? EquipmentID { get; set; }

    public string? sAlarmID { get; set; }

    public string? sAlarmDesc { get; set; }

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
            IQueryable<usrAlarm> userQuery = context.usrAlarms.Where(s => s.dtDate >= StartDate && s.dtDate <= EndDate);

            if (selectedEquipments.Any())
            {
                userQuery = userQuery.Where(s => selectedEquipments.Contains(s.sEquipmentID));
            }

            itemSource = await userQuery.OrderByDescending(s => s.dtDate).ThenByDescending(s => s.sStartTime).ToListAsync();
        }

        queryResult = $"查询结果:已选择设备 - {string.Join(", ", selectedEquipments)}";
        StateHasChanged();
    }

    #endregion 新增选择设备

    private async Task QueryDetail()
    {
        using (var context = contextFactory.CreateDbContext())
        {
            IQueryable<usrAlarm> userQuery = context.usrAlarms.Where(s => s.dtDate >= StartDate && s.dtDate <= EndDate);
            if (EquipmentID != null)
            {
                userQuery = userQuery.Where(s => s.sEquipmentID.Contains(EquipmentID));
            }
            if (sAlarmID != null)
            {
                userQuery = userQuery.Where(s => s.sAlarmID.Contains(sAlarmID));
            }
            if (sAlarmDesc != null)
            {
                userQuery = userQuery.Where(s => s.sAlarmDesc.Contains(sAlarmDesc));
            }
            itemSource = await userQuery.OrderByDescending(s => s.dtDate).ThenByDescending(s => s.sStartTime).ToListAsync();
        }
    }

    private async Task ExportExcel()
    {
        string path = Path.Combine(AppContext.BaseDirectory, $"Alarm-{DateTime.Now.ToString("yyyyMMdd")}.xlsx");
        Workbook book = new Workbook();
        Worksheet sheet = book.Worksheets[0];
        sheet.Cells[0, 0].PutValue("设备编号");
        sheet.Cells[0, 1].PutValue("日期");
        sheet.Cells[0, 2].PutValue("开始时间");
        sheet.Cells[0, 3].PutValue("结束时间");
        sheet.Cells[0, 4].PutValue("报警指令");
        sheet.Cells[0, 5].PutValue("报警说明");
        sheet.Cells[0, 6].PutValue("钻带文件");
        int i = 0;
        foreach (usrAlarm v in itemSource)
        {
            i++;
            sheet.Cells[i, 0].PutValue(v.sEquipmentID);
            sheet.Cells[i, 1].PutValue(v.dtDate);
            sheet.Cells[i, 2].PutValue(v.sStartTime);
            sheet.Cells[i, 3].PutValue(v.sEndTime);
            sheet.Cells[i, 4].PutValue(v.sAlarmID);
            sheet.Cells[i, 5].PutValue(v.sAlarmDesc);
            sheet.Cells[i, 6].PutValue(v.sActProgram);
        }
        book.Save(path, SaveFormat.Xlsx);
        byte[] contentBytes = File.ReadAllBytes(path);

        await BlazorDownloadFileService.DownloadFile(Path.GetFileName(path), contentBytes, "application/octet-stream");
    }
}
