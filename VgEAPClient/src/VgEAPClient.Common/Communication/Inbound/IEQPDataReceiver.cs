// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace VgEAPClient.Common.Communication.Inbound;

public interface IEQPDataReceiver
{
    event Func<AreYouThereReplyModel, Task<AreYouThereReplyModel>>? OnAreYouThereReplyReceived;

    event Func<DateTimeCommandModel, Task<DateTimeCommandModel>>? OnDateTimeCommandReceived;

    event Func<CIMMessageCommandModel, Task<CIMMessageCommandModel>>? OnCIMMessageCommandReceived;

    event Func<LotInfoDownloadCommandModel, Task<LotInfoDownloadCommandModel>>? OnLotInfoDownloadCommandReceived;

    event Func<RecipeValidationResultCommandModel, Task<RecipeValidationResultCommandModel>>? OnRecipeValidationResultCommandReceived;

    event Func<MaterialValidationRequestReplyModel, Task<MaterialValidationRequestReplyModel>>? OnMaterialValidationRequestReply;

    event Func<EQPCommunicationStatusRequestModel, Task<EQPCommunicationStatusRequestModel>>? OnEQPCommunicationStatusRequestReply;

    /// <summary>
    /// EAP收到请求消息时，给出的在线答复（PC通讯）
    /// </summary>
    /// <param name="incomingData"></param>
    /// <returns></returns>
    Task<AreYouThereReplyModel> AreYouThereReply(AreYouThereReplyModel incomingData);

    /// <summary>
    /// 下发时间给设备，同步设备时间和EAP时间同步
    /// </summary>
    /// <param name="incomingData"></param>
    /// <returns></returns>
    Task<DateTimeCommandModel> DateTimeCommand(DateTimeCommandModel incomingData);

    /// <summary>
    /// CIM通讯模式是指设备与EAP之间的通讯状态。包含CIM ON和CIM Off模式
    /// </summary>
    /// <param name="incomingData"></param>
    /// <returns></returns>
    Task<CIMMessageCommandModel> CIMMessageCommand(CIMMessageCommandModel incomingData);

    /// <summary>
    /// EAP下发生产任务信息时使用
    /// </summary>
    /// <param name="incomingData"></param>
    /// <returns></returns>
    Task<LotInfoDownloadCommandModel> LotInfoDownloadCommand(LotInfoDownloadCommandModel incomingData);

    /// <summary>
    /// EAP验证配方参数结果命令
    /// </summary>
    /// <param name="incomingData"></param>
    /// <returns></returns>
    Task<RecipeValidationResultCommandModel> RecipeValidationResultCommand(RecipeValidationResultCommandModel incomingData);

    /// <summary>
    /// 物料上机请求信号返回
    /// </summary>
    /// <param name="incomingData"></param>
    /// <returns></returns>
    Task<MaterialValidationRequestReplyModel> MaterialValidationRequestReply(MaterialValidationRequestReplyModel incomingData);

    /// <summary>
    /// EAP请求查询设备状态
    /// </summary>
    /// <param name="incomingData"></param>
    /// <returns></returns>
    Task<EQPCommunicationStatusRequestModel> EQPCommunicationStatusRequest(EQPCommunicationStatusRequestModel incomingData);

    void Configure(IWebHostBuilder webHostBuilder, IApplicationBuilder app);

}
