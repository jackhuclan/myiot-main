namespace VegaIot.External.XianjinIot.Models;

/// <summary>
/// 上报请求公钥
/// </summary>
internal class PublicKeyPaloyload
{
    /// <summary>
    /// 内容
    /// </summary>
    public PublicKeyBody Body { get; set; } = new();
}
/// <summary>
/// 内容
/// </summary>
internal class PublicKeyBody
{
    /// <summary>
    /// 设备sn码
    /// </summary>
    public string sn { get; set; } = string.Empty;

    /// <summary>
    /// 时间戳
    /// </summary>
    public long timestamp { get; set; } = 0;
}
