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
public class AlarmController : ControllerBase
{
    private readonly IDbContextFactory<VegaContext> _contextFactory;

    public AlarmController(IDbContextFactory<VegaContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    /// <summary>
    /// 报警信息查询
    /// </summary>
    /// <param name="alarmReqModel"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("QueryAlarmList")]
    public ReturnModel<usrAlarm> QueryAlarmList([FromBody] AlarmReqModel alarmReqModel)
    {
        List<usrAlarm> itemSource = new List<usrAlarm>();
        string sql = string.Empty;
        List<MySqlParameter> mySqls = new List<MySqlParameter>();
        int Total = 0;
        string TableName = "usrAlarm";

        using (var context = _contextFactory.CreateDbContext())
        {
            sql = $@" SELECT * FROM {TableName} WHERE 1=1 ";

            if (!string.IsNullOrEmpty(alarmReqModel.sEquipmentID))
            {
                sql += $@" AND sEquipmentID LIKE @sEquipmentID ";
                mySqls.Add(new MySqlParameter("sEquipmentID", $@"%{alarmReqModel.sEquipmentID}%"));
            }

            DateOnly StartDate, EndDate;
            if (DateOnly.TryParse(alarmReqModel.sStartDate, out StartDate))
            {
                sql += $@" AND dtDate >= @StartDate ";
                mySqls.Add(new MySqlParameter("StartDate", StartDate));
            }
            if (DateOnly.TryParse(alarmReqModel.sEndDate, out EndDate))
            {
                sql += $@" AND dtDate <= @EndDate ";
                mySqls.Add(new MySqlParameter("EndDate", EndDate));
            }

            if (!string.IsNullOrEmpty(alarmReqModel.sAlarmID))
            {
                sql += $@" AND sAlarmID LIKE @sAlarmID ";
                mySqls.Add(new MySqlParameter("sAlarmID", $@"%{alarmReqModel.sAlarmID}%"));
            }

            if (!string.IsNullOrEmpty(alarmReqModel.sAlarmDesc))
            {
                sql += $@" AND sAlarmDesc LIKE @sAlarmDesc ";
                mySqls.Add(new MySqlParameter("sAlarmDesc", $@"%{alarmReqModel.sAlarmDesc}%"));
            }

            itemSource = context.usrAlarms.FromSqlRaw(sql, mySqls.ToArray()).OrderByDescending(s => s.dtDate).ThenByDescending(s => s.sStartTime).ToList();
            Total = itemSource.Count;
            if (alarmReqModel.PageNum > 0 && alarmReqModel.PageSize > 0)
            {
                int startIndex = (alarmReqModel.PageNum - 1) * alarmReqModel.PageSize;
                int endIndex = startIndex + alarmReqModel.PageSize - 1;
                itemSource = itemSource.Skip(startIndex).Take(endIndex - startIndex + 1).ToList();
            }

        }

        ReturnModel<usrAlarm> remodel = new ReturnModel<usrAlarm>
        {
            PageNum = alarmReqModel.PageNum,
            PageSize = alarmReqModel.PageSize,
            Total = Total,
            List = itemSource
        };

        return remodel;
    }
}

