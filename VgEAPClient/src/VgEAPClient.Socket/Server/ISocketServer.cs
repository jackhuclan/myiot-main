// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using SuperSocket.Connection;
using SuperSocket.Server.Abstractions.Session;
using VgEAPClient.Socket.Models;

namespace VgEAPClient.Socket.Server;
public interface ISocketServer
{
    event OnHostRecvEvent OnHostRequest;
    event OnHostSessionConnectEvent OnHostSessionConnect;
    event OnHostSessionCloseEvent OnHostSessionClose;
    EapServerOptions Options { get; }

    List<IAppSession> SessionList { get; }

    void SetPipelineFilter();

    void SetPackageHandler();
    void SetSessionHandler();

    void SetConfigureSuperSocket();

    void Build();

    void Start();

    void Send(byte[] bytes);
}


public delegate Task OnHostRecvEvent(IAppSession session, EapMessage package);

public delegate Task OnHostSessionConnectEvent(IAppSession session);

public delegate Task OnHostSessionCloseEvent(IAppSession session, CloseEventArgs e);
