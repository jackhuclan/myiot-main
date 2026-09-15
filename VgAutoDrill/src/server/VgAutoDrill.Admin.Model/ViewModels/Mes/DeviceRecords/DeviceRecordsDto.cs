namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceRecords
{
    public class DeviceRecordsDto : BaseDto
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 工作时间
        /// </summary>
        public virtual string? WorkTime { get; set; }

        /// <summary>
        /// 等待时间
        /// </summary>
        public virtual string? WaitTime { get; set; }

        /// <summary>
        /// 异常时间
        /// </summary>
        public virtual string? ErrorTime { get; set; }

        /// <summary>
        /// 开机时间
        /// </summary>
        public virtual string? OpenTime { get; set; }

        /// <summary>
        /// 稼动率
        /// </summary>
        public virtual string? Duty { get; set; }

        /// <summary>
        /// 结束到开始总时间
        /// </summary>
        public virtual string? EndToStartTime { get; set; }

        /// <summary>
        /// 清洗夹头总时间
        /// </summary>
        public virtual string? CollectClearTime { get; set; }

        /// <summary>
        /// 数据日期
        /// </summary>
        public virtual string? DateString { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public virtual string? RouteCode { get; set; }
    }
}
