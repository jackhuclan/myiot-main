// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlSugar;
using VgAutoDrill.Fundation.Drill;

namespace VegaIot.External.Kinwong
{
    public class KinwongDrillWriteRunInformation : IDrillWriteRunInformation
    {
        private DrillFilePathOptions _drillFilePathOptions;
        private readonly ILogger<KinwongDrillWriteRunInformation> logger;

        public KinwongDrillWriteRunInformation(IOptions<DrillFilePathOptions> options, ILogger<KinwongDrillWriteRunInformation> logger)
        {
            _drillFilePathOptions = options.Value;
            this.logger = logger;
        }

        public void WriteRunInformation(string content)
        {
            try
            {
                logger.LogError($"KinwongDrillWriteRunInformation  WriteRunInformation  content {content}");
                var data = content.Split(',');
                if (data.Length != 5)
                {
                    logger.LogDebug($"KinwongDrillWriteRunInformation  WriteRunInformation content 内容不对");
                    return;
                }
                SqlSugarClient Db = new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = _drillFilePathOptions.ConnectionString,
                    DbType = DbType.MySql,
                    IsAutoCloseConnection = true
                });
                Db.Open();
                DateTime dt = DateTime.Now;
                UInt64 GroupNum = UInt64.Parse(dt.ToString("yyyyMMddHHmmssff"));//20230921002554941M
                Db.Insertable(new PC_DATA_CRAFT { CONTAINER = "", PNLIDORSETID = "", ITEMCODE = "CRAFT_1", ITEMDES = "本趟生产LOT", UOM = "", ITEMVALUE = data[0], ISOK = "", GROUPNUM = GroupNum, CREATETIME = dt }).ExecuteCommand();
                Db.Insertable(new PC_DATA_CRAFT { CONTAINER = "", PNLIDORSETID = "", ITEMCODE = "CRAFT_2", ITEMDES = "本趟实际钻孔数", UOM = "", ITEMVALUE = data[1], ISOK = "", GROUPNUM = GroupNum, CREATETIME = dt }).ExecuteCommand();
                Db.Insertable(new PC_DATA_CRAFT { CONTAINER = "", PNLIDORSETID = "", ITEMCODE = "CRAFT_3", ITEMDES = "本趟趟钻孔时间", UOM = "", ITEMVALUE = data[2], ISOK = "", GROUPNUM = GroupNum, CREATETIME = dt }).ExecuteCommand();
                Db.Insertable(new PC_DATA_CRAFT { CONTAINER = "", PNLIDORSETID = "", ITEMCODE = "CRAFT_4", ITEMDES = "本趟趟报警时间", UOM = "", ITEMVALUE = data[3], ISOK = "", GROUPNUM = GroupNum, CREATETIME = dt }).ExecuteCommand();
                Db.Insertable(new PC_DATA_CRAFT { CONTAINER = "", PNLIDORSETID = "", ITEMCODE = "CRAFT_5", ITEMDES = "生产趟数", UOM = "", ITEMVALUE = data[4], ISOK = "", GROUPNUM = GroupNum, CREATETIME = dt }).ExecuteCommand();
                Db.Close();
                logger.LogDebug($"KinwongDrillWriteRunInformation  WriteRunInformation 插入PC_DATA_CRAFT数据完毕");
            }
            catch (Exception ee)
            {
                logger.LogDebug($"KinwongDrillWriteRunInformation  WriteRunInformation 异常{ee.Message} ");
            }
        }
    }
}
