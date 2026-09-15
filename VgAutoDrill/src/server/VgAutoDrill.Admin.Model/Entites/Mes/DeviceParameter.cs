using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 设备参数
    ///</summary>
    [SugarTable("t_device_parameter")]
    public class DeviceParameter : BaseEntity
    {
        [SugarColumn(ColumnName = "device_type_id")]
        public virtual int DeviceTypeId { get; set; }

        [SugarColumn(ColumnName = "device_id")]
        public virtual int DeviceId { get; set; }

        [SugarColumn(ColumnName = "code")]
        /// <summary>
        /// 编号
        /// </summary>
        public virtual string? Code { get; set; }

        [SugarColumn(ColumnName = "name")]
        /// <summary>
        /// 
        /// </summary>
        public virtual string? Name { get; set; }

        [SugarColumn(ColumnName = "parameters")]
        /// <summary>
        /// 参数
        /// json 字符串
        /// </summary>
        public virtual string? Parameters { get; set; }
    }
}
