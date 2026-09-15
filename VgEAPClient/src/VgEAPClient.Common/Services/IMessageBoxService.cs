// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Services;

public interface IMessageBoxService
{
    void ShowResult(string message);
    void ShowResult(string message, string title);
}
