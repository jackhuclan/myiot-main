using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 设备事件定义
    ///</summary>
    [SugarTable("t_event_define")]
    public class EventDefine : BaseEntity
    {
        /// <summary>
        /// 事件ID
        /// </summary>
        [SugarColumn(ColumnName = "event_id")]
        public virtual string? EventId { get; set; }
        /// <summary>
        /// 事件名称
        /// </summary>
        [SugarColumn(ColumnName = "event_name")]
        public virtual string? EventName { get; set; }

        /// <summary>
        /// 参数配置
        /// </summary>
        [SugarColumn(ColumnName = "parameter_json")]
        public virtual string? ParameterJson { get; set; }

        /// <summary>
        /// 级别（1普通、2严重、3紧急）
        /// </summary>
        [SugarColumn(ColumnName = "event_level")]

        public int? EventLevel { get; set; }

        [SugarColumn(ColumnName = "device_type_id")]
        public int? DeviceTypeId { get; set; }
    }
}
