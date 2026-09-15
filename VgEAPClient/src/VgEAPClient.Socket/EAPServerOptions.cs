// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Socket;

public class EapServerOptions
{
    public const string Options = "EapServerOptions";

    /// <summary>
    /// Client 连接地址
    /// </summary>
    public string Host { get; set; } = string.Empty;
    /// <summary>
    /// Client 连接端口
    /// </summary>
    public int Port { get; set; } = 5200;
    /// <summary>
    /// 断开后是否重连
    /// </summary>
    public bool AutoReconnect { get; set; } = true;
    /// <summary>
    /// Client 连接是否启用
    /// </summary>
    public bool ClientEnable { get; set; } = false;
    /// <summary>
    /// Server监听端口
    /// </summary>
    public int ListenerPort { get; set; } = 502;
    /// <summary>
    /// Server监听是否启用
    /// </summary>
    public bool ListenerEnable { get; set; } = false;

    /// <summary>
    /// 心跳检测PC每间隔4s
    /// </summary>
    public int HeartBeatSeconds { get; set; } = 4;
    /// <summary>
    /// 数据上报频率
    /// </summary>
    public int DataCollectionReportSeconds { get; set; } = 60;
}


