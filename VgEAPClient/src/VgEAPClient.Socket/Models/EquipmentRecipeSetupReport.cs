// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// 机台配方参数(配方名、配方参数、配方文件、cam文件)调用报告
/// EQP发送配方参数调用结果给HOST
/// </summary>
[ReplyModelType(typeof(EquipmentRecipeSetupReportReply))]
public class EquipmentRecipeSetupReport : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// *如机台具备配方名管理功能，则为配方名称
    /// *如机台不具备配方名管理功能，则为任务名称
    /// </summary>
    [XmlElement("process_id")]
    public string ProcessId { get; set; } = string.Empty;

    /// <summary>
    /// *如机台调用配方档，则为配方档全路径
    /// </summary>
    [XmlElement("recipe_path")]
    public string RecipePath { get; set; } = string.Empty;

    /// <summary>
    /// *如机台调用CAM档，则为CAM 档全路径
    /// </summary>
    [XmlElement("cam_path")]
    public string CamPath { get; set; } = string.Empty;

    /// <summary>
    /// 0:OK 1:NG
    /// </summary>
    [XmlElement("setup_result")]
    public string SetupResult { get; set; } = string.Empty;
}
