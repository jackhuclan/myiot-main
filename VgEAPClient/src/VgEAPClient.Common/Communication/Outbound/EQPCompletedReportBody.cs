// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Outbound;

public class EQPCompletedReportBody : EQPReportBody
{
    public string? LotID { get; set; }//批次号
    public string? ItemNum { get; set; }//批次号（3位）
    public string? PanelQTY { get; set; }//完工数量
    public string? FinishStatus { get; set; } = "1"; //批次号


}
