// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// 机台任务进展信息报告 EQP发送任务进展信息给HOST
/// </summary>
[ReplyModelType(typeof(EquipmentJobDataProcessReportReply))]
public class EquipmentJobDataProcessReport : EapMessage.EapBody
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
    /// 任务总数量
    /// </summary>
    [XmlElement("total_panel_count")]
    public string TotalPanelCount { get; set; } = string.Empty;

    /// <summary>
    /// 任务生产OK板数量
    /// </summary>
    [XmlElement("ok_panel_count")]
    public string OkPanelCount { get; set; } = string.Empty;

    /// <summary>
    /// 任务生产NG板数量
    /// </summary>
    [XmlElement("ng_panel_count")]
    public string NgPanelCount { get; set; } = string.Empty;

    /// <summary>
    /// 接收命令:
    /// 1.建立生产任务
    /// 2.更新生产任务（仅变更数量）
    /// 3.删除生产任务
    /// 4.执行任务失败
    /// 任务执行：
    /// 5.开始生产任务
    /// 6.完成生产任务
    /// </summary>
    [XmlElement("process_code")]
    public string ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 异常原因：当执行任务失败时，回复异常原因信息
    /// </summary>
    [XmlElement("exception_reason")]
    public string ExceptionReason { get; set; } = string.Empty;
}
