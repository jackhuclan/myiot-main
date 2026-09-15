// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using VgCNCServer.DAO;
using VgCNCServer.DAO.Models;
using VgCNCWeb.Models;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace VgCNCWeb.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DutyController : ControllerBase
{
    private readonly IDbContextFactory<VegaContext> _contextFactory;

    public DutyController(IDbContextFactory<VegaContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }


    /// <summary>
    /// 标准稼动率查询
    /// </summary>
    /// <param name="dutyReqModel"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("QueryDutyList")]
    public ReturnModel<usrDuty> QueryDutyList([FromBody] DutyReqModel dutyReqModel)
    {
        List<usrDuty> itemSource = new List<usrDuty>();
        string sql = string.Empty;
        List<MySqlParameter> mySqls = new List<MySqlParameter>();
        int Total = 0;
        string TableName = "usrDuty";

        using (var context = _contextFactory.CreateDbContext())
        {
            sql = $@" SELECT * FROM {TableName} WHERE 1=1 ";

            if (!string.IsNullOrEmpty(dutyReqModel.sEquipmentID))
            {
                sql += $@" AND sEquipmentID LIKE @sEquipmentID ";
                mySqls.Add(new MySqlParameter("sEquipmentID", $@"%{dutyReqModel.sEquipmentID}%"));
            }

            DateOnly StartDate, EndDate;
            if (DateOnly.TryParse(dutyReqModel.sStartDate, out StartDate))
            {
                sql += $@" AND dtDate >= @sStartDate ";
                mySqls.Add(new MySqlParameter("sStartDate", StartDate));
            }
            if (DateOnly.TryParse(dutyReqModel.sEndDate, out EndDate))
            {
                sql += $@" AND dtDate <= @sEndDate ";
                mySqls.Add(new MySqlParameter("sEndDate", EndDate));
            }

            itemSource = context.usrDutys.FromSqlRaw(sql, mySqls.ToArray()).OrderByDescending(s => s.dtDate).ToList();
            Total = itemSource.Count;
            if (dutyReqModel.PageNum > 0 && dutyReqModel.PageSize > 0)
            {
                int startIndex = (dutyReqModel.PageNum - 1) * dutyReqModel.PageSize;
                int endIndex = startIndex + dutyReqModel.PageSize - 1;
                itemSource = itemSource.Skip(startIndex).Take(endIndex - startIndex + 1).ToList();
            }
        }

        ReturnModel<usrDuty> remodel = new ReturnModel<usrDuty>
        {
            PageNum = dutyReqModel.PageNum,
            PageSize = dutyReqModel.PageSize,
            Total = Total,
            List = itemSource
        };
        return remodel;
    }

    /// <summary>
    /// 实时稼动率查询
    /// </summary>
    /// <param name="dutyReqModel"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("QueryRealTimeDutyList")]
    public ReturnModel<usrRealTimeDuty> QueryRealTimeDutyList([FromBody] DutyReqModel dutyReqModel)
    {
        List<usrRealTimeDuty> itemSource = new List<usrRealTimeDuty>();
        string sql = string.Empty;
        List<MySqlParameter> mySqls = new List<MySqlParameter>();
        int Total = 0;
        string TableName = "usrRealTimeDuty";

        using (var context = _contextFactory.CreateDbContext())
        {
            sql = $@" SELECT * FROM {TableName} WHERE 1=1 ";

            if (!string.IsNullOrEmpty(dutyReqModel.sEquipmentID))
            {
                sql += $@" AND sEquipmentID LIKE @sEquipmentID ";
                mySqls.Add(new MySqlParameter("sEquipmentID", $@"%{dutyReqModel.sEquipmentID}%"));
            }

            DateOnly StartDate, EndDate;
            if (DateOnly.TryParse(dutyReqModel.sStartDate, out StartDate))
            {
                sql += $@" AND dtDate >= @sStartDate ";
                mySqls.Add(new MySqlParameter("sStartDate", StartDate));
            }
            if (DateOnly.TryParse(dutyReqModel.sEndDate, out EndDate))
            {
                sql += $@" AND dtDate <= @sEndDate ";
                mySqls.Add(new MySqlParameter("sEndDate", EndDate));
            }

            itemSource = context.usrRealTimeDutys.FromSqlRaw(sql, mySqls.ToArray()).OrderByDescending(s => s.dtDate).ThenByDescending(s => s.sTime).ToList();
            Total = itemSource.Count;
            if (dutyReqModel.PageNum > 0 && dutyReqModel.PageSize > 0)
            {
                int startIndex = (dutyReqModel.PageNum - 1) * dutyReqModel.PageSize;
                int endIndex = startIndex + dutyReqModel.PageSize - 1;
                itemSource = itemSource.Skip(startIndex).Take(endIndex - startIndex + 1).ToList();
            }
        }

        ReturnModel<usrRealTimeDuty> remodel = new ReturnModel<usrRealTimeDuty>
        {
            PageNum = dutyReqModel.PageNum,
            PageSize = dutyReqModel.PageSize,
            Total = Total,
            List = itemSource
        };
        return remodel;
    }

    /// <summary>
    /// 班次稼动率查询
    /// </summary>
    /// <param name="dutyReqModel"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("QueryShiftDutyList")]
    public ReturnModel<usrShiftDuty> QueryShiftDutyList([FromBody] DutyShiftReqModel dutyReqModel)
    {
        List<usrShiftDuty> itemSource = new List<usrShiftDuty>();
        string sql = string.Empty;
        List<MySqlParameter> mySqls = new List<MySqlParameter>();
        int Total = 0;
        string TableName = "usrShiftDuty";

        using (var context = _contextFactory.CreateDbContext())
        {
            sql = $@" SELECT * FROM {TableName} WHERE 1=1 ";

            if (!string.IsNullOrEmpty(dutyReqModel.sEquipmentID))
            {
                sql += $@" AND sEquipmentID LIKE @sEquipmentID ";
                mySqls.Add(new MySqlParameter("sEquipmentID", $@"%{dutyReqModel.sEquipmentID}%"));
            }

            DateOnly StartDate, EndDate;
            if (DateOnly.TryParse(dutyReqModel.sStartDate, out StartDate))
            {
                sql += $@" AND dtDate >= @sStartDate ";
                mySqls.Add(new MySqlParameter("sStartDate", StartDate));
            }
            if (DateOnly.TryParse(dutyReqModel.sEndDate, out EndDate))
            {
                sql += $@" AND dtDate <= @sEndDate ";
                mySqls.Add(new MySqlParameter("sEndDate", EndDate));
            }

            if (!string.IsNullOrEmpty(dutyReqModel.sShift))
            {
                sql += $@" AND sShift LIKE @sShift ";
                mySqls.Add(new MySqlParameter("sShift", $@"%{dutyReqModel.sShift}%"));
            }

            itemSource = context.usrShiftDutys.FromSqlRaw(sql, mySqls.ToArray()).OrderByDescending(s => s.dtDate).ThenByDescending(s => s.sTime).ToList();
            Total = itemSource.Count;
            if (dutyReqModel.PageNum > 0 && dutyReqModel.PageSize > 0)
            {
                int startIndex = (dutyReqModel.PageNum - 1) * dutyReqModel.PageSize;
                int endIndex = startIndex + dutyReqModel.PageSize - 1;
                itemSource = itemSource.Skip(startIndex).Take(endIndex - startIndex + 1).ToList();
            }
        }

        ReturnModel<usrShiftDuty> remodel = new ReturnModel<usrShiftDuty>
        {
            PageNum = dutyReqModel.PageNum,
            PageSize = dutyReqModel.PageSize,
            Total = Total,
            List = itemSource
        };
        return remodel;
    }


    /// <summary>
    /// 稼动率分析查询
    /// </summary>
    /// <param name="dutyReqModel"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("QueryAnalysisDutyList")]
    public ReturnModel<usrAnalysisDuty> QueryAnalysisDutyList([FromBody] DutyShiftReqModel dutyReqModel)
    {
        List<usrAnalysisDuty> itemSource = new List<usrAnalysisDuty>();
        string sql = string.Empty;
        List<MySqlParameter> mySqls = new List<MySqlParameter>();
        int Total = 0;
        string TableName = "usrAnalysisDuty";

        using (var context = _contextFactory.CreateDbContext())
        {
            sql = $@" SELECT * FROM {TableName} WHERE 1=1 ";

            if (!string.IsNullOrEmpty(dutyReqModel.sEquipmentID))
            {
                sql += $@" AND sEquipmentID LIKE @sEquipmentID ";
                mySqls.Add(new MySqlParameter("sEquipmentID", $@"%{dutyReqModel.sEquipmentID}%"));
            }

            DateOnly StartDate, EndDate;
            if (DateOnly.TryParse(dutyReqModel.sStartDate, out StartDate))
            {
                sql += $@" AND dtDate >= @sStartDate ";
                mySqls.Add(new MySqlParameter("sStartDate", StartDate));
            }
            if (DateOnly.TryParse(dutyReqModel.sEndDate, out EndDate))
            {
                sql += $@" AND dtDate <= @sEndDate ";
                mySqls.Add(new MySqlParameter("sEndDate", EndDate));
            }

            if (!string.IsNullOrEmpty(dutyReqModel.sShift))
            {
                sql += $@" AND sShift LIKE @sShift ";
                mySqls.Add(new MySqlParameter("sShift", $@"%{dutyReqModel.sShift}%"));
            }

            itemSource = context.usrAnalysisDutys.FromSqlRaw(sql, mySqls.ToArray()).OrderByDescending(s => s.dtDate).ThenByDescending(s => s.sTime).ToList();
            Total = itemSource.Count;
            if (dutyReqModel.PageNum > 0 && dutyReqModel.PageSize > 0)
            {
                int startIndex = (dutyReqModel.PageNum - 1) * dutyReqModel.PageSize;
                int endIndex = startIndex + dutyReqModel.PageSize - 1;
                itemSource = itemSource.Skip(startIndex).Take(endIndex - startIndex + 1).ToList();
            }
        }

        ReturnModel<usrAnalysisDuty> remodel = new ReturnModel<usrAnalysisDuty>
        {
            PageNum = dutyReqModel.PageNum,
            PageSize = dutyReqModel.PageSize,
            Total = Total,
            List = itemSource
        };
        return remodel;
    }

    /// <summary>
    /// 班次最终稼动率查询
    /// </summary>
    /// <param name="dutyReqModel"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("QueryShiftFinalDutyList")]
    public ReturnModel<usrShiftDuty> QueryShiftFinalDutyList([FromBody] DutyShiftReqModel dutyReqModel)
    {
        List<usrShiftDuty> itemSource = new List<usrShiftDuty>();
        string sql = string.Empty;
        List<MySqlParameter> mySqls = new List<MySqlParameter>();
        int Total = 0;
        //string TableName = $@"";

        using (var context = _contextFactory.CreateDbContext())
        {
            string sqlwheredate = string.Empty;
            string sqlwhere = string.Empty;



            if (!string.IsNullOrEmpty(dutyReqModel.sEquipmentID))
            {
                sqlwhere += $@" AND sEquipmentID LIKE @sEquipmentID ";
                mySqls.Add(new MySqlParameter("sEquipmentID", $@"%{dutyReqModel.sEquipmentID}%"));
            }

            DateOnly StartDate, EndDate;
            if (DateOnly.TryParse(dutyReqModel.sStartDate, out StartDate))
            {
                sqlwheredate += $@" AND dtDate >= @sStartDate ";
                mySqls.Add(new MySqlParameter("sStartDate", StartDate));
            }
            if (DateOnly.TryParse(dutyReqModel.sEndDate, out EndDate))
            {
                sqlwheredate += $@" AND dtDate <= @sEndDate ";
                mySqls.Add(new MySqlParameter("sEndDate", EndDate));
            }

            if (!string.IsNullOrEmpty(dutyReqModel.sShift))
            {
                sqlwhere += $@" AND sShift LIKE @sShift ";
                mySqls.Add(new MySqlParameter("sShift", $@"%{dutyReqModel.sShift}%"));
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
			from usrshiftduty where sDuty is not null and trim(sDuty) <> '' {sqlwhere}
		) as s1 where 1=1 {sqlwheredate}
	) as s2
) as usrshiftfinalduty where cnt = 1 ";


            itemSource = context.usrShiftDutys.FromSqlRaw(sql, mySqls.ToArray()).OrderByDescending(s => s.dtDate).ThenByDescending(s => s.sTime).ToList();
            Total = itemSource.Count;
            if (dutyReqModel.PageNum > 0 && dutyReqModel.PageSize > 0)
            {
                int startIndex = (dutyReqModel.PageNum - 1) * dutyReqModel.PageSize;
                int endIndex = startIndex + dutyReqModel.PageSize - 1;
                itemSource = itemSource.Skip(startIndex).Take(endIndex - startIndex + 1).ToList();
            }
        }

        ReturnModel<usrShiftDuty> remodel = new ReturnModel<usrShiftDuty>
        {
            PageNum = dutyReqModel.PageNum,
            PageSize = dutyReqModel.PageSize,
            Total = Total,
            List = itemSource
        };
        return remodel;
    }
}
