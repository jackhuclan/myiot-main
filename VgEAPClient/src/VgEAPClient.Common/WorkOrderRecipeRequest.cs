// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common;

public class WorkOrderRecipeRequest
{
    public string ItemCode { get; set; } = string.Empty;
    public string EquipmentId { get; set; } = string.Empty;
}
