// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using VgEAPClient.Socket.Models;

namespace VgEAPClient.Socket.Client;

public interface ISokcetClient : IDisposable
{
    event OnHostRequestEvent OnHostRequest;
    event OnHostConnectEvent OnHostConnect;
    event OnHostCloseEvent OnHostClose;
    event OnHostConnectErrorEvent OnHostConnectError;

    EapServerOptions Options { get; }
    bool IsConnected { get; }

    Task Connect(CancellationToken cancellationToken = default);

    Task<EapMessage> SendMessage(EapMessage request, CancellationToken cancellationToken = default);

    Task PostMessage(EapMessage request, CancellationToken cancellationToken = default);

    Task<EapMessage> PostByet(byte[] bytes, string TransactionId, CancellationToken cancellationToken = default);

    Task PostByet(byte[] bytes);
}

public delegate Task OnHostRequestEvent(EapMessage hostRequestMessage);

public delegate Task OnHostConnectEvent(string Host, int Port);

public delegate Task OnHostCloseEvent(string Host, int Port);

public delegate Task OnHostConnectErrorEvent(string Host, int Port);
