// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// EQP 发送机台当前控制模式给HOST
/// </summary>
[ReplyModelType(typeof(EquipmentControlModeReply))]
public class EquipmentControlMode : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// 1:Local 离线本地模式 2:Remote 在线远程模式
    /// </summary>
    [XmlElement("control_mode")]
    public string ControlMode { get; set; } = string.Empty;
}
