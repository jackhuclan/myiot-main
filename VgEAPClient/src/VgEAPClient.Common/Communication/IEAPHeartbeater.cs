// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using VgEAPClient.Common.Communication.Outbound;

namespace VgEAPClient.Common.Communication;

public interface IEAPHeartbeater
{
    /// <summary>
    /// eap连接状态发生改变
    /// </summary>
    event Func<bool, Task> OnEAPConnectedChanged;
    /// <summary>
    /// eap是否连接
    /// </summary>
    bool Connected { get; }
    /// <summary>
    /// 心跳检测PC每间隔4s向EAP请求是否在线（PC通讯）
    /// </summary>
    Task SendAreYouThere(AreYouThereBody reportBody);
    Task StartAsync(CancellationToken cancellationToken);
}
