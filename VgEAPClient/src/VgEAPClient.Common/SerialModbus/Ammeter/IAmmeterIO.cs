// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common;


public interface IAmmeterIO
{
    /// <summary>
    /// 电表型号
    /// </summary>
    /// <returns></returns>
    string GetAmmeterModel();

    /// <summary>
    /// 开始电表采集线程。对已经Start Run 的实例再次调用此接口，不会有效果。
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="opions"></param>
    void StartRun(EAPClientOptions opions);

    /// <summary>
    /// 停止电表采集线程
    /// </summary>
    /// <param name="reason"></param>
    void StopRun(string reason = "");

    /// <summary>
    /// 取出累计消耗的电能数值
    /// </summary>
    /// <returns></returns>
    double GetConsumedKWH();

    /// <summary>
    /// 取出日志
    /// </summary>
    /// <returns></returns>
    string PopImportantLog();
}
