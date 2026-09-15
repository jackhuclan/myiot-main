namespace VegaIot.External.XianjinIot.Models;

/// <summary>
/// 用于MES下发公钥给设备
/// </summary>
internal class PublicKeyAckPayload
{
    /// <summary>
    /// 请求头
    /// </summary>
    public PublicKeyAckHeader Header { get; set; } = new();
    /// <summary>
    /// 内容
    /// </summary>
    public PublicKeyBody Body { get; set; } = new();
}

/// <summary>
/// 请求头
/// </summary>
internal class PublicKeyAckHeader
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

/// <summary>
/// 内容
/// </summary>
internal class PublicKeyAckBody
{
    /// <summary>
    ///  公钥
    /// </summary>
    public string publicKey { get; set; } = string.Empty;
}
