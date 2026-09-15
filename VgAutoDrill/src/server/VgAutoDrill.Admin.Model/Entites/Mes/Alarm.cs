using SqlSugar;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 告警记录
    ///</summary>
    [SugarTable("t_alarm")]
    public class Alarm : BaseEntity
    {
        /// <summary>
        /// 告警时间
        /// </summary>
        [SugarColumn(ColumnName = "alarm_time")]

        public virtual DateTime? AlarmTime { get; set; }

        /// <summary>
        /// 告警编号
        /// </summary>
        [SugarColumn(ColumnName = "alarm_code")]

        public virtual string? AlarmCode { get; set; }

        /// <summary>
        /// 告警名称
        /// </summary>
        [SugarColumn(ColumnName = "alarm_name")]

        public virtual string? AlarmName { get; set; }

        /// <summary>
        /// 告警级别（1普通、2严重、3紧急）
        /// </summary>
        [SugarColumn(ColumnName = "alarm_level")]

        public int? AlarmLevel { get; set; }

        /// <summary>
        /// 告警设备ID
        /// </summary>
        [SugarColumn(ColumnName = "device_id")]
        public int? DeviceId { get; set; }

        /// <summary>
        /// 事件ID
        /// </summary>
        [SugarColumn(ColumnName = "event_id")]
        public int? EventId { get; set; }

        /// <summary>
        /// 事件数据
        /// </summary>
        [SugarColumn(ColumnName = "event_data")]

        public virtual string? EventData { get; set; }
        /// <summary>
        /// 处理时间 
        ///</summary>
        [SugarColumn(ColumnName = "handle_time")]
        public DateTime? HandledTime { get; set; }
        /// <summary>
        /// 是否已处理
        ///</summary>
        [SugarColumn(ColumnName = "is_handled")]
        public bool? IsHandled { get; set; }

        [SugarColumn(ColumnName = "sync_id")]
        public long? SyncId { get; set; }

        /// <summary>
        /// 告警分类
        /// </summary>
        [SugarColumn(ColumnName = "alarm_kind")]
        public virtual AlarmKind? AlarmKind { get; set; }

        /// <summary>
        /// 告警内容
        /// </summary>
        [SugarColumn(ColumnName = "alarm_content")]
        public virtual string? AlarmContent { get; set; }

        [SugarColumn(ColumnName = "token")]
        public virtual string? Token { get; set; }

        /// <summary>
        /// 库位编码
        /// </summary>
        [SugarColumn(ColumnName = "location_code")]
        public virtual string? LocationCode { get; set; }

        /// <summary>
        /// 分区编码
        /// </summary>
        [SugarColumn(ColumnName = "partition_code")]
        public virtual string? PartitionCode { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        [SugarColumn(ColumnName = "item_code")]
        public virtual string? ItemCode { get; set; }
    }
}
