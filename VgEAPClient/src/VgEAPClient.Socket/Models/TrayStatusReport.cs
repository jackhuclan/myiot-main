// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// 底盘身份读取报告  EQP报告底盘身份给HOST
/// </summary>
[ReplyModelType(typeof(TrayStatusReportReply))]
public class TrayStatusReport : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// 底盘ID
    /// </summary>
    [XmlElement("tray_id")]
    public string TrayId { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称
    /// </summary>
    [XmlElement("job_id")]
    public string JobId { get; set; } = string.Empty;

    /// <summary>
    /// 底盘状态：
    /// 1:绑定 2:解绑
    /// </summary>
    [XmlElement("tray_status")]
    public string TrayStatus { get; set; } = string.Empty;
}
