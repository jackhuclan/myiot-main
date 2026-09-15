// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using VgEAPClient.Socket.Models;

namespace VgEAPClient.Socket;
public interface IEQPSocketDataReporter
{

    /// <summary>
    /// 处理LotInfoRequest返回数据回调
    /// </summary>
    event Func<EapMessage, Task<EapMessage>>? OnLotInfoRequestReturn;
    /// <summary>
    /// 周期上报
    /// </summary>
    event Func<Task> OnTick;
    Task StartAsync(CancellationToken cancellationToken);


    /// <summary>
    /// 设备请求生产任务信息时使用
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    Task SendLotInfoRequest(EapMessage.EapBody body);

    /// <summary>
    /// 设备状态发生改变时，应触发相应的事件上报。
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    Task SendEQPStatusChangeReport(EapMessage.EapBody body);

    /// <summary>
    /// 设备制程结束时上报出片报告
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    Task SendEQPSendOutJobReport(EapMessage.EapBody body);

    /// <summary>
    /// 当发生一个或多个报警时，设备应该上报所有报警信息给EAP
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    Task SendEQPAlarmReport(EapMessage.EapBody body);

    /// <summary>
    /// 当任一字段有变更时,发送设备信息给EAP
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    Task SendEquipmentInfoReport(EapMessage.EapBody body);

    /// <summary>
    /// 员工值机时上下机报告
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    Task<EapMessage> SendUserCheckCardReport(EapMessage.EapBody body);

    /// <summary>
    /// 设备周期上报采集参数
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    Task SendEQPDataCollectionReport(EapMessage.EapBody body);

    /// <summary>
    /// 当出现断刀异常时，设备需将此异常及当时相关刀具信息绑定上报
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    Task SendBrokenKnifeAlarmReport(EapMessage.EapBody body);
}
