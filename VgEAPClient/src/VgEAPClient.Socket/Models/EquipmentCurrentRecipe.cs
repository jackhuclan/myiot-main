// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// 设备当前使用配方发生改变时，上报当前使用配方名称, EQP发送设备当前配方给HOST
/// </summary>
[ReplyModelType(typeof(EquipmentCurrentRecipeReply))]
public class EquipmentCurrentRecipe : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// 当前使用配方名称
    /// </summary>
    [XmlElement("current_recipe_name")]
    public string CurrentRecipeName { get; set; } = string.Empty;
}
