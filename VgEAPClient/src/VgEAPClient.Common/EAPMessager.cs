// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common;

/// <summary>
/// 向EAP发送/接收消息
/// </summary>
public class EAPMessager
{
    private readonly IEAPConnector _connector;

    public EAPMessager(IEAPConnector connector)
    {
        _connector = connector;
    }

    public Task AreYouThere()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// 设备上报连接状态CIM OFF/ON
    /// </summary>
    /// <returns></returns>
    public Task EQPCommunicationStatusReport()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// 设备使用此事件向EAP请求系统时间
    /// </summary>
    /// <returns></returns>
    public Task EQPDateTimeRequest() { return Task.CompletedTask; }
    /// <summary>
    /// 设备触发不同的状态上报
    /// </summary>
    /// <returns></returns>
    public Task EQPStatusChangeReport() { return Task.CompletedTask; }

    /// <summary>
    /// 设备在CIM/ON的状态下开自动/手动
    /// </summary>
    /// <returns></returns>
    public Task EQPRunningModeReport() { return Task.CompletedTask; }
}
