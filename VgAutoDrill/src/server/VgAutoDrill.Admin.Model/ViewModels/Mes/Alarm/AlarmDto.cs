using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DvAlarmRecord
{
    public class AlarmDto : BaseDto
    {
        /// <summary>
        /// 告警时间
        /// </summary>

        public virtual DateTime? AlarmTime { get; set; }

        /// <summary>
        /// 告警编号
        /// </summary>

        public virtual string? AlarmCode { get; set; }
        /// <summary>
        /// 告警描述
        /// </summary>

        public virtual string? AlarmName { get; set; }

        /// <summary>
        /// 告警级别（1普通、2严重、3紧急）
        /// </summary>

        public int? AlarmLevel { get; set; }

        /// <summary>
        /// 告警设备ID
        /// </summary>
        public int? DeviceId { get; set; }
        /// <summary>
        /// 事件ID
        /// </summary>
        public int? EventId { get; set; }

        /// <summary>
        /// 事件数据
        /// </summary>
        public virtual string? EventData { get; set; }

        public DateTime? HandledTime { get; set; }
        public bool? IsHandled { get; set; }
        public long? SyncId { get; set; }

        /// <summary>
        /// 告警分类
        /// </summary>
        public virtual AlarmKind? AlarmKind { get; set; }

        /// <summary>
        /// 告警分类描述
        /// </summary>
        public virtual string? AlarmKindDesc { get; set; }

        /// <summary>
        /// 告警内容
        /// </summary>
        public virtual string? AlarmContent { get; set; }

        public virtual string? Token { get; set; }

        /// <summary>
        /// 库位编码
        /// </summary>
        public virtual string? LocationCode { get; set; }

        /// <summary>
        /// 分区编码
        /// </summary>
        public virtual string? PartitionCode { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public virtual string? ItemCode { get; set; }
    }
}
