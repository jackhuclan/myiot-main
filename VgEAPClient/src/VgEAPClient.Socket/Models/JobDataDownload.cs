// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// HOST发送任务信息给EQP
/// </summary>
[ReplyModelType(typeof(JobDataDownloadReply))]
public class JobDataDownload : EapMessage.EapBody
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
    /// 设备端口ID，如果没端口，此栏位为空
    /// L01-上料口#1
    /// L02-上料口#2
    /// L03- NG工位
    /// L04- 陪镀位
    /// ...
    /// U01-下料口#1
    /// U02-下料口#2
    /// U03-下料口#3
    /// U04- NG位
    /// U05- 收板机陪镀板工位
    /// U06- 检修NG位
    /// </summary>
    [XmlElement("port_id")]
    public string PortId { get; set; } = string.Empty;

    /// <summary>
    /// 任务数量
    /// </summary>
    [XmlElement("total_panel_count ")]
    public string TotalPanelCount { get; set; } = string.Empty;

    /// <summary>
    /// 产品列表
    /// </summary>
    [XmlElement("panel_list")]
    public List<Panel> PanelList { get; set; } = new();

    /// <summary>
    /// 配方参数列表
    /// </summary>
    [XmlElement("recipe_parameter_list")]
    public List<RecipeParameter> RecipeParameterList { get; set; } = new();

    [XmlRoot("panel")]
    public class Panel
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        [XmlElement("panel_id")]
        public string PanelId { get; set; } = string.Empty;
    }
}

[XmlRoot("recipe_parameter")]
public class RecipeParameter
{
    /// <summary>
    /// 配方参数名称
    /// </summary>
    [XmlElement("item_name")]
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 配方参数值
    /// </summary>
    [XmlElement("item_value")]
    public string ItemValue { get; set; } = string.Empty;
}
