// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SuperSocket.ProtoBase;
using SuperSocket.Server.Abstractions.Host;
using SuperSocket.Server.Abstractions.Session;
using SuperSocket.Server.Host;
using VgEAPClient.Socket.Models;

namespace VgEAPClient.Socket.Server;
public class SocketServer<T, TPackageInfo> : ISocketServer
    where T : IPipelineFilter<TPackageInfo>, new()
    where TPackageInfo : EapMessage
{
    public event OnHostRecvEvent? OnHostRequest;
    public event OnHostSessionConnectEvent? OnHostSessionConnect;
    public event OnHostSessionCloseEvent? OnHostSessionClose;

    private readonly ILogger<SocketServer<T, TPackageInfo>> _logger;
    public EapServerOptions Options { get; private set; }
    public List<IAppSession> SessionList { get; private set; }

    public IHost? _host;

    public ISuperSocketHostBuilder<TPackageInfo>? _supperhost;

    public SocketServer(IOptions<EapServerOptions> options,
        ILoggerFactory loggerFactory)
    {
        Options = options.Value;
        _logger = loggerFactory.CreateLogger<SocketServer<T, TPackageInfo>>();
        SessionList = [];

    }

    public virtual void SetPipelineFilter()
    {
        //_supperhost?.UsePipelineFilter<T, TPackageInfo>();
    }

    public void SetPackageHandler()
    {
        _supperhost?.UsePackageHandler(async (session, package) =>
        {
            OnHostRequest?.Invoke(session, package);
            await Task.CompletedTask;
        });
    }

    public void SetSessionHandler()
    {
        _supperhost?.UseSessionHandler(
            async (s) =>
            {
                SessionList.Add(s);
                _logger.LogInformation($@"新增会话{s.SessionID} {s.RemoteEndPoint}");
                OnHostSessionConnect?.Invoke(s);
                await Task.CompletedTask;
            },
            async (s, e) =>
            {
                SessionList.Remove(s);
                _logger.LogInformation($@"断开会话{s.SessionID} {s.RemoteEndPoint} , 原因：{e.Reason}");
                OnHostSessionClose?.Invoke(s, e);
                await Task.CompletedTask;
            });
    }

    public void SetConfigureSuperSocket()
    {
        _supperhost?.ConfigureSuperSocket(options =>
        {
            options.Name = EapServerOptions.Options;
            options.Listeners =
            [
                new()
                {
                    Ip = "Any",
                    Port = Options.ListenerPort
                }
            ];
        });
    }

    public void Build()
    {
        _supperhost = SuperSocketHostBuilder.Create<TPackageInfo, T>();
        SetPipelineFilter();
        SetPackageHandler();
        SetSessionHandler();
        SetConfigureSuperSocket();
        _host = _supperhost.Build();
    }
    public void Start()
    {
        Build();
        _host?.RunAsync();
    }

    public async void Send(byte[] bytes)
    {
        List<IAppSession> SessionList_cur = [.. SessionList];
        foreach (IAppSession session in SessionList_cur)
        {
            if (session.State == SuperSocket.Server.Abstractions.SessionState.Connected)
            {
                await session.SendAsync(bytes);
            }
        }
    }
}

