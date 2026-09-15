namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Device
{
    /// <summary>
    /// 调度设备 每日调度数量统计
    /// </summary>
    public class ScheduleDeviceByTimesStatsDto
    {
        /// <summary>
        /// 时间段
        /// </summary>
        public virtual List<string> StatsTimes { get; set; } = new List<string>();

        /// <summary>
        /// 每个设备 调度数量
        /// </summary>
        public virtual List<ScheduleStatsDto> QuantitysStats { get; set; } = new List<ScheduleStatsDto>();
    }

    public class ScheduleStatsDto
    {
        /// <summary>
        /// 设备编码
        /// </summary>
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public virtual string? Lable { get; set; }

        /// <summary>
        /// 数量集合
        /// </summary>
        public virtual List<string> Quantitys { get; set; } = new List<string>();
    }
}
