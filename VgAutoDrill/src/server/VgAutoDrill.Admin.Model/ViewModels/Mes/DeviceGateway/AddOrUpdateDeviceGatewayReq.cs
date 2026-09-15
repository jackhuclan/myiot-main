namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceGateway
{
    public class AddOrUpdateDeviceGatewayReq : BaseAddOrUpdateWithTreeDto
    {

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
        public virtual DateTime? SetupTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>

        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual int? CreatorId { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        public virtual int? IsOnline { get; set; }

        /// <summary>
        /// 安装位置
        /// </summary>
        public virtual string? InstalledLocation { get; set; }

        /// <summary>
        /// 服务名
        /// </summary>

        public virtual string? ServiceName { get; set; }

        /// <summary>
        /// 参数
        /// </summary>

        public virtual string? parameters { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 更新的人
        /// </summary>
        public virtual int? ModifyId { get; set; }

        /// <summary>
        /// 应用名
        /// </summary>
        public virtual string? AppName { get; set; }
    }
}
