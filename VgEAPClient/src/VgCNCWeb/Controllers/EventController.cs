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
public class EventController : ControllerBase
{
    private readonly IDbContextFactory<VegaContext> _contextFactory;

    public EventController(IDbContextFactory<VegaContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    /// <summary>
    /// 事件日志查询
    /// </summary>
    /// <param name="eventReqModel"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("QueryEventList")]
    public ReturnModel<usrEvent> QueryEventList([FromBody] EventReqModel eventReqModel)
    {
        List<usrEvent> itemSource = new List<usrEvent>();
        string sql = string.Empty;
        List<MySqlParameter> mySqls = new List<MySqlParameter>();
        int Total = 0;
        string TableName = "usrEvent";

        using (var context = _contextFactory.CreateDbContext())
        {
            sql = $@" SELECT * FROM {TableName} WHERE 1=1 ";

            if (!string.IsNullOrEmpty(eventReqModel.sEquipmentID))
            {
                sql += $@" AND sEquipmentID LIKE @sEquipmentID ";
                mySqls.Add(new MySqlParameter("sEquipmentID", $@"%{eventReqModel.sEquipmentID}%"));
            }

            DateOnly StartDate, EndDate;
            if (DateOnly.TryParse(eventReqModel.sStartDate, out StartDate))
            {
                sql += $@" AND dtDate >= @sStartDate ";
                mySqls.Add(new MySqlParameter("sStartDate", StartDate));
            }
            if (DateOnly.TryParse(eventReqModel.sEndDate, out EndDate))
            {
                sql += $@" AND dtDate <= @sEndDate ";
                mySqls.Add(new MySqlParameter("sEndDate", EndDate));
            }

            if (!string.IsNullOrEmpty(eventReqModel.sEventID))
            {
                sql += $@" AND sEventID LIKE @sEventID ";
                mySqls.Add(new MySqlParameter("sEventID", $@"%{eventReqModel.sEventID}%"));
            }

            if (!string.IsNullOrEmpty(eventReqModel.sEventDesc))
            {
                sql += $@" AND sEventDesc LIKE @sEventDesc ";
                mySqls.Add(new MySqlParameter("sEventDesc", $@"%{eventReqModel.sEventDesc}%"));
            }

            itemSource = context.usrEvents.FromSqlRaw(sql, mySqls.ToArray()).OrderByDescending(s => s.dtDate).ThenByDescending(s => s.sTime).ToList();
            Total = itemSource.Count;
            if (eventReqModel.PageNum > 0 && eventReqModel.PageSize > 0)
            {
                int startIndex = (eventReqModel.PageNum - 1) * eventReqModel.PageSize;
                int endIndex = startIndex + eventReqModel.PageSize - 1;
                itemSource = itemSource.Skip(startIndex).Take(endIndex - startIndex + 1).ToList();
            }

        }

        ReturnModel<usrEvent> remodel = new ReturnModel<usrEvent>
        {
            PageNum = eventReqModel.PageNum,
            PageSize = eventReqModel.PageSize,
            Total = Total,
            List = itemSource
        };
        return remodel;
    }

    /// <summary>
    /// M52日志查询
    /// </summary>
    /// <param name="eventReqModel"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("QueryM54List")]
    public ReturnModel<usrM54Event> QueryM54List([FromBody] EventReqModel eventReqModel)
    {
        List<usrM54Event> itemSource = new List<usrM54Event>();
        string sql = string.Empty;
        List<MySqlParameter> mySqls = new List<MySqlParameter>();
        int Total = 0;
        string TableName = "usrM54Event";

        using (var context = _contextFactory.CreateDbContext())
        {
            sql = $@" SELECT * FROM {TableName} WHERE 1=1 ";

            if (!string.IsNullOrEmpty(eventReqModel.sEquipmentID))
            {
                sql += $@" AND sEquipmentID LIKE @sEquipmentID ";
                mySqls.Add(new MySqlParameter("sEquipmentID", $@"%{eventReqModel.sEquipmentID}%"));
            }

            DateOnly StartDate, EndDate;
            if (DateOnly.TryParse(eventReqModel.sStartDate, out StartDate))
            {
                sql += $@" AND dtDate >= @sStartDate ";
                mySqls.Add(new MySqlParameter("sStartDate", StartDate));
            }
            if (DateOnly.TryParse(eventReqModel.sEndDate, out EndDate))
            {
                sql += $@" AND dtDate <= @sEndDate ";
                mySqls.Add(new MySqlParameter("sEndDate", EndDate));
            }

            if (!string.IsNullOrEmpty(eventReqModel.sEventID))
            {
                sql += $@" AND sEventID LIKE @sEventID ";
                mySqls.Add(new MySqlParameter("sEventID", $@"%{eventReqModel.sEventID}%"));
            }

            if (!string.IsNullOrEmpty(eventReqModel.sEventDesc))
            {
                sql += $@" AND sEventDesc LIKE @sEventDesc ";
                mySqls.Add(new MySqlParameter("sEventDesc", $@"%{eventReqModel.sEventDesc}%"));
            }

            itemSource = context.usrM54Events.FromSqlRaw(sql, mySqls.ToArray()).OrderByDescending(s => s.dtDate).ThenByDescending(s => s.sTime).ToList();
            Total = itemSource.Count;
            if (eventReqModel.PageNum > 0 && eventReqModel.PageSize > 0)
            {
                int startIndex = (eventReqModel.PageNum - 1) * eventReqModel.PageSize;
                int endIndex = startIndex + eventReqModel.PageSize - 1;
                itemSource = itemSource.Skip(startIndex).Take(endIndex - startIndex + 1).ToList();
            }
        }

        ReturnModel<usrM54Event> remodel = new ReturnModel<usrM54Event>
        {
            PageNum = eventReqModel.PageNum,
            PageSize = eventReqModel.PageSize,
            Total = Total,
            List = itemSource
        };
        return remodel;
    }
}
