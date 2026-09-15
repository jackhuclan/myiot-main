using Aspose.Cells;
using BlazorDownloadFile;
using Microsoft.EntityFrameworkCore;
using VgCNCServer.DAO;
using VgCNCServer.DAO.Models;

namespace VgCNCServer.Razor.Pages.Broken;

public partial class BrokenEndList
{
    [Inject] private IBlazorDownloadFileService BlazorDownloadFileService { get; set; }
    [Inject] private IDbContextFactory<VegaContext> contextFactory { get; set; }

    private readonly List<DataTableHeader<usrToolsBrokenEnd>> dataTableHeaders = new List<DataTableHeader<usrToolsBrokenEnd>>
    {
      new (){Text= "机台编号",Align= DataTableHeaderAlign.Start,Sortable= false,Value= nameof(usrToolsBrokenEnd.sEquipmentID)},
      new (){ Text= "日期", Value= nameof(usrToolsBrokenEnd.dtDate)},
      new (){ Text= "结束时间", Value= nameof(usrToolsBrokenEnd.sTime)},
      new (){ Text= "钻带文件", Value= nameof(usrToolsBrokenEnd.sActProgram)},
      new (){ Text= "断刀次数", Value= nameof(usrToolsBrokenEnd.sBrokens)},
      new (){ Text= "断刀轴号", Value= nameof(usrToolsBrokenEnd.sSpindle)},
      new (){ Text= "刀具号", Value= nameof(usrToolsBrokenEnd.sToolID)},
      new (){ Text= "刀径", Value= nameof(usrToolsBrokenEnd.sToolDIA)},
      new (){ Text= "孔号", Value= nameof(usrToolsBrokenEnd.sHoleID)},
      new (){ Text= "X坐标", Value= nameof(usrToolsBrokenEnd.sX)},
      new (){ Text= "Y坐标", Value= nameof(usrToolsBrokenEnd.sY)},
      new (){ Text= "程序块", Value= nameof(usrToolsBrokenEnd.sProgramBlock)},
      new (){ Text= "程序阶级", Value= nameof(usrToolsBrokenEnd.sProgramStep)}
    };

    private List<usrToolsBrokenEnd> itemSource = new();

    public string? EquipmentID { get; set; }

    public string? ActProgram { get; set; }

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

        using (var context = contextFactory.CreateDbContext())
        {
            IQueryable<usrToolsBrokenEnd> userQuery = context.usrToolsBrokenEnds.Where(s => s.dtDate >= StartDate && s.dtDate <= EndDate);

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
            IQueryable<usrToolsBrokenEnd> userQuery = context.usrToolsBrokenEnds.Where(s => s.dtDate >= StartDate && s.dtDate <= EndDate);
            if (EquipmentID != null)
            {
                userQuery = userQuery.Where(s => s.sEquipmentID.Contains(EquipmentID));
            }
            if (ActProgram != null)
            {
                userQuery = userQuery.Where(s => s.sActProgram.Contains(ActProgram));
            }
            itemSource = await userQuery.OrderByDescending(s => s.dtDate).ThenByDescending(s => s.sTime).ToListAsync();
        }
    }

    private async Task ExportExcel()
    {
        string path = Path.Combine(AppContext.BaseDirectory, $"BrokenEnd-{DateTime.Now.ToString("yyyyMMdd")}.xlsx");
        Workbook book = new Workbook();
        Worksheet sheet = book.Worksheets[0];
        sheet.Cells[0, 0].PutValue("设备编号");
        sheet.Cells[0, 1].PutValue("日期");
        sheet.Cells[0, 2].PutValue("结束时间");
        sheet.Cells[0, 3].PutValue("钻带文件");
        sheet.Cells[0, 4].PutValue("断刀次数");
        sheet.Cells[0, 5].PutValue("断刀轴号");
        sheet.Cells[0, 6].PutValue("刀具号");
        sheet.Cells[0, 7].PutValue("刀径");
        sheet.Cells[0, 8].PutValue("孔号");
        sheet.Cells[0, 9].PutValue("X坐标");
        sheet.Cells[0, 10].PutValue("Y坐标");
        sheet.Cells[0, 11].PutValue("程序块");
        sheet.Cells[0, 12].PutValue("程序阶级");
        int i = 0;
        foreach (var v in itemSource)
        {
            i++;
            sheet.Cells[i, 0].PutValue(v.sEquipmentID);
            sheet.Cells[i, 1].PutValue(v.dtDate);
            sheet.Cells[i, 2].PutValue(v.sTime);
            sheet.Cells[i, 3].PutValue(v.sActProgram);
            sheet.Cells[i, 4].PutValue(v.sBrokens);
            sheet.Cells[i, 5].PutValue(v.sSpindle);
            sheet.Cells[i, 6].PutValue(v.sToolID);
            sheet.Cells[i, 7].PutValue(v.sToolDIA);
            sheet.Cells[i, 8].PutValue(v.sHoleID);
            sheet.Cells[i, 9].PutValue(v.sX);
            sheet.Cells[i, 10].PutValue(v.sY);
            sheet.Cells[i, 11].PutValue(v.sProgramBlock);
            sheet.Cells[i, 12].PutValue(v.sProgramStep);
        }
        book.Save(path, SaveFormat.Xlsx);
        byte[] contentBytes = File.ReadAllBytes(path);

        await BlazorDownloadFileService.DownloadFile(Path.GetFileName(path), contentBytes, "application/octet-stream");
    }
}
