// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using VgEAPClient.Common.CNC.Common;
using VgEAPClient.Common.CNC.Status;

namespace VgEAPClient.Common.CNC;

public interface ICNCConnector
{
    event Action? OnCNCConnected;

    event Action? OnCNCDisconnected;

    event Action<Exception>? OnCNCConnectException;

    event Action<Exception>? OnCNCDisonnectException;

    event Action<Exception>? OnCNCDataReceivedException;

    event Action? OnCNCConnectFailed;

    event Action? OnCNCDataReceived;

    event Action? OnCNCClosed;

    event Action? OnCNCError;

    event Action? OnCNCDataSent;

    event Action<Exception>? OnCNCDataSentException;

    bool IsConnected { get; }

    /// <summary>
    /// 连接cnc
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Connect(CancellationToken cancellationToken);

    /// <summary>
    /// 断开cnc连接
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Disonnect(CancellationToken cancellationToken);

    /// <summary>
    /// 获取指定key的值
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string> RetrieveData(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// 解析断刀数据
    /// </summary>
    /// <param name="blockText"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    BrokenToolData ParseBrokenToolData(BrokenToolData brokenToolData, string blockText, CancellationToken cancellationToken = default);

    Task<bool> CncSetXYOfProgramZero(double x, double y, CancellationToken cancellationToken = default);

    Task<bool> CncLoadFile(LoadFileType loadFileType, string strFilePath, CancellationToken cancellationToken = default);

    Task<RetMsg> LoadCNCCommand(string cmd);


    List<string> EcList { get; set; }

    DrillCommonDataA _drillCommonDataA { get; set; }

    DrillStatusData _drillStatusData { get; set; }
}
