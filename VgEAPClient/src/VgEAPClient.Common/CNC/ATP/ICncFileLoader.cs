// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using VgEAPClient.Common.CNC.ATP.Models;

namespace VgEAPClient.Common.CNC.ATP;

public interface ICncFileLoader
{
    /// <summary>
    /// atp file load failed event
    /// </summary>
    event Action<string>? OnLoadFileFailed;

    /// <summary>
    /// atp file parse failed event
    /// </summary>
    event Action<string>? OnParseFileFailed;

    Task LoadFile(string workOrder);

    MessageEntity GenerateNewAtp(string strMesAtpPath, string strDiaPath, string strGeneratePath = "");
}
