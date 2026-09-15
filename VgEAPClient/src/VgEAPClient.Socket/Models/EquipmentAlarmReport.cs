// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// EQP发送报警数据给HOST
/// </summary>
[ReplyModelType(typeof(EquipmentAlarmReportReply))]
public class EquipmentAlarmReport : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// 0:警报清除 1:警报发生
    /// </summary>
    [XmlElement("report_type")]
    public string ReportType { get; set; } = string.Empty;

    /// <summary>
    /// S: 重大  L: 一般 W：预警
    /// </summary>
    [XmlElement("alarm_type")]
    public string AlarmType { get; set; } = string.Empty;

    /// <summary>
    /// 警报代码
    /// </summary>
    [XmlElement("alarm_code")]
    public string AlarmCode { get; set; } = string.Empty;
}
