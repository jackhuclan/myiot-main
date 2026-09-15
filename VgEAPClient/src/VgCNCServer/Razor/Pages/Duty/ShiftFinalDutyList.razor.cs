using Aspose.Cells;
using BlazorDownloadFile;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Quartz.Util;
using VgCNCServer.DAO;
using VgCNCServer.DAO.Models;

namespace VgCNCServer.Razor.Pages.Duty;

public partial class ShiftFinalDutyList
{
    [Inject] private IBlazorDownloadFileService BlazorDownloadFileService { get; set; }
    [Inject] private IDbContextFactory<VegaContext> contextFactory { get; set; }

    private readonly List<DataTableHeader<usrShiftDuty>> _headers = new List<DataTableHeader<usrShiftDuty>>
    {
      new (){Text= "机台编号",Align= DataTableHeaderAlign.Start,Sortable= true,Value= nameof(usrShiftDuty.sEquipmentID)},
      new (){Text= "方式", Value= nameof(usrShiftDuty.sZ)},
      new (){ Text= "日期", Value= nameof(usrShiftDuty.dtDate)},
      new (){ Text= "采集时间", Value= nameof(usrShiftDuty.sTime)},
      new (){ Text= "稼动率", Value= nameof(usrShiftDuty.sDuty)},
      new (){ Text= "轴数", Value= nameof(usrShiftDuty.sT)},
      new (){ Text= "加工时间", Value= nameof(usrShiftDuty.sWorkTime)},
      new (){ Text= "等待时间", Value= nameof(usrShiftDuty.sWaitTime)},
      new (){ Text= "停机时间", Value= nameof(usrShiftDuty.sStopTime)},
      new (){ Text= "总时间", Value= nameof(usrShiftDuty.sTotalTime)},
      new (){ Text= "孔数", Value= nameof(usrShiftDuty.sHits)},
      new (){ Text= "班次总孔数", Value= nameof(usrShiftDuty.sShiftHits)},
      new (){ Text= "断刀", Value= nameof(usrShiftDuty.sBrokens)},
      new (){ Text= "换板次数", Value= nameof(usrShiftDuty.sChangePanels)},
      new (){ Text= "换料次数", Value= nameof(usrShiftDuty.sChangeDrills)},
      new (){ Text= "换料时间", Value= nameof(usrShiftDuty.sChangeDrillsTime)},
      //new (){ Text= "注册日期", Value= nameof(usrShiftDuty.sRegistrationDate)},
      new (){ Text= "所属班次", Value= nameof(usrShiftDuty.sShift)}
    };

    private List<usrShiftDuty> _desserts = new List<usrShiftDuty>
    {
    };

    public string? EquipmentID { get; set; }

    public string? Shift { get; set; }

    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddDays(-7));

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
            IQueryable<usrShiftDuty> userQuery = context.usrShiftDutys.Where(s => s.dtDate >= StartDate && s.dtDate <= EndDate);

            if (selectedEquipments.Any())
            {
                userQuery = userQuery.Where(s => selectedEquipments.Contains(s.sEquipmentID));
            }

            _desserts = await userQuery.OrderByDescending(s => s.dtDate).ThenByDescending(s => s.sTime).ToListAsync();
        }

        queryResult = $"查询结果:已选择设备 - {string.Join(", ", selectedEquipments)}";
        StateHasChanged();
    }

    #endregion 新增选择设备

    private Task QueryDetail()
    {
        string sql = string.Empty;
        List<MySqlParameter> mySqls = new List<MySqlParameter>();
        using (var context = contextFactory.CreateDbContext())
        {
            string sqlwheredate = string.Empty;
            string sqlwhere = string.Empty;

            sqlwheredate += $@" AND dtDate >= @sStartDate ";
            mySqls.Add(new MySqlParameter("sStartDate", StartDate));

            sqlwheredate += $@" AND dtDate <= @sEndDate ";
            mySqls.Add(new MySqlParameter("sEndDate", EndDate));

            if (!EquipmentID.IsNullOrWhiteSpace())
            {
                sqlwhere += $@" AND sEquipmentID LIKE @sEquipmentID ";
                mySqls.Add(new MySqlParameter("sEquipmentID", $@"%{EquipmentID}%"));
            }

            if (!Shift.IsNullOrWhiteSpace())
            {
                sqlwhere += $@" AND sShift LIKE @sShift ";
                mySqls.Add(new MySqlParameter("sShift", $@"%{Shift}%"));
            }

            sql = $@"select 
* 
from(
	select 
		row_number() over(partition by sEquipmentID,dayshift order by dtDate desc,sTime desc) as cnt,
		s2.*
	from(
		select 
			s1.*,
			case when sTime < '08:00:00' then CONCAT(dtDate - INTERVAL 1 day,'_N') 
				when sTime > '20:00:00' then CONCAT(dtDate,'_N')
				else CONCAT(dtDate,'_D') 
				end AS dayshift
		from(
			select
				sInnerID,
				sZ,
				case when sTime < '08:00:00' then dtDate + INTERVAL 1 day else dtDate end as dtDate,
				sTime,
				sEquipmentID,
                sEqpType,
				sDuty,
				sT,
				sWorkTime,
				sWaitTime,
				sStopTime,
				sTotalTime,
				sHits,
				sShiftHits,
				sTools,
				sBrokens,
				sChangePanels,
				sChangeDrills,
				sChangeDrillsTime,
				sRegistrationDate,
				sShift
			from usrshiftduty where sDuty is not null and trim(sDuty) <> ''
            and sWorkTime is not null and trim(sWorkTime) <> ''
            and sZ is not null and trim(sZ) <> ''
            and sHits is not null and trim(sHits) <> ''
            and sShiftHits is not null and trim(sShiftHits) <> '' and trim(sShiftHits) <> '0'
            {sqlwhere}
		) as s1 where 1=1 {sqlwheredate}
	) as s2
) as usrshiftfinalduty where cnt = 1 ";

            _desserts = context.usrShiftDutys.FromSqlRaw(sql, mySqls.ToArray()).OrderByDescending(s => s.dtDate).ThenByDescending(s => s.sTime).ToList();
        }
        return Task.CompletedTask;
    }

    private async Task ExportExcel()
    {
        string path = Path.Combine(AppContext.BaseDirectory, $"ShiftDuey-{DateTime.Now.ToString("yyyyMMdd")}.xlsx");
        Workbook book = new Workbook();
        Worksheet sheet = book.Worksheets[0];
        sheet.Cells[0, 0].PutValue("设备编号");
        sheet.Cells[0, 1].PutValue("方式");
        sheet.Cells[0, 2].PutValue("日期时间");
        sheet.Cells[0, 3].PutValue("稼动率");
        sheet.Cells[0, 4].PutValue("轴数");
        sheet.Cells[0, 5].PutValue("加工时间");
        sheet.Cells[0, 6].PutValue("等待时间");
        sheet.Cells[0, 7].PutValue("停机时间");
        sheet.Cells[0, 8].PutValue("总时间");
        sheet.Cells[0, 9].PutValue("孔数");
        sheet.Cells[0, 10].PutValue("班次总孔数");
        sheet.Cells[0, 11].PutValue("断刀");
        sheet.Cells[0, 12].PutValue("换板次数");
        sheet.Cells[0, 13].PutValue("换料次数");
        sheet.Cells[0, 14].PutValue("换料时间");
        sheet.Cells[0, 15].PutValue("所属班次");
        int i = 0;
        foreach (usrShiftDuty v in _desserts)
        {
            i++;
            sheet.Cells[i, 0].PutValue(v.sEquipmentID);
            sheet.Cells[i, 1].PutValue(v.sZ);
            sheet.Cells[i, 2].PutValue(v.dtDate + " " + v.sTime);
            sheet.Cells[i, 3].PutValue(v.sDuty);
            sheet.Cells[i, 4].PutValue(v.sT);
            sheet.Cells[i, 5].PutValue(v.sWorkTime);
            sheet.Cells[i, 6].PutValue(v.sWaitTime);
            sheet.Cells[i, 7].PutValue(v.sStopTime);
            sheet.Cells[i, 8].PutValue(v.sTotalTime);
            sheet.Cells[i, 9].PutValue(v.sHits);
            sheet.Cells[i, 10].PutValue(v.sShiftHits);
            sheet.Cells[i, 11].PutValue(v.sBrokens);
            sheet.Cells[i, 12].PutValue(v.sChangePanels);
            sheet.Cells[i, 13].PutValue(v.sChangeDrills);
            sheet.Cells[i, 14].PutValue(v.sChangeDrillsTime);
            sheet.Cells[i, 15].PutValue(v.sShift);
        }
        book.Save(path, SaveFormat.Xlsx);
        byte[] contentBytes = File.ReadAllBytes(path);

        await BlazorDownloadFileService.DownloadFile(Path.GetFileName(path), contentBytes, "application/octet-stream");
    }
}
