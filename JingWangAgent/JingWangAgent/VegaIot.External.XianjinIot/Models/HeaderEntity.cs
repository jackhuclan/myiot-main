
namespace VegaIot.External.XianjinIot.Models
{
    internal class HeaderEntity
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
}
