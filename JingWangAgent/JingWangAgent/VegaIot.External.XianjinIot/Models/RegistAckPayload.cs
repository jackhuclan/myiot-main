

namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于MES下发注册结果
    /// </summary>
    internal class RegistAckPayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public RegistAckHeader Header { get; set; } = new();
        /// <summary>
        /// 内容
        /// </summary>
        public RegistAckBody Body { get; set; } = new();
    }
    internal class RegistAckHeader
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

    internal class RegistAckBody
    {
        /// <summary>
        /// sn
        /// </summary>
        public string sn { get; set; } = string.Empty;
        /// <summary>
        /// 注册结果:200为成功，其他为失败
        /// </summary>
        public int code { get; set; } = 0;
        /// <summary>
        /// 注册信息
        /// </summary>
        public string msg { get; set; } = string.Empty;
    }
}
