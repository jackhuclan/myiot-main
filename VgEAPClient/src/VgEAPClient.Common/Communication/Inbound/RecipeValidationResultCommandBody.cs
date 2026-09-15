// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Inbound;

public class RecipeValidationResultCommandBody : EQPReportBody
{
    public string? RecipeID { get; set; }  //配方ID或配方资料路径
    public int? Result { get; set; }
    public string IsOK { get; set; } = string.Empty;
}
