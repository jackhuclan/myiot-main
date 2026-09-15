namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Device
{
    public class DeviceStatsDto
    {
        /// <summary>
        /// 设备类型
        /// </summary>
        public virtual string? DeviceTypeName { get; set; }

        /// <summary>
        /// 设备总数
        /// </summary>
        public virtual decimal AllDeviceCount { get; set; }

        /// <summary>
        /// 设备在线比例
        /// </summary>
        public virtual decimal OnlineDevicePr { get; set; }
    }
}
