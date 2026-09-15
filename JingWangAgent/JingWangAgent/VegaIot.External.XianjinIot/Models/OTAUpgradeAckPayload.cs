

namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于设备上报OTA升级结果
    /// </summary>
    internal class OTAUpgradeAckPayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public OATUpgradeAckHeader header { get; set; } = new();
        /// <summary>
        /// 内容
        /// </summary>
        public OATUpgradeAckBody body { get; set; } = new();
    }
    internal class OATUpgradeAckHeader
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

    internal class OATUpgradeAckBody
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
        /// 结果:200为成功，其它为失败
        /// </summary>
        public int code { get; set; } = 0;
        /// <summary>
        /// 信息
        /// </summary>
        public string msg { get; set; } = string.Empty;

        /// <summary>
        /// 应用信息集合
        /// </summary>
        public List<OATUpgradeAckUpgradeAck> upgradeAck { get; set; } = new();
    }
    internal class OATUpgradeAckUpgradeAck
    {
        /// <summary>
        /// 应用名
        /// </summary>
        public string applicationName { get; set; } = string.Empty;
        /// <summary>
        /// 是否成功
        /// </summary>
        public string success { get; set; } = string.Empty;
    }
}
