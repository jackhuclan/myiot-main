

namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于MES下发OTA升级信息
    /// </summary>
    internal class OATUpgradePayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public OATUpgradeHeader header { get; set; } = new();
        /// <summary>
        /// 内容
        /// </summary>
        public OATUpgradeBody body { get; set; } = new();
    }
    internal class OATUpgradeHeader
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

    internal class OATUpgradeBody
    {
        /// <summary>
        /// 应用信息集合
        /// </summary>
        public List<OATUpgradeApplicationInfo> applicationInfoList { get; set; } = new();
    }
    /// <summary>
    /// 应用信息集合
    /// </summary>
    internal class OATUpgradeApplicationInfo
    {
        /// <summary>
        /// 版本
        /// </summary>
        public string version { get; set; } = string.Empty;
        /// <summary>
        /// 安装包路径
        /// </summary>
        public string packageFilePath { get; set; } = string.Empty;

        /// <summary>
        /// 是否强制升级
        /// </summary>
        public string forceFlag { get; set; } = string.Empty;
        /// <summary>
        /// 应用名
        /// </summary>
        public string applicationName { get; set; } = string.Empty;
    }
}
