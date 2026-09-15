namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Device
{
    /// <summary>
    /// 首页设备状态统计
    /// </summary>
    public class DeviceStatusForHomeDto
    {
        /// <summary>
        /// 设备总数
        /// </summary>
        public virtual int AllDeviceCount { get; set; }

        /// <summary>
        /// 运行设备数量
        /// </summary>
        public virtual int RunningCount { get; set; }

        /// <summary>
        /// 待机设备数量
        /// </summary>
        public virtual int StandbyCount { get; set; }

        /// <summary>
        /// 故障设备数量
        /// </summary>
        public virtual int WarnningCount { get; set; }

        /// <summary>
        /// 在线设备数量
        /// </summary>
        public virtual int OnlineCount { get; set; }

        /// <summary>
        /// 离线设备数量
        /// </summary>
        public virtual int OfflineCount { get; set; }

        /// <summary>
        /// 维护设备数量
        /// </summary>
        public virtual int MaintenanceCount { get; set; }

        /// <summary>
        /// 低电量设备数量
        /// </summary>
        public virtual int LowBatteryCount { get; set; }

        /// <summary>
        /// 充电中设备数量
        /// </summary>
        public virtual int ChargingCount { get; set; }

    }
}
