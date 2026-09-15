// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Outbound;

public class PanelProcessDataReportBody : EQPReportBody
{
    public string PanelID { get; set; } = string.Empty;
    /// <summary>
    /// Panel 序号
    /// </summary>
    public string SequenceNO { get; set; } = string.Empty;
    /// <summary>
    /// 批次ID
    /// </summary>
    public string LotID { get; set; } = string.Empty;
    /// <summary>
    /// 配方名称
    /// </summary>
    public string RecipeID { get; set; } = string.Empty;
    /// <summary>
    /// 飞把ID
    /// </summary>
    public string ToolID { get; set; } = string.Empty;
    /// <summary>
    /// 槽号
    /// </summary>
    public string SlotID { get; set; } = string.Empty;

    public List<Parameter> ParameterList = new List<Parameter>();
}
