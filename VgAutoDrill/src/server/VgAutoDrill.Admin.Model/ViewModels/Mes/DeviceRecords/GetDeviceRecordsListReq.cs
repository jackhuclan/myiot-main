using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceRecords
{
    public class GetDeviceRecordsListReq : Page
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
        /// 等待时长排序 0-asc 1-desc
        /// </summary>
        public virtual int? OrderByWaitTime { get; set; }
    }
}
