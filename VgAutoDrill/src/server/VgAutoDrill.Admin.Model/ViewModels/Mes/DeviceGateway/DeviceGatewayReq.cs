namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceGateway
{
    public class DeviceGatewayReq : Page
    {
        /// <summary>
        /// id
        /// </summary>
        public virtual long? Id { get; set; }
        /// <summary>
        /// code
        /// </summary>
        public virtual string? Code { get; set; }
        /// <summary>
        /// name
        /// </summary>
        public virtual string? Name { get; set; }

        /// <summary>
        /// c_version当前版本
        /// </summary>
        public virtual string? CVersion { get; set; }

        /// <summary>
        /// 可用版本
        /// </summary>
        public virtual string? AVersion { get; set; }

        /// <summary>
        /// 访问网址
        /// </summary>
        public virtual string? VisitWebsite { get; set; }

        /// <summary>
        /// 安装时间
        /// </summary>
        public virtual DateTime SetupTime { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        public virtual int? IsOnline { get; set; }

        /// <summary>
        /// 参数
        /// </summary>

        public virtual string? parameters { get; set; }

        /// <summary>
        /// 应用名
        /// </summary>
        public virtual string? AppName { get; set; }
    }
}
