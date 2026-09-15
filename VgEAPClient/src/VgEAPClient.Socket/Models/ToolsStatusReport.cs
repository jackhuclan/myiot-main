// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;
using VgEAPClientLib.EAP.Models;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// 治具（缓冲垫、刀具等）状态报告 EQP报告治具（缓冲垫、刀具等）状态给HOST
/// </summary>
[ReplyModelType(typeof(ToolsStatusReportReply))]
public class ToolsStatusReport : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// 治具ID
    /// </summary>
    [XmlElement("tools_id")]
    public string ToolsId { get; set; } = string.Empty;

    /// <summary>
    /// 治具状态：
    /// 1:装载  2:卸载
    /// </summary>
    [XmlElement("tools_status")]
    public string ToolsStatus { get; set; } = string.Empty;
}
