namespace VgAutoDrill.DataCollect.Application.Models.DeviceProperty
{
    /// <summary>
    /// 设备属性记录
    /// </summary>
    public class DevicePropertyDto
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
        /// 事件参数，
        /// 对应表中的 properpty_json
        /// </summary>
        public string PropertyJson { get; set; } = string.Empty;
        /// <summary>
        /// 创建时间，
        /// 对应表中的 createtime
        /// </summary>
        public string CreateTime { get; set; } = string.Empty;
    }
}
