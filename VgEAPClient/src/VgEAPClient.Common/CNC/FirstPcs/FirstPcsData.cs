// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.CNC.FirstPcs;

public class FirstPcsData
{
    public event Action? OnPgmFilePathChanged;

    private string _pgmFilePath = string.Empty;

    public string PgmFilePath
    {
        get => _pgmFilePath;
        set
        {
            bool bChanged = _pgmFilePath != value;
            _pgmFilePath = value;

            if (bChanged)
            {
                OnPgmFilePathChanged?.Invoke();
            }
        }
    }
}
