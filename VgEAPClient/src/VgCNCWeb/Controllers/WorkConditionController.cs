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
public class WorkConditionController : ControllerBase
{
    private readonly IDbContextFactory<VegaContext> _contextFactory;

    public WorkConditionController(IDbContextFactory<VegaContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    /// <summary>
    /// 工况查询
    /// </summary>
    /// <param name="sStartDate"></param>
    /// <param name="sEndDate"></param>
    /// <param name="sEquipmentID"></param>
    /// <param name="sActProgram"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("QueryWorkConditionList")]
    public ReturnModel<usrWorkingCondition> QueryWorkConditionList([FromBody] WorkingConditionReqModel workingConditionReqModel)
    {
        List<usrWorkingCondition> itemSource = new List<usrWorkingCondition>();
        string sql = string.Empty;
        List<MySqlParameter> mySqls = new List<MySqlParameter>();
        int Total = 0;
        string TableName = "usrWorkingCondition";

        using (var context = _contextFactory.CreateDbContext())
        {
            sql = $@" SELECT * FROM {TableName} WHERE 1=1 ";

            if (!string.IsNullOrEmpty(workingConditionReqModel.sEquipmentID))
            {
                sql += $@" AND sEquipmentID LIKE @sEquipmentID ";
                mySqls.Add(new MySqlParameter("sEquipmentID", $@"%{workingConditionReqModel.sEquipmentID}%"));
            }

            DateOnly StartDate, EndDate;
            if (DateOnly.TryParse(workingConditionReqModel.sStartDate, out StartDate))
            {
                sql += $@" AND dtDate >= @sStartDate ";
                mySqls.Add(new MySqlParameter("sStartDate", StartDate));
            }
            if (DateOnly.TryParse(workingConditionReqModel.sEndDate, out EndDate))
            {
                sql += $@" AND dtDate <= @sEndDate ";
                mySqls.Add(new MySqlParameter("sEndDate", EndDate));
            }

            if (!string.IsNullOrEmpty(workingConditionReqModel.sActProgram))
            {
                sql += $@" AND sActProgram LIKE @sActProgram ";
                mySqls.Add(new MySqlParameter("sActProgram", $@"%{workingConditionReqModel.sActProgram}%"));
            }

            itemSource = context.usrWorkingConditions.FromSqlRaw(sql, mySqls.ToArray()).OrderByDescending(s => s.dtDate).ThenByDescending(s => s.sTime).ToList();
            Total = itemSource.Count;
            if (workingConditionReqModel.PageNum > 0 && workingConditionReqModel.PageSize > 0)
            {
                int startIndex = (workingConditionReqModel.PageNum - 1) * workingConditionReqModel.PageSize;
                int endIndex = startIndex + workingConditionReqModel.PageSize - 1;
                itemSource = itemSource.Skip(startIndex).Take(endIndex - startIndex + 1).ToList();
            }

        }

        ReturnModel<usrWorkingCondition> remodel = new ReturnModel<usrWorkingCondition>
        {
            PageNum = workingConditionReqModel.PageNum,
            PageSize = workingConditionReqModel.PageSize,
            Total = Total,
            List = itemSource
        };
        return remodel;
    }
}
