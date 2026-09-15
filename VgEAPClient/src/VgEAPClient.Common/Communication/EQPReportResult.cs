// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace VgEAPClient.Common.Communication;

public class EQPReportResult
{
    /// <summary>
    ///
    /// </summary>
    [JsonConverter(typeof(IntConverter))]
    public int Code { get; set; } = 1;

    /// <summary>
    ///
    public string MessageCH { get; set; } = string.Empty;

    /// </summary>
    public string MessageEN { get; set; } = string.Empty;
}
