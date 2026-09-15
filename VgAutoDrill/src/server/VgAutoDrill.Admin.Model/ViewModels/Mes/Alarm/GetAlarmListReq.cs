using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DvAlarmRecord
{
    public class GetAlarmListReq : Page
    {
        public virtual string? AlarmName { get; set; }

        /// <summary>
        /// 告警编号
        /// </summary>
        public string AlarmCode { set; get; }
        public int? AlarmLevel { get; set; }
        public bool? IsHandled { get; set; }
        /// <summary>
        /// 状态
        /// </summary>

        public int Status { set; get; } = -1;

        public List<AlarmKind>? AlarmKinds { get; set; }

        /// <summary>
        /// 告警查询开始时间
        /// </summary>
        public DateTime? QueryStartTime { set; get; }

        /// <summary>
        /// 告警查询结束时间
        /// </summary>
        public DateTime? QueryEndTime { set; get; }

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
