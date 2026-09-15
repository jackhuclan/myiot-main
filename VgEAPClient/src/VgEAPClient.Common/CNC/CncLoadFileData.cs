// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace VgEAPClient.Common.CNC;

public class CncLoadFileData
{
    public LoadModel PgmLoadModel { get; set; } = new LoadModel();
    public LoadModel DiaLoadModel { get; set; } = new LoadModel();
    public LoadModel AtpLoadModel { get; set; } = new LoadModel();

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });
    }

    public void ClearData()
    {
        PgmLoadModel.ClearData();
        DiaLoadModel.ClearData();
        AtpLoadModel.ClearData();
    }
}

public class LoadModel
{
    public string FilePath { get; set; } = string.Empty;
    public string LoadResult { get; set; } = string.Empty;

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });
    }

    public void ClearData()
    {
        FilePath = string.Empty;
        LoadResult = string.Empty;
    }
}

public class CncLoadResult
{
    public string PgmLoadResult { get; set; } = string.Empty;
    public string DiaLoadResult { get; set; } = string.Empty;
    public string AtpLoadResult { get; set; } = string.Empty;

    public string GetAllLoadResult()
    {
        return AtpLoadResult + DiaLoadResult + PgmLoadResult;
    }

    public void ClearData()
    {
        PgmLoadResult = string.Empty;
        DiaLoadResult = string.Empty;
        AtpLoadResult = string.Empty;
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });
    }
}
