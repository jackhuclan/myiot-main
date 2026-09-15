// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// HOST发送远程提示信息给EQP
/// </summary>
[ReplyModelType(typeof(CIMMessageCommandReply))]
public class CIMMessageCommand : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// 显示间隔时间 0:需要确认后关闭(默认)；>0:到达显示时间，自动关闭
    /// </summary>
    [XmlElement("interval_second_time")]
    public string IntervalSecondTime { get; set; } = string.Empty;

    /// <summary>
    /// 信息内容
    /// </summary>
    [XmlElement("cim_message")]
    public string CimMessage { get; set; } = string.Empty;
}
