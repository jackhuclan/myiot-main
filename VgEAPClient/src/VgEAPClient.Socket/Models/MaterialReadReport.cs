// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;
using VgEAPClientLib.EAP.Models;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// 物料身份读取报告 EQP发送物料ID给HOST
/// </summary>
[ReplyModelType(typeof(MaterialReadReportReply))]
public class MaterialReadReport : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// 扫码枪扫到物料号
    /// *读取失败固定为ERROR
    /// </summary>
    [XmlElement("material_id")]
    public string MaterialId { get; set; } = string.Empty;
}
