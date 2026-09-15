// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// EQP 回复任务信息
/// </summary>
public class EquipmentJobDataRequestReply : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// 批次任务
    /// </summary>
    [XmlElement("job_id")]
    public string JobId { get; set; } = string.Empty;

    /// <summary>
    /// 生产数量
    /// </summary>
    [XmlElement("total_panel_count")]
    public string TotalPanelCount { get; set; } = string.Empty;

    /// <summary>
    /// 当前完工数量
    /// </summary>
    [XmlElement("process_panel_count")]
    public string ProcessPanelCount { get; set; } = string.Empty;
}
