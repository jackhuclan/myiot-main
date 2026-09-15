// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// EQP发送设备状态给HOST
/// </summary>
[ReplyModelType(typeof(EquipmentCurrentStatusReply))]
public class EquipmentCurrentStatus : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// 0: Unknown, 1:Run 运行 2:Pause 暂停 3:Idle 待机 4:Down 故障 5:PM 保养 6:Ready准备
    /// </summary>
    [XmlElement("eqp_status")]
    public EqpStatus EqpStatus { get; set; } = EqpStatus.Unknown;
}
