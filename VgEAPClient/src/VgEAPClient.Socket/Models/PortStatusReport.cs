// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// 端口状态  上下料端口状态上报给HOST
/// </summary>
[ReplyModelType(typeof(PortStatusReportReply))]
public class PortStatusReport : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// Unload Count(Binary  1~999)
    /// 下料数量
    /// </summary>
    [XmlElement("unload_count")]
    public string UnloadCount { get; set; } = string.Empty;

    /// <summary>
    /// Panel Type:(Binary 1~15)
    /// 板涨缩类型
    /// 1：涨
    /// 2：缩
    /// 3：OK
    /// 4：NG
    /// </summary>
    [XmlElement("panel_type")]
    public string PanelType { get; set; } = string.Empty;

    /// <summary>
    /// 1:Load Request 请求上料
    /// 2:Load Complete 上料完成
    /// 3:Unload Request 请求下料
    /// 4:Unload Complete 下料完成
    /// </summary>
    [XmlElement("port_status")]
    public string PortStatus { get; set; } = string.Empty;
}
