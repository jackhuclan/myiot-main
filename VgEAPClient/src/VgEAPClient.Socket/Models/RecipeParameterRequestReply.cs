// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// EQP回复Recipe Parameter信息给HOST
/// </summary>
public class RecipeParameterRequestReply : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// 配方名称 *如果设备使用配方参数不需要配方名时，此栏位为空
    /// </summary>
    [XmlElement("recipe_id")]
    public string RecipeId { get; set; } = string.Empty;

    /// <summary>
    /// 配方参数列表
    /// </summary>
    [XmlElement("recipe_parameter_list")]
    public List<RecipeParameter> RecipeParameterList { get; set; } = new();
}
