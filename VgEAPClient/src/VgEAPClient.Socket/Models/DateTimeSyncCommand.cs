// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// HOST发送对时命令给EQP
/// </summary>
[ReplyModelType(typeof(DateTimeSyncReply))]
public class DateTimeSyncCommand : EapMessage.EapBody
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
