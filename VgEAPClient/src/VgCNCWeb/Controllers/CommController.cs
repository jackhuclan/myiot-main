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
public class CommController : ControllerBase
{
    private readonly IDbContextFactory<VegaContext> _contextFactory;

    public CommController(IDbContextFactory<VegaContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    /// <summary>
    /// 用户指令查询
    /// </summary>
    /// <param name="commReqModel"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("QueryCOMMList")]
    public ReturnModel<usrCOMM> QueryCOMMList([FromBody] CommReqModel commReqModel)
    {
        List<usrCOMM> itemSource = new List<usrCOMM>();
        string sql = string.Empty;
        List<MySqlParameter> mySqls = new List<MySqlParameter>();
        int Total = 0;
        string TableName = "usrCOMM";

        using (var context = _contextFactory.CreateDbContext())
        {
            sql = $@" SELECT * FROM  {TableName}  WHERE 1=1 ";

            if (!string.IsNullOrEmpty(commReqModel.sEquipmentID))
            {
                sql += $@" AND sEquipmentID LIKE @sEquipmentID ";
                mySqls.Add(new MySqlParameter("sEquipmentID", $@"%{commReqModel.sEquipmentID}%"));
            }

            DateOnly StartDate, EndDate;
            if (DateOnly.TryParse(commReqModel.sStartDate, out StartDate))
            {
                sql += $@" AND dtDate >= @sStartDate ";
                mySqls.Add(new MySqlParameter("sStartDate", StartDate));
            }
            if (DateOnly.TryParse(commReqModel.sEndDate, out EndDate))
            {
                sql += $@" AND dtDate <= @sEndDate ";
                mySqls.Add(new MySqlParameter("sEndDate", EndDate));
            }

            itemSource = context.usrCOMMs.FromSqlRaw(sql, mySqls.ToArray()).OrderByDescending(s => s.dtDate).ThenByDescending(s => s.sTime).ToList();
            Total = itemSource.Count;
            if (commReqModel.PageNum > 0 && commReqModel.PageSize > 0)
            {
                int startIndex = (commReqModel.PageNum - 1) * commReqModel.PageSize;
                int endIndex = startIndex + commReqModel.PageSize - 1;
                itemSource = itemSource.Skip(startIndex).Take(endIndex - startIndex + 1).ToList();
            }
        }

        ReturnModel<usrCOMM> remodel = new ReturnModel<usrCOMM>
        {
            PageNum = commReqModel.PageNum,
            PageSize = commReqModel.PageSize,
            Total = Total,
            List = itemSource
        };
        return remodel;
    }
}
