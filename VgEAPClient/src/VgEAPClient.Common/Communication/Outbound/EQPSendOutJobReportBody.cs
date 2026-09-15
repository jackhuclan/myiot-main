// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Outbound;

public class EQPSendOutJobReportBody : EQPReportBody
{
    /// <summary>
    /// 工单号
    /// </summary>
    public string LotID { get; set; } = string.Empty;
    /// <summary>
    /// 批次号(3位)
    /// </summary>
    public string? ItemNum { get; set; } = string.Empty;
    /// <summary>
    /// 大板件ID
    /// </summary>
    public string? PanelID { get; set; }
    /// <summary>
    /// 小板件ID   
    /// </summary>
    public string? SetID { get; set; }
    /// <summary>
    /// Panel序号
    /// </summary>
    public int? SequenceNO { get; set; }
    /// <summary>
    /// 结果 OK NG
    /// </summary>
    public string? Result { get; set; }
    /// <summary>
    /// 配方ID
    /// </summary>
    public string RecipeID { get; set; } = string.Empty;

    public List<Parameter> ParameterList { get; set; } = new List<Parameter>();
}
