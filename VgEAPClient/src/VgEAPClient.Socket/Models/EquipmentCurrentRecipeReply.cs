// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// HOST 回复收到数据
/// </summary>
public class EquipmentCurrentRecipeReply : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// 0:OK 1:NG
    /// </summary>
    [XmlElement("return_code")]
    public string ReturnCode { get; set; } = string.Empty;
}
