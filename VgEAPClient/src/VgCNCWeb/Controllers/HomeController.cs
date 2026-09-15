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


namespace VgCNCWeb.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class HomeController : ControllerBase
{
    private readonly IDbContextFactory<VegaContext> _contextFactory;

    public HomeController(IDbContextFactory<VegaContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    /// <summary>
    /// 获取设备信息列表
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("GetSysdrillInformation")]
    public ReturnSysdrill GetSysdrillInformation()
    {
        List<sysDrillInformation> drillList = new List<sysDrillInformation>();
        using (var context = _contextFactory.CreateDbContext())
        {
            drillList = context.sysDrillInformations.OrderBy(s => s.sEquipmentID).ToList();
        }

        int WorkingCount = 0;
        int AlarmCount = 0;
        int StopCount = 0;
        int WaitingCount = 0;
        int IdleCount = 0;
        int OfflineCount = 0;

        foreach (var drill in drillList)
        {
            switch (drill.sWorkMode)
            {
                case "IDLE":
                    IdleCount++;
                    break;

                case "WAIT":
                    WaitingCount++;
                    break;

                case "WORK":
                    WorkingCount++;
                    break;

                case "STOP":
                    StopCount++;
                    break;

                case "ALAM":
                    AlarmCount++;
                    break;

                case "SERV":
                    StopCount++;
                    break;

                default:
                    OfflineCount++;
                    break;
            }
        }


        ReturnSysdrill remode = new ReturnSysdrill
        {
            WorkingCount = WorkingCount,
            AlarmCount = AlarmCount,
            StopCount = StopCount,
            WaitingCount = WaitingCount,
            IdleCount = IdleCount,
            OfflineCount = OfflineCount,
            drillList = drillList
        };
        return remode;
    }

    /// <summary>
    /// 获取指定设备明细
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("GetEquDrillInformation")]
    public sysDrillInformation GetEquDrillInformation([FromBody] EquReqModel equReqModel)
    {
        sysDrillInformation drill = new sysDrillInformation();
        string sql = string.Empty;
        List<MySqlParameter> mySqls = new List<MySqlParameter>();

        using (var context = _contextFactory.CreateDbContext())
        {
            sql = $@" SELECT * FROM sysDrillInformation WHERE sEquipmentID = @sEquipmentID ";
            mySqls.Add(new MySqlParameter("sEquipmentID", equReqModel.sEquipmentID));
            var equlist = context.sysDrillInformations.FromSqlRaw(sql, mySqls.ToArray()).ToList();
            if (equlist.Count > 0)
            {
                drill = equlist.First();
            }
        }

        return drill;
    }
}
