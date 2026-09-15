using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlSugar;
using VgAutoDrill.Fundation.Drill;

namespace VegaIot.External.Kinwong
{
    public class KinwongInsertQRCodeToDB : IDrillLoadPanelSNToDB
    {
        private DrillFilePathOptions _drillFilePathOptions;
        private readonly ILogger<KinwongInsertQRCodeToDB> logger;

        public KinwongInsertQRCodeToDB(IOptions<DrillFilePathOptions> options, ILogger<KinwongInsertQRCodeToDB> logger)
        {
            _drillFilePathOptions = options.Value;
            this.logger = logger;
        }
        public void LoadPanelSNToDB(string snInfo)
        {
            logger.LogWarning($"KinwongInsertQRCodeToDB  参数：{snInfo} ");
            try
            {
                // $"spindleNum:{spindleNum},Lot:{codeReaderContent},SN:{codeReaderContent},deviceId:{InteractingDevice.DeviceId}"
                string spindleNum = snInfo.Split(',')[0].Split(':')[1].ToString();
                string pancelSN = snInfo.Split(',')[2].Split(':')[1];
                string container = snInfo.Split(',')[1].Split(':')[1];
                string deviceId = snInfo.Split(',')[3].Split(':')[1];
                

                SqlSugarClient Db = new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = _drillFilePathOptions.ConnectionString,
                    DbType = DbType.MySql,
                    IsAutoCloseConnection = true
                });
                Db.Open();
                logger.LogWarning($"KinwongInsertQRCodeToDB  LoadPanelSNToDB 二维码{pancelSN}开始写入DB ");
                var affectedRows = Db.Insertable(new PC_DATA_SCAN() { CREATETIME = DateTime.Now, CONTAINER = $"{container}", PARENTID = $"FOLD", DRCTYPE = $"{spindleNum}", PNLIDORSETID = $"{pancelSN}", CREATEUSER = $"{deviceId}" }).ExecuteCommand();
                if (affectedRows > 0)
                {
                    logger.LogWarning($"PC_DATA_SCAN 写入数据OK:{snInfo}");
                }
                else
                {
                    logger.LogError("PC_DATA_SCAN  写入数据Fail");
                    throw new Exception("PC_DATA_SCAN _ERROR");
                }

                Db.Close();

            }
            catch (Exception ee)
            {
                logger.LogError($"KinwongInsertQRCodeToDB  LoadPanelSNToDB 异常 {ee.Message} ");
            }
        }
    }
}
