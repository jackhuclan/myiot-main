namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Device
{
    /// <summary>
    /// 设备运行情况统计
    /// </summary>
    public class DeviceStatusStatsDto
    {
        /// <summary>
        /// 设备运行占比
        /// </summary>
        public virtual string? WorkingDevicePr { get; set; }

        /// <summary>
        /// 设备故障占比
        /// </summary>
        public virtual string? WarnningDevicePr { get; set; }

        /// <summary>
        /// 设备停用占比
        /// </summary>
        public virtual string? StopDevicePr { get; set; }

        /// <summary>
        /// 设备待机占比
        /// </summary>
        public virtual string? StandbyDevicePr { get; set; }
    }
}
