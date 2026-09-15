// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// 铝片打印报告 上 PIN 机铝片码打印完成后上报
/// </summary>
[ReplyModelType(typeof(AluminiumPrintReportReply))]
public class AluminumPrintReport : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// 铝片ID
    /// </summary>
    [XmlElement("aluminum_id")]
    public string aluminum_id { get; set; } = string.Empty;

    /// <summary>
    /// 板号ID #1
    /// </summary>
    [XmlElement("panel_id1")]
    public string PanelId1 { get; set; } = string.Empty;

    /// <summary>
    /// 板号ID #2
    /// </summary>
    [XmlElement("panel_id2")]
    public string PanelId2 { get; set; } = string.Empty;

    /// <summary>
    /// 板号ID #3
    /// </summary>
    [XmlElement("panel_id3")]
    public string PanelId3 { get; set; } = string.Empty;
}
