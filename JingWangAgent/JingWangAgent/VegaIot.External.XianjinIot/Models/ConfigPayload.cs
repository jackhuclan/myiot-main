

namespace VegaIot.External.XianjinIot.Models
{
    internal class ConfigPayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public ConfigHeader header { get; set; } = new();
        /// <summary>
        /// 配置集合
        /// </summary>
        public List<ConfigBody> body { get; set; } = new();
    }

    internal class ConfigHeader
    {
        /// <summary>
        /// 根据body生产的hashCode
        /// </summary>
        public int signCode { get; set; } = 0;
        /// <summary>
        /// 用存储的公钥对hashCode进行加密
        /// </summary>
        public string signature { get; set; } = string.Empty;
    }

    internal class ConfigBody
    {
        /// <summary>
        /// sn
        /// </summary>
        public string sn { get; set; } = string.Empty;

        /// <summary>
        /// 上报设备信息频率，单位秒
        /// </summary>
        public int reportDeviceFrequency { get; set; } = 0;
        /// <summary>
        /// 上报钻机进度频率,单位秒
        /// </summary>
        public int reportDrillingScheduledFrequency { get; set; } = 0;
    }
}
