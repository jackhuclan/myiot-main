// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


namespace VgEAPClient.Common.Communication.Outbound;
public class MaterialValidationRequestModel : EQPReportModel<EQPReportHeader, MaterialValidationRequestBody, EQPReportResult>
{
    public MaterialValidationRequestModel(EQPReportHeader header, MaterialValidationRequestBody body, EQPReportResult result)
        : base(header, body, result)
    {
    }
}

public class MaterialValidationRequestBody : EQPReportBody
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
    /// 1：Kitting ,2：UnKitting
    /// </summary>
    public int MaterialStatus { get; set; } = 1;
    /// <summary>
    /// 物料数量
    /// </summary>
    public string MaterialQTY { get; set; } = string.Empty;
    /// <summary>
    /// 位置
    /// </summary>
    public string Position { get; set; } = string.Empty;
}
