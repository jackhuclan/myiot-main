// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// EQP 发送机台当前操作模式给HOST
/// </summary>
[ReplyModelType(typeof(EquipmentOperationModeReply))]
public class EquipmentOperationMode : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// 1:Manual 手动模式
    /// 2:Auto 自动模式
    /// </summary>
    [XmlElement("operation_mode")]
    public string OperationMode { get; set; } = string.Empty;
}
