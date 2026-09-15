// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


namespace VgEAPClient.Common.Communication.Outbound;
public class MaterialUseReportModel : EQPReportModel<EQPReportHeader, MaterialUseReportBody, EQPReportResult>
{
    public MaterialUseReportModel(EQPReportHeader header, MaterialUseReportBody body, EQPReportResult result)
        : base(header, body, result)
    {
    }
}

public class MaterialUseReportBody : EQPReportBody
{
    /// <summary>
    /// 物料ID
    /// </summary>
    public string MaterialID { get; set; } = string.Empty;
    /// <summary>
    /// 物料类型
    /// </summary>
    public string MaterialType { get; set; } = string.Empty;
    /// <summary>
    /// 物料已使用数量
    /// </summary>
    public string MaterialQTY { get; set; } = string.Empty;

}
