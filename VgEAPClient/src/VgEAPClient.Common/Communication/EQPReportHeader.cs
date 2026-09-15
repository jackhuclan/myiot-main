// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication;

public class EQPReportHeader
{
    /// <summary>
    ///
    /// </summary>
    public string MessageName { get; set; } = string.Empty;

    /// <summary>
    ///
    public string TransactionID { get; set; } = string.Empty;

    /// </summary>
    public string UserID { get; set; } = string.Empty;
}
