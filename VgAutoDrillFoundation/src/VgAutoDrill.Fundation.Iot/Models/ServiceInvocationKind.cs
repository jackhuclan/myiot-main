using System.ComponentModel;

namespace VgAutoDrill.Fundation.Iot.Models;

/// <summary>
/// 服务调用类型
/// </summary>
public enum ServiceInvocationKind
{
    /// <summary>
    /// 请求回复类型
    /// </summary>
    [Description("请求回复类型")]
    RequestReply,

    /// <summary>
    /// 请求不等待回复
    /// </summary>
    [Description("请求不等待回复")]
    Post = 2
}
