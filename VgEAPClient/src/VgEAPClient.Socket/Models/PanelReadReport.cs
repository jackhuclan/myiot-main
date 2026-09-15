// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// 读板报告 EQP向HOST报告Panel ID 读取结果
/// </summary>
[ReplyModelType(typeof(PanelReadReportReply))]
public class PanelReadReport : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// CCD读到板号
    /// *读取失败固定为ERROR
    /// </summary>
    [XmlElement("panel_id")]
    public string PanelId { get; set; } = string.Empty;

    /// <summary>
    /// 板子片型
    /// </summary>
    [XmlElement("slice_type")]
    public string SliceType { get; set; } = string.Empty;

    /// <summary>
    /// MES过账类型（TrackIn、TrackOut）
    /// </summary>
    [XmlElement("track_type")]
    public string TrackType { get; set; } = string.Empty;
}
