// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// HOST发送呼叫RGV指令给EQP
/// </summary>
[ReplyModelType(typeof(RGVDispatchCommandReply))]
public class RGVDispatchCommand : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称
    /// </summary>
    [XmlElement("job_id")]
    public string JobId { get; set; } = string.Empty;

    /// <summary>
    /// 目的地
    /// </summary>
    [XmlElement("to_id")]
    public string ToId { get; set; } = string.Empty;
}
