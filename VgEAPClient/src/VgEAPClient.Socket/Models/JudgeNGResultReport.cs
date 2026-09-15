// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// 判定检修NG结果报告 EQP向HOST报告判定NG板结果
/// </summary>
[ReplyModelType(typeof(JudgeNGResultReportReply))]
public class JudgeNGResultReport : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// 产品ID
    /// </summary>
    [XmlElement("panel_id")]
    public string PanelId { get; set; } = string.Empty;

    /// <summary>
    /// 判定结果（0：检修OK板，1：待检修板）
    /// </summary>
    [XmlElement("result")]
    public string Result { get; set; } = string.Empty;
}
