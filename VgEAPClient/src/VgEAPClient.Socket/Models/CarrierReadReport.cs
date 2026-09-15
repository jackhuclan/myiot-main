// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// 载具身份读取报告 EQP报告载具身份给HOST
/// </summary>
[ReplyModelType(typeof(CarrierReadReportReply))]
public class CarrierReadReport : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// 扫码枪扫到载具ID
    /// *读取失败固定为ERROR
    /// </summary>
    [XmlElement("carrier_id")]
    public string CarrierId { get; set; } = string.Empty;
}
