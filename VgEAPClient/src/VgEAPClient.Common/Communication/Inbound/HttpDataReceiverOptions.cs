// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Inbound;

public class HttpDataReceiverOptions
{
    /// <summary>
    /// 是否开启 HTTP 监听
    /// </summary>
    public bool Enabled { get; set; } = false;

    public string UrlPrefix { get; set; } = "http://*:5000";

    /// <summary>
    /// EAP收到请求消息时，给出的在线答复（PC通讯）
    /// </summary>
    public string AreYouThereReplyUrl { get; set; } = string.Empty;

    /// <summary>
    /// 下发时间给设备，同步设备时间和EAP时间同步
    /// </summary>
    public string DateTimeCommandUrl { get; set; } = string.Empty;

    /// <summary>
    /// CIM通讯模式是指设备与EAP之间的通讯状态。包含CIM ON和CIM Off模式
    /// </summary>
    public string CIMMessageCommandUrl { get; set; } = string.Empty;

    /// <summary>
    /// EAP下发生产任务信息时使用
    /// </summary>
    public string LotInfoDownloadCommandUrl { get; set; } = string.Empty;

    /// <summary>
    /// EAP验证配方参数结果命令
    /// </summary>
    public string RecipeValidationResultCommandUrl { get; set; } = string.Empty;

    /// <summary>
    /// 物料上机请求信号返回
    /// </summary>
    public string MaterialValidationRequestReplyUrl { get; set; } = string.Empty;

    /// <summary>
    /// EAP请求查询设备当前状态
    /// </summary>
    public string EQPCommunicationStatusRequestUrl { get; set; } = string.Empty;
}
