// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// EQP 发送系统时间给HOST
/// </summary>
[ReplyModelType(typeof(EquipmentCurrentDateTimeReply))]
public class EquipmentCurrentDateTime : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// yyyyMMddhhmmss
    /// </summary>
    [XmlElement("date_time")]
    public string DateTime { get; set; } = string.Empty;
}
