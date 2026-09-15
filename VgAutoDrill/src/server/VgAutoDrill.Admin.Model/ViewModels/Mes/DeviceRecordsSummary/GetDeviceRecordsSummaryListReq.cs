using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceRecordsSummary
{
    public class GetDeviceRecordsSummaryListReq : Page
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 查询开始时间
        /// </summary>
        public DateTime? QueryStartTime { set; get; }

        /// <summary>
        /// 查询结束时间
        /// </summary>
        public DateTime? QueryEndTime { set; get; }

        public virtual List<string>? RouteCodeList { get; set; }

        public QueryOrderByEnum? QueryOrderBy { get; set; }

        /// <summary>
        /// 班次(0-白班，1-中班，2-夜班)
        /// </summary>
        public virtual int? Sailings { get; set; }

        /// <summary>
        /// 等待时长排序 0-asc 1-desc
        /// </summary>
        public virtual int? OrderByWaitTime { get; set; }

        /// <summary>
        /// 设备编号集合
        /// </summary>
        public virtual List<string>? DeviceCodeList { get; set; }
    }
}
