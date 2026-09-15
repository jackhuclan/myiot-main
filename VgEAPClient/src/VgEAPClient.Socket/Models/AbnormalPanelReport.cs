// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// EQP向HOST报告异常板原因
/// </summary>
[ReplyModelType(typeof(AbnormalPanelReportReply))]
public class AbnormalPanelReport : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// 批次任务
    /// </summary>
    [XmlElement("job_id")]
    public string JobId { get; set; } = string.Empty;

    /// <summary>
    /// 异常板数量
    /// </summary>
    [XmlElement("panel_count")]
    public string PanelCount { get; set; } = string.Empty;
}
