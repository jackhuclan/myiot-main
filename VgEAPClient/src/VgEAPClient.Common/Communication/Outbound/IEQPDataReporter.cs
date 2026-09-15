// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Outbound;

public interface IEQPDataReporter
{
    /// <summary>
    /// 处理LotInfoRequest返回数据回调
    /// </summary>
    event Func<LotInfoRequestModel, Task<LotInfoRequestModel>>? OnLotInfoRequestReturn;

    /// <summary>
    /// 处理SendEQPCompletedReport返回数据回调
    /// </summary>
    event Func<EQPCompletedReportModel, Task<EQPCompletedReportModel>>? OnEQPCompletedReportReturn;

    /// <summary>
    /// 周期上报
    /// </summary>
    event Func<Task> OnTick;

    /// <summary>
    /// 设备使用此事件向EAP请求系统时间
    /// </summary>
    /// <param name="reportBody"></param>
    /// <returns></returns>
    Task SendEQPDateTimeRequest(EQPDateTimeRequestBody reportBody);

    /// <summary>
    /// CIM通讯模式是指设备与EAP之间的通讯状态。包含CIM ON和CIM Off模式
    /// </summary>
    Task SendEQPCommunicationStatusReport(EQPCommunicationStatusReportBody reportBody);

    /// <summary>
    /// 设备在CIM/ON的情况下开自动/手动
    /// </summary>
    /// <param name="reportBody"></param>
    /// <returns></returns>
    Task SendEQPRunningModeReport(EQPRunningModeReportBody reportBody);

    /// <summary>
    /// 当发生一个或多个报警时，设备应该上报所有报警信息给EAP
    /// </summary>
    /// <param name="reportBody"></param>
    /// <returns></returns>
    Task SendEQPAlarmReport(EQPAlarmReportBody reportBody);

    /// <summary>
    /// 设备请求生产任务信息时使用
    /// </summary>
    /// <param name="reportBody"></param>
    /// <returns></returns>
    Task SendLotInfoRequest(LotInfoRequestBody reportBody);

    /// <summary>
    /// 设备切换配方后，需上报设备当前配方。
    /// </summary>
    /// <param name="reportBody"></param>
    /// <returns></returns>
    Task SendEQPCurrentChangeRecipeReport(EQPCurrentChangeRecipeReportBody reportBody);

    /// <summary>
    /// 设备读取板件ID开始制程时上报进片报告
    /// </summary>
    /// <param name="reportBody"></param>
    /// <returns></returns>
    Task SendEQPReceiveJobReport(EQPReceiveJobReportBody reportBody);

    /// <summary>
    /// 设备制程结束时上报出片报告
    /// </summary>
    /// <param name="reportBody"></param>
    /// <returns></returns>
    Task SendEQPSendOutJobReport(EQPSendOutJobReportBody reportBody);

    /// <summary>
    /// 设备上报完工报告
    /// </summary>
    /// <param name="reportBody"></param>
    /// <returns></returns>
    Task SendEQPCompletedReport(EQPCompletedReportBody reportBody);

    /// <summary>
    /// 员工值机时上下机报告
    /// </summary>
    /// <param name="reportBody"></param>
    /// <returns></returns>
    Task SendUserCheckCardReport(UserCheckCardReportBody reportBody);

    /// <summary>
    /// 设备周期上报采集参数
    /// </summary>
    /// <param name="reportBody"></param>
    /// <returns></returns>
    Task SendEQPDataCollectionReport(EQPDataCollectionReportBody reportBody);

    /// <summary>
    /// 设备状态发生改变时，应触发相应的事件上报。
    /// </summary>
    /// <param name="reportBody"></param>
    /// <returns></returns>
    Task SendEQPStatusChangeReport(EQPStatusChangeReportBody reportBody);

    /// <summary>
    /// 每趟钻明码完成后，将钻的各轴明码与生产参数绑定上报
    /// </summary>
    /// <param name="reportBody"></param>
    /// <returns></returns>
    Task SendPanelProcessDataReport(PanelProcessDataReportBody reportBody);

    /// <summary>
    /// 当出现断刀异常时，设备需将此异常及当时相关刀具信息绑定上报
    /// </summary>
    /// <param name="reportBody"></param>
    /// <returns></returns>
    Task SendBrokenKnifeAlarmReport(BrokenKnifeAlarmReportBody reportBody);

    /// <summary>
    /// EQP启动的时候，将本机WebAPI服务的IP和Port上报到EAP，设备修改IP和Port的时候，需要上报该消息【开机即上报，修改后上报】
    /// </summary>
    /// <param name="reportBody"></param>
    /// <returns></returns>
    Task SendIPPortReport(IPPortReportBody reportBody);

    Task StartAsync(CancellationToken cancellationToken);


    /// <summary>
    /// 物料上机/下机请求
    /// </summary>
    /// <param name="reportBody"></param>
    /// <returns></returns>
    Task SendMaterialValidationRequest(MaterialValidationRequestBody reportBody);
    /// <summary>
    /// 物料使用报告
    /// </summary>
    /// <param name="reportBody"></param>
    /// <returns></returns>
    Task SendMaterialUseReport(MaterialUseReportBody reportBody);

}
