namespace VgAutoDrill.Admin.Common.Configuration
{
    public class AppSettingsConstHelper
    {
        #region 数据库================================================================================
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        private static readonly string DbSqlConnection = AppSettingsHelper.GetContent<string>("ConnectionStrings", "VgAutoDrillAuthDB");
        /// <summary>
        /// 获取数据库类型
        /// </summary>
        private static readonly string DbDbType = AppSettingsHelper.GetContent<string>("ConnectionStrings", "DbType");
        #endregion
    }
}
