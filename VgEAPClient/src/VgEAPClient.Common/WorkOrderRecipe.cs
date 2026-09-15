// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using VgEAPClient.Common.CNC;

namespace VgEAPClient.Common;

public class WorkOrderRecipe
{
    public string tagCode { get; set; } = string.Empty;
    public LoadFileType tagType { get; set; } = LoadFileType.PROGRAM;
    public string tagPath { get; set; } = string.Empty;
    public bool isLoaded { get; set; } = false;

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });
    }
}
