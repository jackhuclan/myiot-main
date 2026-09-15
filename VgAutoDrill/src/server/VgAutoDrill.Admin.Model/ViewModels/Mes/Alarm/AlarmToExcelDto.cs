using Npoi.Mapper.Attributes;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DvAlarmRecord
{
    public class AlarmToExcelDto
    {
        [Column("告警设备编码")]
        public string? DeviceCode { get; set; }

        [Column("告警设备名称")]
        public string? DeviceName { get; set; }

        [Column("告警时间")]
        public DateTime? AlarmTime { get; set; }

        [Column("告警编号")]
        public string? AlarmCode { get; set; }

        [Column("告警描述")]
        public string? AlarmName { get; set; }

        [Column("告警级别")]
        public int? AlarmLevel { get; set; }

        [Column("事件ID")]
        public int? EventId { get; set; }

        [Column("事件数据")]
        public string? EventData { get; set; }
        [Column("处理时间")]
        public DateTime? HandledTime { get; set; }
        [Column("是否已处理")]
        public bool? IsHandled { get; set; }
        [Column("同步ID")]
        public long? SyncId { get; set; }

        /// <summary>
        /// 库位编码
        /// </summary>
        [Column("库位编码")]
        public virtual string? LocationCode { get; set; }

        /// <summary>
        /// 分区编码
        /// </summary>
        [Column("分区编码")]
        public virtual string? PartitionCode { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        [Column("产品编号")]
        public virtual string? ItemCode { get; set; }
    }
}
