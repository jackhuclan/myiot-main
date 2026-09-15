// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Outbound;

public class BrokenKnifeAlarmReportBody : EQPReportBody
{
    public string? LotID { get; set; }//批次号
    public string? ItemNum { get; set; }
    public string? FilePath { get; set; }
    public string? DrillPath { get; set; }

    /// BrokenKnifeAlarmReport
    public string? KnifeSeq { get; set; }

    public string? AxisNum { get; set; }
    public string? KnifeDia { get; set; }
    public string? SetLife { get; set; }
    public string? UsedLife { get; set; }
    public string? BrokenTime { get; set; }
    public string? HolesNum { get; set; }
    public string? PanelSite { get; set; }
    /// <summary>
    /// 断针数量
    /// </summary>
    public Int32? BrokenNum { get; set; }
    /// <summary>
    /// 更换磨次
    /// </summary>
    public String? ReplaceNum { get; set; }
    /// <summary>
    /// 单/双 刃
    /// </summary>
    /// <remarks>
    /// 1:单刃 2:双刃
    /// </remarks>
    public Int32? Knife { get; set; }
    /// <summary>
    /// 断刀刃长
    /// </summary>
    public Single? KnifeLength { get; set; }
    /// <summary>
    /// 断刃原因
    /// </summary>
    public String? BrokenReason { get; set; }
    /// <summary>
    /// 压力脚 
    /// </summary>
    /// <remarks>
    /// 1:是  2：否
    /// </remarks>
    public Int32? PressureFoot { get; set; }
    /// <summary>
    /// 趟数
    /// </summary>
    public string NumRank { get; set; } = string.Empty;
}
