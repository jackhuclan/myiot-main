// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Outbound;

public class EQPDataCollectionReportBody : EQPReportBody
{
    public string? TotalOnQty { get; set; } = ""; //总开机时长
    public string? TotalProQty { get; set; } = ""; //总加工时长
    public string? TotalOnTime { get; set; }//总开机时长
    public string? TotalProTime { get; set; }//总加工时长
    public string? ProductNO { get; set; } = "";
    public string? LotID { get; set; } = "";
    public List<Parameter>? ParameterList { get; set; }

}
