using Aspose.Cells;
using BlazorDownloadFile;
using Microsoft.EntityFrameworkCore;
using VgCNCServer.DAO;
using VgCNCServer.DAO.Models;

namespace VgCNCServer.Razor.Pages.Event;

public partial class M54List
{
    [Inject] private IBlazorDownloadFileService BlazorDownloadFileService { get; set; }
    [Inject] private IDbContextFactory<VegaContext> contextFactory { get; set; }

    private readonly List<DataTableHeader<usrM54Event>> dataTableHeaders = new List<DataTableHeader<usrM54Event>>
    {
      new (){ Text= "机台编号",Align= DataTableHeaderAlign.Start,Sortable= false,Value= nameof(usrM54Event.sEquipmentID)},
      new (){ Text= "日期", Value= nameof(usrM54Event.dtDate)},
      new (){ Text= "事件时间", Value= nameof(usrM54Event.sTime)},
      new (){ Text= "日志描述", Value= nameof(usrM54Event.sEventDesc)},
      new (){ Text= "钻带文件", Value= nameof(usrM54Event.sActProgram)},
      new (){ Text= "事件代码", Value= nameof(usrM54Event.sEventID)}
    };

    private List<usrM54Event> itemSource = new();

    public string? EquipmentID { get; set; }

    public string? sEventID { get; set; }

    public string? sEventDesc { get; set; } = "COMM M52";

    public DateOnly? StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public DateOnly? EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public TimeOnly? StartTime { get; set; } = new TimeOnly(8, 00);

    public TimeOnly? EndTime { get; set; } = new TimeOnly(20, 00);

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
            IQueryable<usrM54Event> userQuery = context.usrM54Events.Where(s => s.dtDate >= StartDate && s.dtDate <= EndDate);

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
        #region
        //IQueryable<usrM54Event> userQuery = ctx.usrM54Events.Where(s => s.dtDate >= StartDate && s.dtDate <= EndDate);
        //IQueryable<usrM54Event> userQuery = ctx.usrM54Events
        //.Where(s => (s.dtDate == StartDate &&
        //(EF.Functions.Like(s.sTime, "08:%") || EF.Functions.Like(s.sTime, "09:%") ||
        //  EF.Functions.Like(s.sTime, "10:%") || EF.Functions.Like(s.sTime, "11:%") ||
        //  EF.Functions.Like(s.sTime, "12:%") || EF.Functions.Like(s.sTime, "13:%") ||
        //  EF.Functions.Like(s.sTime, "14:%") || EF.Functions.Like(s.sTime, "15:%") ||
        //  EF.Functions.Like(s.sTime, "16:%") || EF.Functions.Like(s.sTime, "17:%") ||
        //  EF.Functions.Like(s.sTime, "18:%") || EF.Functions.Like(s.sTime, "19:%") ||
        //  EF.Functions.Like(s.sTime, "20:%") || EF.Functions.Like(s.sTime, "21:%") ||
        //  EF.Functions.Like(s.sTime, "22:%") || EF.Functions.Like(s.sTime, "23:%")))
        //|| (s.dtDate == EndDate.AddDays(1) &&
        //(EF.Functions.Like(s.sTime, "00:%") || EF.Functions.Like(s.sTime, "01:%") ||
        //  EF.Functions.Like(s.sTime, "02:%") || EF.Functions.Like(s.sTime, "03:%") ||
        //  EF.Functions.Like(s.sTime, "04:%") || EF.Functions.Like(s.sTime, "05:%") ||
        //  EF.Functions.Like(s.sTime, "06:%") || EF.Functions.Like(s.sTime, "07:%"))));
        //string startTimeStr = StartTime?.ToString("HH:mm") ?? "00:00";
        //string endTimeStr = EndTime?.ToString("HH:mm") ?? "23:59";
        //IQueryable<usrM54Event> userQuery = ctx.usrM54Events.FromSqlRaw(
        //                "SELECT * FROM usrM54Event WHERE CONVERT(time,SUBSTRING(sTime, 1, 5), 108) BETWEEN {0} AND {1} AND dtDate = {2} AND dtDate <= {3}",
        //                startTimeStr,
        //                endTimeStr,
        //                StartDate.ToString("yyyy-MM-dd"),
        //                EndDate.AddDays(1).ToString("yyyy-MM-dd"));
        #endregion
        using (var context = contextFactory.CreateDbContext())
        {
            DateTime? startTime = GetStartDateTime();
            DateTime? endTime = GetEndDateTime();

            string sql = string.Format("SELECT * FROM usrM54Event WHERE CONVERT(CONCAT(dtDate, ' ', LEFT(sTime, 5)),datetime)  BETWEEN '{0}'AND '{1}'", startTime, endTime);
            if (EquipmentID != null)
            {
                sql = sql + "and sEquipmentID like '%" + EquipmentID.Trim() + "%'";
            }
            if (sEventID != null)
            {
                sql = sql + "and sEventID like '%" + sEventID.Trim() + "%'";
            }
            if (sEventDesc != null)
            {
                sql = sql + "and sEventDesc like '%" + sEventDesc.Trim() + "%'";
            }

            IQueryable<usrM54Event> userQuery = context.usrM54Events.FromSqlRaw(sql);
            itemSource = await userQuery.OrderByDescending(s => s.dtDate).ThenByDescending(s => s.sTime).ToListAsync();
        }
    }

    private DateTime? GetStartDateTime()
    {
        if (StartDate != null && StartTime.HasValue)
        {
            return new DateTime(StartDate.Value.Year, StartDate.Value.Month, StartDate.Value.Day, StartTime.Value.Hour, StartTime.Value.Minute, 0);
        }
        return null;
    }

    private DateTime? GetEndDateTime()
    {
        if (EndDate != null && EndTime.HasValue)
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, EndTime.Value.Hour, EndTime.Value.Minute, 0);
        }
        return null;
    }

    private async Task ExportExcel()
    {
        string path = Path.Combine(AppContext.BaseDirectory, $"M54Events-{DateTime.Now.ToString("yyyyMMdd")}.xlsx");
        Workbook book = new Workbook();
        Worksheet sheet = book.Worksheets[0];
        sheet.Cells[0, 0].PutValue("设备编号");
        sheet.Cells[0, 1].PutValue("日期");
        sheet.Cells[0, 2].PutValue("事件时间");
        sheet.Cells[0, 3].PutValue("日志描述");
        sheet.Cells[0, 4].PutValue("钻带文件");
        sheet.Cells[0, 5].PutValue("事件代码");
        int i = 0;
        foreach (usrM54Event v in itemSource)
        {
            i++;
            sheet.Cells[i, 0].PutValue(v.sEquipmentID);
            sheet.Cells[i, 1].PutValue(v.dtDate);
            sheet.Cells[i, 2].PutValue(v.sTime);
            sheet.Cells[i, 3].PutValue(v.sEventDesc);
            sheet.Cells[i, 4].PutValue(v.sActProgram);
            sheet.Cells[i, 5].PutValue(v.sEventID);
        }
        book.Save(path, SaveFormat.Xlsx);
        byte[] contentBytes = File.ReadAllBytes(path);

        await BlazorDownloadFileService.DownloadFile(Path.GetFileName(path), contentBytes, "application/octet-stream");
    }
}
