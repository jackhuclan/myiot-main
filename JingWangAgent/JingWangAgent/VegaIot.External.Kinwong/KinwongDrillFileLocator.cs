// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlSugar;
using VgAutoDrill.Fundation.Drill;

namespace VegaIot.External.Kinwong;

public class KinwongDrillFileLocator : IDrillFilePathLocator
{
    private DrillFilePathOptions _drillFilePathOptions;
    private KinwongDatabaseItemcode _kinwongDatabaseItemcodeOptions;
    private readonly ILogger<KinwongDrillFileLocator> logger;

    public KinwongDrillFileLocator(IOptions<DrillFilePathOptions> options, IOptions<KinwongDatabaseItemcode> kinwongDatabaseItemcode, ILogger<KinwongDrillFileLocator> logger)
    {
        _drillFilePathOptions = options.Value;
        _kinwongDatabaseItemcodeOptions = kinwongDatabaseItemcode.Value;
        this.logger = logger;
    }

    /// <summary>
    /// 根据_drillFilePathOptions中的ConnectionString连接数据库，
    /// 然后根据itemCode查找到钻机参数的path
    /// </summary>
    /// <param name="itemCode"></param>
    /// <returns></returns>
    public DrillInfo GetFilePath(string itemCode)
    {
        logger.LogError($"KinwongDrillFileLocator  GetFilePath start  读超时时间 {_drillFilePathOptions.ReadTimeoutSeconds} dia前缀{_drillFilePathOptions.PrefixDiaPath} dia后缀{_drillFilePathOptions.DiaFileExtension}");
        SqlSugarClient Db = new SqlSugarClient(new ConnectionConfig()
        {
            ConnectionString = _drillFilePathOptions.ConnectionString,
            DbType = DbType.MySql,
            IsAutoCloseConnection = true
        });
        Db.Open();
        List<EAP_MATERIAL_DATA> oldMaterial = Db.Queryable<EAP_MATERIAL_DATA>().ToList();
        logger.LogInformation($" 触发任务下发之前 :EAP_MATERIAL_DATA  {JsonSerializer.Serialize(oldMaterial)}");
        Db.Deleteable<EAP_MATERIAL_DATA>().ExecuteCommand();
        logger.LogError($"触发任务下发: {itemCode}");
        var affectedRows = Db.Insertable(new PC_SIGNAL_STATUS() { CREATETIME = DateTime.Now, SIGNALCODE = _kinwongDatabaseItemcodeOptions.TriggerItemcode, SIGNALDES = _kinwongDatabaseItemcodeOptions.TriggerDes, SIGNALVALUE = $"{itemCode}" }).ExecuteCommand();
        if (affectedRows > 0)
        {
            logger.LogError($"PC_SIGNAL_STATUS 写入数据OK:SIGNALCODE = {_kinwongDatabaseItemcodeOptions.TriggerItemcode},SIGNALDES = {_kinwongDatabaseItemcodeOptions.TriggerDes}, SIGNALVALUE ={itemCode}");
        }
        else
        {
            logger.LogError("PC_SIGNAL_STATUS 写入数据Fail");
            throw new Exception("WRITE_PC_SIGNAL_STATUS_ERROR");
        }
        Stopwatch stopwatch = Stopwatch.StartNew();
        List<EAP_MATERIAL_DATA> material = null;
        var existsContent = false;
        do
        {
            Thread.Sleep(50);
            material = Db.Queryable<EAP_MATERIAL_DATA>().ToList();

            bool existsDrill = material.Any(x =>
                                      x.ITEMCODE != null
                                      && x.ITEMCODE.Contains(_kinwongDatabaseItemcodeOptions.DrillItemcode, StringComparison.OrdinalIgnoreCase));
            var existsDia = material.Any(x =>
                                      x.ITEMCODE != null
                                      && x.ITEMCODE.Contains(_kinwongDatabaseItemcodeOptions.DiaItemcode, StringComparison.OrdinalIgnoreCase));
            logger.LogError($"触发任务下发之后 EAP_MATERIAL_DATA  existsDrill:{existsDrill} existsDia:{existsDia}");
            existsContent = existsDrill && existsDia;
            if (existsContent)
            {
                logger.LogError($"等待下发用时{stopwatch.ElapsedMilliseconds} ms");
                stopwatch.Stop();
                break;
            }
        } while (stopwatch.ElapsedMilliseconds < _drillFilePathOptions.ReadTimeoutSeconds * 1000);

        if (!existsContent)
        {
            logger.LogError("EAP_MATERIAL_DATA 中没数据");
            throw new Exception("EAP_MATERIAL_DATA_HAS_NO_DATA");
        }

        logger.LogError($"触发任务下发之后 EAP_MATERIAL_DATA  {JsonSerializer.Serialize(material)}");
        Db.Deleteable<EAP_MATERIAL_DATA>().ExecuteCommand();
        Db.Close();
        var drllPath = material.FirstOrDefault(s => _kinwongDatabaseItemcodeOptions.DrillItemcode.Equals(s.ITEMCODE, StringComparison.OrdinalIgnoreCase))?.ITEMVALUE;
        if (string.IsNullOrEmpty(drllPath))
        {
            logger.LogError("EAP_MATERIAL_DATA 钻机参数");
            throw new Exception("EAP_MATERIAL_DATA_HAS_NO_DRLL_PATH");
        }
        var diaPath = material.FirstOrDefault(s => _kinwongDatabaseItemcodeOptions.DiaItemcode.Equals(s.ITEMCODE, StringComparison.OrdinalIgnoreCase))?.ITEMVALUE;
        if (!string.IsNullOrEmpty(diaPath))
        {
            var subDiaPath = Path.Combine(_drillFilePathOptions.PrefixDiaPath, diaPath);
            diaPath = subDiaPath + _drillFilePathOptions.DiaFileExtension;
            logger.LogError($"EAP_MATERIAL_DATA 整合后的数据是 {diaPath}");
        }
        logger.LogError($"KinwongDrillFileLocator  GetFilePath end");
        return new DrillInfo() { DrlPath = drllPath, DiaPath = diaPath };
    }
}
