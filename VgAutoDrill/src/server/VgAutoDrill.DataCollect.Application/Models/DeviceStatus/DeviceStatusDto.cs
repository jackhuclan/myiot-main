namespace VgAutoDrill.DataCollect.Application.Models.DeviceStatus
{
    /// <summary>
    /// 设备状态记录
    /// </summary>
    public class DeviceStatusDto
    {
        /// <summary>
        /// 设备编码，
        /// 对应表中的device_id
        /// </summary>
        public string DeviceCode { get; set; } = string.Empty;

        /// <summary>
        /// 设备类型编码，
        /// 对应表中的product_id
        /// </summary>
        public string DeviceTypeCode { get; set; } = string.Empty;
        /// <summary>
        /// 原有状态，
        /// 对应表中的 old_status
        /// </summary>
        public string OldStatus { get; set; } = string.Empty;
        /// <summary>
        /// 当前状态，
        /// 对应表中的 new_status
        /// </summary>
        public string NewStatus { get; set; } = string.Empty;
        /// <summary>
        /// 创建时间，
        /// 对应表中的 createtime
        /// </summary>
        public string CreateTime { get; set; } = string.Empty;
    }
}
