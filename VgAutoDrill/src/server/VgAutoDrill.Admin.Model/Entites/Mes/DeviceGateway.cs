using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    [SugarTable("t_device_gateway")]
    public class DeviceGateway : BaseEntity
    {
        /// <summary>
        /// name
        /// </summary>
        [SugarColumn(ColumnName = "name")]
        public string? Name { get; set; }

        /// <summary>
        /// name
        /// </summary>
        [SugarColumn(ColumnName = "code")]
        public string? Code { get; set; }

        /// <summary>
        /// 当前版本
        /// </summary>
        [SugarColumn(ColumnName = "c_version")]
        public string? CVersion { get; set; }

        /// <summary>
        /// 可用版本
        /// </summary>
        [SugarColumn(ColumnName = "a_version")]
        public string? AVersion { get; set; }

        /// <summary>
        /// 访问网址
        /// </summary>
        [SugarColumn(ColumnName = "visit_website")]
        public string? VisitWebsite { get; set; }

        /// <summary>
        /// 安装时间
        /// </summary>
        [SugarColumn(ColumnName = "setup_time")]
        public DateTime SetupTime { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        [SugarColumn(ColumnName = "is_online")]
        public virtual int? IsOnline { get; set; }

        /// <summary>
        /// 安装位置
        /// </summary>
        [SugarColumn(ColumnName = "installed_location")]
        public virtual string? InstalledLocation { get; set; }

        /// <summary>
        /// 服务名
        /// </summary>
        [SugarColumn(ColumnName = "service_name")]
        public virtual string? ServiceName { get; set; }


        /// <summary>
        /// 参数
        /// </summary>
        [SugarColumn(ColumnName = "parameters")]
        public virtual string? parameters { get; set; }

        /// <summary>
        /// 应用名
        /// </summary>
        [SugarColumn(ColumnName = "app_name")]
        public virtual string? AppName { get; set; }
    }
}
