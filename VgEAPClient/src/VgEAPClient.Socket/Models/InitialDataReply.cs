// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// 设备回复EAP数据
/// </summary>
public class InitialDataReply : EapMessage.EapBody
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

    /// <summary>
    /// 1: Manual 手动模式 2: Auto自动模式
    /// </summary>
    [XmlElement("operation_mode")]
    public string OperationMode { get; set; } = string.Empty;

    /// <summary>
    /// 1:Run 运行 2:Pause 暂停 3:Idle 待机 4:Down 故障 5:PM 保养 6:Ready准备
    /// </summary>
    [XmlElement("eqp_status")]
    public string EqpStatus { get; set; } = string.Empty;

    /// <summary>
    /// 配方名称；如果没有，默认为空值
    /// </summary>
    [XmlElement("recipe_name")]
    public string RecipeName { get; set; } = string.Empty;

    /// <summary>
    /// 配方档全路径；如果没有，默认为空值
    /// </summary>
    [XmlElement("recipe_path")]
    public string RecipePath { get; set; } = string.Empty;

    /// <summary>
    /// CAM 档全路径；如果没有，默认为空值
    /// </summary>
    [XmlElement("cam_path")]
    public string CamPath { get; set; } = string.Empty;

    /// <summary>
    /// 当前任务名称；如果没有，默认为空值
    /// </summary>
    [XmlElement("job_id")]
    public string JobId { get; set; } = string.Empty;

    /// <summary>
    /// 任务总数量；如果没有，默认为空值
    /// </summary>
    [XmlElement("total_panel_count")]
    public string TotalPanelCount { get; set; } = string.Empty;

    /// <summary>
    /// 当前完工数量；如果没有，默认为空值
    /// </summary>
    [XmlElement("process_panel_count")]
    public string ProcessPanelCount { get; set; } = string.Empty;
}
