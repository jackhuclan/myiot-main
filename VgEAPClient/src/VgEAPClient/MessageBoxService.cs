// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using VgEAPClient.Common.Services;
using WindowsFormsLifetime;

namespace VgEAPClient;

public class MessageBoxService : IMessageBoxService
{
    private readonly IGuiContext _guiContext;

    public MessageBoxService(IGuiContext guiContext)
    {
        _guiContext = guiContext;
    }

    public void ShowResult(string message, string title)
    {
        _guiContext.Invoke(new Action(() =>
        {
            MessageBox.Show(message, title);
        }));
    }

    public void ShowResult(string message)
    {
        _guiContext.Invoke(new Action(() =>
        {
            MessageBox.Show(message);
        }));
    }
}
