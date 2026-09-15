using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 设备关联工艺路线表
    ///</summary>
    [SugarTable("t_device_and_route")]
    public class DeviceAndRoute : BaseEntity
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        [SugarColumn(ColumnName = "device_id")]
        public virtual long? DeviceId { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        [SugarColumn(ColumnName = "device_code")]
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [SugarColumn(ColumnName = "device_name")]
        public virtual string? DeviceName { get; set; }

        [SugarColumn(ColumnName = "device_type_id")]
        /// <summary>
        /// 设备类型ID
        /// </summary>
        public virtual int? DeviceTypeId { get; set; }

        [SugarColumn(ColumnName = "device_type_code")]
        /// <summary>
        /// 设备类型Code
        /// </summary>
        public virtual string? DeviceTypeCode { get; set; }

        /// <summary>
        /// 工艺路线ID
        /// </summary>
        [SugarColumn(ColumnName = "route_id")]
        public virtual long? RouteId { get; set; }

        /// <summary>
        /// 工艺路线编号
        /// </summary>
        [SugarColumn(ColumnName = "route_code")]
        public virtual string? RouteCode { get; set; }
        /// <summary>
        /// 工艺路线名称
        /// </summary>
        [SugarColumn(ColumnName = "route_name")]
        public virtual string? RouteName { get; set; }
    }
}
