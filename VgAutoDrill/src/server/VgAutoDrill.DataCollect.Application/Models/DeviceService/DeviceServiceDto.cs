namespace VgAutoDrill.DataCollect.Application.Models.DeviceService
{
    /// <summary>
    /// 设备服务调用记录
    /// </summary>
    public class DeviceServiceDto
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
        /// 事件编码，
        /// 对应表中的 service_id
        /// </summary>
        public string ServiceCode { get; set; } = string.Empty;
        /// <summary>
        /// 事件编码，
        /// 对应表中的 event_id
        /// </summary>
        public string EventCode { get; set; } = string.Empty;
        /// <summary>
        /// 事件名称，
        /// 对应表中的 event_name
        /// </summary>
        public string EventName { get; set; } = string.Empty;
        /// <summary>
        /// 目标设备编码，
        /// 对应表中的 target_device_id
        /// </summary>
        public string TargetDeviceCode { get; set; } = string.Empty;
        /// <summary>
        /// 目标设备类型编码，
        /// 对应表中的 target_product_id
        /// </summary>
        public string TargetProductCode { get; set; } = string.Empty;
        /// <summary>
        /// 申请参数，
        /// 对应表中的 request_json
        /// </summary>
        public string RequestJson { get; set; } = string.Empty;
        /// <summary>
        /// 响应参数，
        /// 对应表中的 response_json
        /// </summary>
        public string ResponseJson { get; set; } = string.Empty;
        /// <summary>
        /// 创建时间，
        /// 对应表中的 createtime
        /// </summary>
        public string CreateTime { get; set; } = string.Empty;
    }
}
