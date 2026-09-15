// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// 机台配方修改报告 EQP发送配方修改给HOST
/// </summary>
[ReplyModelType(typeof(RecipeModifyReportReply))]
public class EquipmentRecipeModifyReport : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// 配方名称
    /// </summary>
    [XmlElement("recipe_name")]
    public string RecipeName { get; set; } = string.Empty;

    /// <summary>
    /// 1: New 新增
    /// 2: Edit 修改
    /// 3: Delete 删除
    /// </summary>
    [XmlElement("modify_code")]
    public string ModifyCode { get; set; } = string.Empty;
}
