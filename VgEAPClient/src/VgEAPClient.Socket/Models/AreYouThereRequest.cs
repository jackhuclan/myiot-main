// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// EQP发送询问HOST是否存在, 上报时间点：设备端设定上报时间间隔（默认值：60秒 范围：1-999秒）
/// </summary>
[ReplyModelType(typeof(AreYouThereRequestReply))]
public class AreYouThereRequest : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// Server IP:127.0.0.1
    /// </summary>
    [XmlElement("server_ip")]
    public string ServerIp { get; set; } = string.Empty;
}
