// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


namespace VgEAPClient.Common;

internal class DefaultGuiLogger : IGuiLogger
{
    public event Action<string, string>? OnShowResult;
    public event Action<string>? OnShowWarn;
    public event Action<string>? OnShowError;
    public event Action<string>? OnShowRecv;
    public event Action<string>? OnShowSend;

    public void ShowError(string msg) => OnShowError?.Invoke(msg);
    public void ShowRecv(string msg) => OnShowRecv?.Invoke(msg);
    public void ShowResult(string msg, string events = "") => OnShowResult?.Invoke(msg, events);
    public void ShowSend(string msg) => OnShowSend?.Invoke(msg);
    public void ShowWarn(string msg) => OnShowWarn?.Invoke(msg);
}
