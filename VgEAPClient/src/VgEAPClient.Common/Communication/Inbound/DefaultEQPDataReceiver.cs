// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;

namespace VgEAPClient.Common.Communication.Inbound;

public class DefaultEQPDataReceiver : IEQPDataReceiver
{
    public readonly HttpDataReceiverOptions _httpDataReceiverOptions;
    public readonly IObjectFactory _objectFactory;

    public DefaultEQPDataReceiver(IOptions<HttpDataReceiverOptions> options,
        IObjectFactory objectFactory)
    {
        _httpDataReceiverOptions = options.Value;
        _objectFactory = objectFactory;
    }

    public event Func<AreYouThereReplyModel, Task<AreYouThereReplyModel>>? OnAreYouThereReplyReceived;

    public event Func<DateTimeCommandModel, Task<DateTimeCommandModel>>? OnDateTimeCommandReceived;

    public event Func<CIMMessageCommandModel, Task<CIMMessageCommandModel>>? OnCIMMessageCommandReceived;

    public event Func<LotInfoDownloadCommandModel, Task<LotInfoDownloadCommandModel>>? OnLotInfoDownloadCommandReceived;

    public event Func<RecipeValidationResultCommandModel, Task<RecipeValidationResultCommandModel>>? OnRecipeValidationResultCommandReceived;

    public event Func<MaterialValidationRequestReplyModel, Task<MaterialValidationRequestReplyModel>>? OnMaterialValidationRequestReply;

    public event Func<EQPCommunicationStatusRequestModel, Task<EQPCommunicationStatusRequestModel>>? OnEQPCommunicationStatusRequestReply;

    public Task<AreYouThereReplyModel> AreYouThereReply(AreYouThereReplyModel incomingData) => OnAreYouThereReplyReceived?.Invoke(incomingData) ?? Task.FromResult(incomingData);

    public Task<DateTimeCommandModel> DateTimeCommand(DateTimeCommandModel incomingData) => OnDateTimeCommandReceived?.Invoke(incomingData) ?? Task.FromResult(incomingData);

    public Task<CIMMessageCommandModel> CIMMessageCommand(CIMMessageCommandModel incomingData) => OnCIMMessageCommandReceived?.Invoke(incomingData) ?? Task.FromResult(incomingData);

    public Task<LotInfoDownloadCommandModel> LotInfoDownloadCommand(LotInfoDownloadCommandModel incomingData) => OnLotInfoDownloadCommandReceived?.Invoke(incomingData) ?? Task.FromResult(incomingData);

    public Task<RecipeValidationResultCommandModel> RecipeValidationResultCommand(RecipeValidationResultCommandModel incomingData) => OnRecipeValidationResultCommandReceived?.Invoke(incomingData) ?? Task.FromResult(incomingData);

    public Task<MaterialValidationRequestReplyModel> MaterialValidationRequestReply(MaterialValidationRequestReplyModel incomingData) => OnMaterialValidationRequestReply?.Invoke(incomingData) ?? Task.FromResult(incomingData);

    public Task<EQPCommunicationStatusRequestModel> EQPCommunicationStatusRequest(EQPCommunicationStatusRequestModel incomingData) => OnEQPCommunicationStatusRequestReply?.Invoke(incomingData) ?? Task.FromResult(incomingData);

    public virtual void Configure(IWebHostBuilder webHostBuilder, IApplicationBuilder app)
    {
        if (!string.IsNullOrWhiteSpace(_httpDataReceiverOptions.AreYouThereReplyUrl))
        {
            var areYouThereReplyReceiver = _objectFactory.CreateObject<AreYouThereReplyReceiver>();
            app.Map(_httpDataReceiverOptions.AreYouThereReplyUrl.TrimStart(_httpDataReceiverOptions.UrlPrefix), app => app.Run(areYouThereReplyReceiver.Invoke));
        }

        if (!string.IsNullOrWhiteSpace(_httpDataReceiverOptions.CIMMessageCommandUrl))
        {
            var cIMMessageCommandReceiver = _objectFactory.CreateObject<CIMMessageCommandReceiver>();
            app.Map(_httpDataReceiverOptions.CIMMessageCommandUrl.TrimStart(_httpDataReceiverOptions.UrlPrefix), app => app.Run(cIMMessageCommandReceiver.Invoke));
        }

        if (!string.IsNullOrWhiteSpace(_httpDataReceiverOptions.DateTimeCommandUrl))
        {
            var dateTimeCommandReceiver = _objectFactory.CreateObject<DateTimeCommandReceiver>();
            app.Map(_httpDataReceiverOptions.DateTimeCommandUrl.TrimStart(_httpDataReceiverOptions.UrlPrefix), app => app.Run(dateTimeCommandReceiver.Invoke));
        }

        if (!string.IsNullOrWhiteSpace(_httpDataReceiverOptions.LotInfoDownloadCommandUrl))
        {
            var lotInfoDownloadCommandReceiver = _objectFactory.CreateObject<LotInfoDownloadCommandReceiver>();
            app.Map(_httpDataReceiverOptions.LotInfoDownloadCommandUrl.TrimStart(_httpDataReceiverOptions.UrlPrefix), app => app.Run(lotInfoDownloadCommandReceiver.Invoke));
        }

        if (!string.IsNullOrWhiteSpace(_httpDataReceiverOptions.RecipeValidationResultCommandUrl))
        {
            var recipeValidationResultCommandReceiver = _objectFactory.CreateObject<RecipeValidationResultCommandReceiver>();
            app.Map(_httpDataReceiverOptions.RecipeValidationResultCommandUrl.TrimStart(_httpDataReceiverOptions.UrlPrefix), app => app.Run(recipeValidationResultCommandReceiver.Invoke));
        }

        if (!string.IsNullOrWhiteSpace(_httpDataReceiverOptions.MaterialValidationRequestReplyUrl))
        {
            var materialValidationRequestReplyReceiver = _objectFactory.CreateObject<MaterialValidationRequestReplyReceiver>();
            app.Map(_httpDataReceiverOptions.MaterialValidationRequestReplyUrl.TrimStart(_httpDataReceiverOptions.UrlPrefix), app => app.Run(materialValidationRequestReplyReceiver.Invoke));
        }

        if (!string.IsNullOrWhiteSpace(_httpDataReceiverOptions.EQPCommunicationStatusRequestUrl))
        {
            var eQPCommunicationStatusRequestReceiver = _objectFactory.CreateObject<EQPCommunicationStatusRequestReceiver>();
            app.Map(_httpDataReceiverOptions.EQPCommunicationStatusRequestUrl.TrimStart(_httpDataReceiverOptions.UrlPrefix), app => app.Run(eQPCommunicationStatusRequestReceiver.Invoke));
        }
    }
}
