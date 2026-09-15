// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlSugar;
using VgAutoDrill.Fundation.Drill;

namespace VegaIot.External.Kinwong
{
    public class KinwongLoadFileResult : IDrillFileLoadResult
    {
        private DrillFilePathOptions _drillFilePathOptions;
        private readonly ILogger<KinwongLoadFileResult> logger;

        public KinwongLoadFileResult(IOptions<DrillFilePathOptions> options, ILogger<KinwongLoadFileResult> logger)
        {
            _drillFilePathOptions = options.Value;
            this.logger = logger;
        }

        public void WriteFileLoadResult(string itemCode)
        {
            try
            {
                logger.LogDebug($"KinwongLoadFileResult  WriteFileLoadResult ");
                SqlSugarClient Db = new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = _drillFilePathOptions.ConnectionString,
                    DbType = DbType.MySql,
                    IsAutoCloseConnection = true
                });
                Db.Open();
                logger.LogDebug($"KinwongLoadFileResult  WriteFileLoadResult 插入数据 设备通知EAP资料加载完毕 {itemCode} ");
                Db.Insertable(new PC_SIGNAL_STATUS() { CREATETIME = DateTime.Now, SIGNALCODE = "MATERIALOK", SIGNALDES = "设备通知EAP资料加载完毕", SIGNALVALUE = $"{itemCode}" }).ExecuteCommand();
                logger.LogDebug($"KinwongLoadFileResult  WriteFileLoadResult 插入数据 设备通知EAP资料加载完毕 {itemCode} 写数据库完成 ");
                Db.Close();
            }
            catch (Exception ee)
            {
                logger.LogDebug($"KinwongLoadFileResult  WriteFileLoadResult 异常 {ee.Message} ");
            }
        }
    }
}
