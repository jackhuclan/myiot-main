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
public class BrokenController : ControllerBase
{
    private readonly IDbContextFactory<VegaContext> _contextFactory;

    public BrokenController(IDbContextFactory<VegaContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }


    /// <summary>
    /// 实时断刀查询
    /// </summary>
    /// <param name="brokenReqModel"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("QueryBrokenList")]
    public ReturnModel<usrToolsBroken> QueryBrokenList([FromBody] BrokenReqModel brokenReqModel)
    {
        List<usrToolsBroken> itemSource = new List<usrToolsBroken>();
        string sql = string.Empty;
        List<MySqlParameter> mySqls = new List<MySqlParameter>();
        int Total = 0;
        string TableName = "usrtoolsbroken";

        using (var context = _contextFactory.CreateDbContext())
        {
            sql = $@" SELECT * FROM {TableName} WHERE 1=1 ";

            if (!string.IsNullOrEmpty(brokenReqModel.sEquipmentID))
            {
                sql += $@" AND sEquipmentID LIKE @sEquipmentID ";
                mySqls.Add(new MySqlParameter("sEquipmentID", $@"%{brokenReqModel.sEquipmentID}%"));
            }

            if (!string.IsNullOrEmpty(brokenReqModel.sActProgram))
            {
                sql += $@" AND sActProgram LIKE @sActProgram ";
                mySqls.Add(new MySqlParameter("sActProgram", $@"%{brokenReqModel.sActProgram}%"));
            }

            DateOnly StartDate, EndDate;
            if (DateOnly.TryParse(brokenReqModel.sStartDate, out StartDate))
            {
                sql += $@" AND dtDate >= @StartDate ";
                mySqls.Add(new MySqlParameter("StartDate", StartDate));
            }
            if (DateOnly.TryParse(brokenReqModel.sEndDate, out EndDate))
            {
                sql += $@" AND dtDate <= @EndDate ";
                mySqls.Add(new MySqlParameter("EndDate", EndDate));
            }

            itemSource = context.usrToolsBrokens.FromSqlRaw(sql, mySqls.ToArray()).OrderByDescending(s => s.dtDate).ThenByDescending(s => s.sTime).ToList();
            Total = itemSource.Count;
            if (brokenReqModel.PageNum > 0 && brokenReqModel.PageSize > 0)
            {
                int startIndex = (brokenReqModel.PageNum - 1) * brokenReqModel.PageSize;
                int endIndex = startIndex + brokenReqModel.PageSize - 1;
                itemSource = itemSource.Skip(startIndex).Take(endIndex - startIndex + 1).ToList();
            }
        }

        ReturnModel<usrToolsBroken> remodel = new ReturnModel<usrToolsBroken>
        {
            PageNum = brokenReqModel.PageNum,
            PageSize = brokenReqModel.PageSize,
            Total = Total,
            List = itemSource
        };
        return remodel;
    }

    /// <summary>
    /// 断刀信息查询
    /// </summary>
    /// <param name="brokenReqModel"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("QueryBrokenEndList")]
    public ReturnModel<usrToolsBrokenEnd> QueryBrokenEndList([FromBody] BrokenReqModel brokenReqModel)
    {
        List<usrToolsBrokenEnd> itemSource = new List<usrToolsBrokenEnd>();
        string sql = string.Empty;
        List<MySqlParameter> mySqls = new List<MySqlParameter>();
        int Total = 0;
        string TableName = "usrToolsBrokenEnd";

        using (var context = _contextFactory.CreateDbContext())
        {
            sql = $@" SELECT * FROM {TableName} WHERE 1=1 ";

            if (!string.IsNullOrEmpty(brokenReqModel.sEquipmentID))
            {
                sql += $@" AND sEquipmentID LIKE @sEquipmentID ";
                mySqls.Add(new MySqlParameter("sEquipmentID", $@"%{brokenReqModel.sEquipmentID}%"));
            }

            if (!string.IsNullOrEmpty(brokenReqModel.sActProgram))
            {
                sql += $@" AND sActProgram LIKE @sActProgram ";
                mySqls.Add(new MySqlParameter("sActProgram", $@"%{brokenReqModel.sActProgram}%"));
            }

            DateOnly StartDate, EndDate;
            if (DateOnly.TryParse(brokenReqModel.sStartDate, out StartDate))
            {
                sql += $@" AND dtDate >= @StartDate ";
                mySqls.Add(new MySqlParameter("StartDate", StartDate));
            }
            if (DateOnly.TryParse(brokenReqModel.sEndDate, out EndDate))
            {
                sql += $@" AND dtDate <= @EndDate ";
                mySqls.Add(new MySqlParameter("EndDate", EndDate));
            }

            if (brokenReqModel.PageNum > 0 && brokenReqModel.PageSize > 0)
            {
                sql += $@" order by dtDate desc,sTime desc LIMIT @PageIndex , @PageSize ";
                mySqls.Add(new MySqlParameter("PageIndex", (brokenReqModel.PageNum - 1) * brokenReqModel.PageSize));
                mySqls.Add(new MySqlParameter("PageSize", brokenReqModel.PageSize));
            }

            itemSource = context.usrToolsBrokenEnds.FromSqlRaw(sql, mySqls.ToArray()).OrderByDescending(s => s.dtDate).ThenByDescending(s => s.sTime).ToList();
            Total = itemSource.Count;
            if (brokenReqModel.PageNum > 0 && brokenReqModel.PageSize > 0)
            {
                int startIndex = (brokenReqModel.PageNum - 1) * brokenReqModel.PageSize;
                int endIndex = startIndex + brokenReqModel.PageSize - 1;
                itemSource = itemSource.Skip(startIndex).Take(endIndex - startIndex + 1).ToList();
            }
        }

        ReturnModel<usrToolsBrokenEnd> remodel = new ReturnModel<usrToolsBrokenEnd>
        {
            PageNum = brokenReqModel.PageNum,
            PageSize = brokenReqModel.PageSize,
            Total = Total,
            List = itemSource
        };
        return remodel;
    }
}
