

namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于设备注册信息
    /// </summary>
    internal class RegistPayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public RegistHeader header { get; set; } = new();
        /// <summary>
        /// 内容
        /// </summary>
        public RegistBody body { get; set; } = new();
    }

    internal class RegistHeader
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
    internal class RegistBody
    {
        public string signCode { get; set; } = string.Empty;

        /// <summary>
        /// 时间戳
        /// </summary>
        public long timestamp { get; set; } = 0;
        /// <summary>
        /// 设备类型
        /// </summary>
        public string deviceType { get; set; } = string.Empty;
        /// <summary>
        /// 设备编码
        /// </summary>
        public string deviceCode { get; set; } = string.Empty;
        /// <summary>
        /// 设备供应商
        /// </summary>
        public string deviceProvider { get; set; } = string.Empty;
        /// <summary>
        /// 会话密钥
        /// </summary>
        public string sessionKey { get; set; } = string.Empty;
        /// <summary>
        /// IP
        /// </summary>
        public string deviceIp { get; set; } = string.Empty;
        /// <summary>
        /// port
        /// </summary>
        public string devicePort { get; set; } = string.Empty;
        /// <summary>
        /// 入料口高度，针对钻机参数，其它设备不传单位cm
        /// </summary>
        public int feedHeight { get; set; } = 0;
        /// <summary>
        /// 出料口高度，针对钻机参数，其它设备不传单位cm
        /// </summary>
        public int dischargeHeight { get; set; } = 0;
        /// <summary>
        /// 应用信息集合
        /// </summary>
        public List<RegistApplicationInfo> applicationInfoList { get; set; } = new();
    }
    /// <summary>
    /// 应用信息集合
    /// </summary>
    internal class RegistApplicationInfo
    {
        /// <summary>
        /// 应用名
        /// </summary>
        public string name { get; set; } = string.Empty;
        /// <summary>
        /// 应用版本
        /// </summary>
        public string version { get; set; } = string.Empty;
    }
}
