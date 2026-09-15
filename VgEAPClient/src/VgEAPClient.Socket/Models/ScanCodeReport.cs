// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// PP裁切机扫码上报 EQP上报扫码据给HOST
/// </summary>
[ReplyModelType(typeof(ScanCodeReportReply))]
public class ScanCodeReport : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// 设备端口ID，如果没端口，此栏位为空
    /// L01-上料口#1
    /// L02-上料口#2
    /// L03- NG工位
    /// L04- 陪镀位
    /// ...
    /// U01-下料口#1
    /// U02-下料口#2
    /// U03-下料口#3
    /// U04- NG位
    /// U05- 收板机陪镀板工位
    /// U06- 检修NG位
    /// </summary>
    [XmlElement("port_id")]
    public string PortId { get; set; } = string.Empty;

    /// <summary>
    /// 扫码ID
    /// </summary>
    [XmlElement("code_id")]
    public string CodeId { get; set; } = string.Empty;
}
