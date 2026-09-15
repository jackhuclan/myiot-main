// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using SharpNodeSettings.OpcUaServer;

namespace VgEAPClient.Common.OpcUaServer;

public interface IOpcUaServerStartup
{
    public event Action? OnStartup;
    public event Action<Exception>? OnStartupFailed;

    SharpNodeSettingsServer _sharpNodeSettingsServer { get; set; }
    List<NodeDescription> extradrilList { get; set; }

    string NotUseEqpName { get; set; }
    Task Start();
}
