// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common;

public interface IGuiLogger
{
    event Action<string, string>? OnShowResult;
    event Action<string>? OnShowWarn;
    event Action<string>? OnShowError;
    event Action<string>? OnShowRecv;
    event Action<string>? OnShowSend;
    void ShowResult(string msg, string events = "");
    void ShowWarn(string msg);
    void ShowError(string msg);
    void ShowRecv(string msg);
    void ShowSend(string msg);
}
