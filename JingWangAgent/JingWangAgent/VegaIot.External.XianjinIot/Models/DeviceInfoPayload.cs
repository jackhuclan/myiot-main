

namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于设备上报设备信息
    /// </summary>
    internal class DeviceInfoPayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public DeviceInfoHeader header { get; set; } = new();
        /// <summary>
        /// 内容
        /// </summary>
        public DeviceInfoBody body { get; set; } = new();
    }
    /// <summary>
    /// 请求头
    /// </summary>
    internal class DeviceInfoHeader
    {
        /// <summary>
        /// 根据body生产的hashCode
        /// </summary>
        public string signCode { get; set; } = string.Empty;

        /// <summary>
        /// 用存储的公钥对hashCode进行加密
        /// </summary>
        public string signature { get; set; } = string.Empty;
    }
    /// <summary>
    /// 内容
    /// </summary>
    internal class DeviceInfoBody
    {
        /// <summary>
        /// 设备码
        /// </summary>
        public string sn { get; set; } = string.Empty;
        /// <summary>
        /// 时间戳
        /// </summary>
        public string timestamp { get; set; } = string.Empty;
        /// <summary>
        /// 设备状态
        /// </summary>
        public int deviceStatus { get; set; } = 0;

        /// <summary>
        /// 设备编码
        /// </summary>
        public string deviceCode { get; set; } = string.Empty;

        /// <summary>
        /// 设备位置信息(仅AGV需要)
        /// </summary>
        public string location { get; set; } = string.Empty;
        /// <summary>
        /// 设备电量
        /// </summary>
        public string quantityOfElectricity { get; set; } = string.Empty;
        /// <summary>
        /// 任务进度20（表示20%）
        /// </summary>
        public int taskSchedule { get; set; } = 0;
    }
}
