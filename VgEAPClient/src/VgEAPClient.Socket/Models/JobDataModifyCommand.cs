// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// HOST通知EQP更新或删除任务
/// </summary>
[ReplyModelType(typeof(JobDataModifyCommandReply))]
public class JobDataModifyCommand : EapMessage.EapBody
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
    /// 任务数量(旧)
    /// </summary>
    [XmlElement("old_panel_count")]
    public string OldPanelCount { get; set; } = string.Empty;

    /// <summary>
    /// 任务数量(新)
    /// </summary>
    [XmlElement("new_panel_count")]
    public string NewPanelCount { get; set; } = string.Empty;

    /// <summary>
    /// 1: Updata 更新任务
    /// 2: Delete 删除任务
    /// 3:Change 更换任务
    /// </summary>
    [XmlElement("modify_type")]
    public string ModifyType { get; set; } = string.Empty;
}
