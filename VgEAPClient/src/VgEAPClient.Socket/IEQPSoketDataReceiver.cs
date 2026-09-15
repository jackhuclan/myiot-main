// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using VgEAPClient.Socket.Models;

namespace VgEAPClient.Socket;
public interface IEQPSoketDataReceiver
{
    event Func<EapMessage, Task<EapMessage>>? OnInitialDataRequestReceived;

    event Func<EapMessage, Task<EapMessage>>? OnLotInfoDownloadCommandReceived;

    event Func<EapMessage, Task<EapMessage>>? OnDateTimeCommandReceived;

    /// <summary>
    /// EAP向设备请求目前状态 回复设备当前信息
    /// </summary>
    /// <param name="eap"></param>
    /// <returns></returns>
    Task<EapMessage> InitialDataRequest(EapMessage eap);

    /// <summary>
    /// EAP下发生产任务信息时使用
    /// </summary>
    /// <param name="eap"></param>
    /// <returns></returns>
    Task<EapMessage> LotInfoDownloadCommand(EapMessage eap);

    /// <summary>
    /// 下发时间给设备，同步设备时间和EAP时间同步
    /// </summary>
    /// <param name="eap"></param>
    /// <returns></returns>
    Task<EapMessage> DateTimeCommand(EapMessage eap);
}
