// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Outbound;

public class HttpDataReporterOptions
{
    /// <summary>
    /// 心跳检测PC每间隔4s
    /// </summary>
    public int HeartBeatSeconds { get; set; } = 4;
    /// <summary>
    /// 数据上报频率
    /// </summary>
    public int DataCollectionReportSeconds { get; set; } = 60;
    /// <summary>
    /// 心跳检测PC每间隔4s向EAP请求是否在线（PC通讯）URL
    /// </summary>
    public string AreYouThereUrl { get; set; } = string.Empty;
    /// <summary>
    /// EQP启动的时候，将本机WebAPI服务的IP和Port上报到EAP，设备修改IP和Port的时候，需要上报该消息【开机即上报，修改后上报】（PC通讯）URL
    /// </summary>
    public string IPPortReportUrl { get; set; } = string.Empty;
    /// <summary>
    /// 设备使用此事件向EAP请求系统时间
    /// </summary>
    public string EQPDateTimeRequestUrl { get; set; } = string.Empty;
    /// <summary>
    /// CIM通讯模式是指设备与EAP之间的通讯状态。包含CIM ON和CIM Off模式URL
    /// </summary>
    public string EQPCommunicationStatusReportUrl { get; set; } = string.Empty;
    /// <summary>
    /// 设备在CIM/ON的情况下开自动/手动URL
    /// </summary>
    public string EQPRunningModeReportUrl { get; set; } = string.Empty;
    /// <summary>
    /// 当发生一个或多个报警时，设备应该上报所有报警信息给EAPURL
    /// </summary>
    public string EQPAlarmReportUrl { get; set; } = string.Empty;
    /// <summary>
    /// 设备请求生产任务信息时使用URL
    /// </summary>
    public string LotInfoRequestUrl { get; set; } = string.Empty;
    /// <summary>
    /// 设备切换配方后，需上报设备当前配方。
    /// </summary>
    public string EQPCurrentChangeRecipeReportUrl { get; set; } = string.Empty;
    /// <summary>
    /// 设备读取板件ID开始制程时上报进片报告URL
    /// </summary>
    public string EQPReceiveJobReportUrl { get; set; } = string.Empty;
    /// <summary>
    /// 设备制程结束时上报出片报告URL
    /// </summary>
    public string EQPSendOutJobReportUrl { get; set; } = string.Empty;
    /// <summary>
    /// 设备上报完工报告URL
    /// </summary>
    public string EQPCompletedReportUrl { get; set; } = string.Empty;
    /// <summary>
    /// 员工值机时上下机报告URL
    /// </summary>
    public string UserCheckCardReportUrl { get; set; } = string.Empty;
    /// <summary>
    /// 设备周期上报采集参数URL
    /// </summary>
    public string EQPDataCollectionReportUrl { get; set; } = string.Empty;
    /// <summary>
    /// 设备状态发生改变时，应触发相应的事件上报。URL
    /// </summary>
    public string EQPStatusChangeReportUrl { get; set; } = string.Empty;
    /// <summary>
    /// 设备上报断刀报警URL
    /// </summary>
    public string BrokenKnifeAlarmReportUrl { get; set; } = string.Empty;
    /// <summary>
    /// 物料上机/下机请求URL
    /// </summary>
    public string MaterialValidationRequestUrl { get; set; } = string.Empty;
    /// <summary>
    /// 物料使用报告URL
    /// </summary>
    public string MaterialUseReportUrl { get; set; } = string.Empty;
    /// <summary>
    /// 按片上报数据
    /// </summary>
    public string PanelProcessDataReportUrl { get; set; } = string.Empty;
}
