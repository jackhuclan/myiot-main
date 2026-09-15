namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceAndRoute
{
    public class DeviceAndRouteInfo : BaseDto
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public virtual long? DeviceId { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public virtual string? DeviceName { get; set; }

        /// <summary>
        /// 设备类型ID
        /// </summary>
        public virtual int? DeviceTypeId { get; set; }

        /// <summary>
        /// 设备类型Code
        /// </summary>
        public virtual string? DeviceTypeCode { get; set; }

        /// <summary>
        /// 工艺路线名称组合
        /// </summary>
        public virtual string? RouteNameDescription { get; set; }
    }
}
